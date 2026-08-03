// Copyright (c) coherence ApS.
// See the license file in the package root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coherence.Cloud;
using Coherence.Connection;
using Coherence.Toolkit;
using Coherence.UI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Coherence.Samples.WorldDialog
{
    using System.Net.Sockets;
    using Runtime;
    using UnityEngine.EventSystems;

    public class WorldDialogUI : MonoBehaviour
    {
        private const int MaxReconnectionAttempts = 5;

        #region References
        [Header("References")]
        public GameObject connectDialog;
        public GameObject disconnectDialog;
        public GameObject noWorldsObject;
        public GameObject loadingSpinner;
        public Button refreshWorldsButton;
        public Button joinButton;
        public ConnectDialogWorldView templateWorldView;
        public Text worldTitleText;
        public GameObject popupDialog;
        public Text popupText;
        public Text popupTitleText;
        public Button popupDismissButton;
        #endregion

        private CoherenceBridge bridge;
        private CoherenceCloudLogin cloudLogin;
        private IReadOnlyList<WorldData> availableCloudWorlds = new List<WorldData>();
        private Coroutine cloudServiceReady;
        private string initialWorldTitle;
        private WorldData localWorld;
        private bool? localWorldExists;
        private Coroutine localWorldRefresher;
        private readonly ListView worldsListView = new();
        private WorldData? lastJoinedWorld;
        private int reconnectionTriesLeft = MaxReconnectionAttempts;

        private PlayerAccount PlayerAccount => cloudLogin ? cloudLogin.PlayerAccount : null;
        private WorldsService CloudWorlds => PlayerAccount?.Services?.Worlds;

        #region Unity Events
        private void OnEnable()
        {
            var eventSystem = EventSystem.current;
            if (!eventSystem)
            {
                eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                eventSystem.gameObject.AddComponent<StandaloneInputModule>();
                Debug.LogWarning("EventSystem not found on the scene. Adding one now.\nConsider creating an EventSystem yourself to forward UI input.", eventSystem);
            }

            if (!bridge && !CoherenceBridgeStore.TryGetBridge(gameObject.scene, out bridge))
            {
                Debug.LogError($"{nameof(CoherenceBridge)} required on the scene.\n" +
                               "Add one via 'GameObject > coherence > Coherence Bridge'.", this);
                return;
            }

            if (!FindAnyObjectByType<EventSystem>())
            {
                Debug.LogError($"{nameof(EventSystem)} required on the scene.\n" +
                               "Add one via 'GameObject > UI > Event System'.", this);
            }

            bridge.onConnected.AddListener(OnBridgeConnected);
            bridge.onDisconnected.AddListener(OnBridgeDisconnected);
            bridge.onConnectionError.AddListener(OnConnectionError);
            noWorldsObject.SetActive(true);
            joinButton.interactable = false;

            if (!string.IsNullOrEmpty(RuntimeSettings.Instance.ProjectID))
            {
                cloudServiceReady = StartCoroutine(WaitForCloudService());
            }
            else
            {
                refreshWorldsButton.gameObject.SetActive(false);
            }

            localWorldRefresher = StartCoroutine(LocalWorldRefresher());
            UpdateDialogsVisibility();
        }

        private void OnDisable()
        {
            if (bridge)
            {
                bridge.onConnected.RemoveListener(OnBridgeConnected);
                bridge.onDisconnected.RemoveListener(OnBridgeDisconnected);
                bridge.onConnectionError.RemoveListener(OnConnectionError);
            }

            if (localWorldRefresher != null)
            {
                StopCoroutine(localWorldRefresher);
            }

            if (cloudServiceReady != null)
            {
                StopCoroutine(cloudServiceReady);
            }
        }

        private void Awake()
        {
            if (SimulatorUtility.IsSimulator)
            {
                gameObject.SetActive(false);
            }

            worldsListView.Template = templateWorldView;
            worldsListView.onSelectionChange = view =>
            {
                joinButton.interactable = view && view.WorldData.WorldId != default(WorldData).WorldId;
            };
        }

        private IEnumerator Start()
        {
            initialWorldTitle = worldTitleText.text;
            refreshWorldsButton.onClick.AddListener(RefreshWorlds);
            joinButton.onClick.AddListener(OnClickJoin);
            popupDismissButton.onClick.AddListener(HideError);

            popupDialog.SetActive(false);
            templateWorldView.gameObject.SetActive(false);

            while (localWorldExists is null)
            {
                yield return null;
            }

            if (localWorldExists is false)
            {
                LogInToCoherenceCloud();
            }
        }
        #endregion

        #region Cloud Requests
        private void LogInToCoherenceCloud()
        {
            if (!cloudLogin && !TryGetComponent(out cloudLogin) && !(cloudLogin = FindAnyObjectByType<CoherenceCloudLogin>()))
            {
                cloudLogin = gameObject.AddComponent<CoherenceCloudLogin>();
            }

            cloudLogin.LogInAsync().OnFail(error =>
            {
                var errorMessage = error.Type switch
                {
                    LoginErrorType.SchemaNotFound => "Logging in failed because local schema has not been uploaded to the Cloud.\n\nYou can upload local schema via <b>coherence > Upload Schema</b>.",
                    LoginErrorType.NoProjectSelected => "Logging in failed because no project was selected.\n\nYou can select a project via <b>coherence > Hub > Cloud</b>.",
                    LoginErrorType.ServerError => "Logging in failed because of a server error.",
                    LoginErrorType.InvalidCredentials => "Logging in failed because invalid credentials were provided.",
                    LoginErrorType.InvalidResponse => "Logging in failed because was unable to deserialize the response from the server.",
                    LoginErrorType.TooManyRequests => "Logging in failed because too many requests have been sent within a short amount of time.\n\nPlease slow down the rate of sending requests, and try again later.",
                    LoginErrorType.ConnectionError => "Logging in failed because of connection failure.",
                    LoginErrorType.AlreadyLoggedIn => $"The cloud services are already connected to a player account. You have to call {nameof(PlayerAccount)}.{nameof(Cloud.PlayerAccount.Logout)}. before attempting to log in again.",
                    LoginErrorType.ConcurrentConnection
                        => "We have received a concurrent connection for your Player Account. Your current credentials will be invalidated.\n\n" +
                        "Usually this happens when a concurrent connection is detected, e.g. running multiple game clients for the same player.\n\n" +
                        "When this happens the game should present a prompt to the player to inform them that there is another instance of the game running. " +
                        "The game should wait for player input and never try to reconnect on its own or else the two game clients would disconnect each other indefinitely.",
                    LoginErrorType.InvalidConfig => "Logging in failed because of invalid configuration in Online Dashboard." +
                                               "\nMake sure that the authentication method has been enabled and all required configuration has been provided in Project Settings." +
                                               "\nOnline Dashboard can found be found at: https://coherence.io/dashboard",
                    LoginErrorType.OneTimeCodeExpired => "Logging in failed because the provided ticket has expired.",
                    LoginErrorType.OneTimeCodeNotFound => "Logging in failed because no account has been linked to the authentication method in question. Pass an 'autoSignup' value of 'true' to automatically create a new account if one does not exist yet.",
                    LoginErrorType.IdentityLimit => "Logging in failed because identity limit has been reached.",
                    LoginErrorType.IdentityNotFound => "Logging in failed because provided identity not found",
                    LoginErrorType.IdentityTaken => "Logging in failed because the identity is already linked to another account. Pass a 'force' value of 'true' to automatically unlink the authentication method from the other player account.",
                    LoginErrorType.IdentityTotalLimit => "Logging in failed because maximum allowed number of identities has been reached.",
                    LoginErrorType.InvalidInput => "Logging in failed due to invalid input.",
                    LoginErrorType.PasswordNotSet => "Logging in failed because password has not been set for the player account.",
                    LoginErrorType.UsernameNotAvailable => "Logging in failed because the provided username is already taken by another player account.",
                    LoginErrorType.InternalException => "Logging in failed because of an internal exception.",
                    _ => error.Message,
                };

                ShowError("Logging in Failed", errorMessage);
                Debug.LogError(errorMessage, this);
            });
        }

        private IEnumerator WaitForCloudService()
        {
            ShowLoadingState();

            while (CloudWorlds is not { IsLoggedIn : true })
            {
                yield return null;
            }

            HideLoadingState();

            RefreshWorlds();
            cloudServiceReady = null;
        }

        private void RefreshWorlds()
        {
            if (CloudWorlds is { IsLoggedIn : true })
            {
                ShowLoadingState();
                CloudWorlds.FetchWorlds(OnWorldsFetched);
            }
        }
        #endregion

        #region Request Callbacks
        private void OnClickJoin()
        {
            reconnectionTriesLeft = MaxReconnectionAttempts;
            ShowLoadingState();
            lastJoinedWorld = worldsListView.Selection.WorldData;
            bridge.JoinWorld(lastJoinedWorld.Value);
        }

        private void OnWorldsFetched(RequestResponse<IReadOnlyList<WorldData>> requestResponse)
        {
            HideLoadingState();

            if (requestResponse.Status != RequestStatus.Success)
            {
                var errorMessage = GetErrorFromResponse(requestResponse);
                ShowError("Error fetching worlds", errorMessage);
                Debug.LogException(requestResponse.Exception);
                return;
            }

            availableCloudWorlds = requestResponse.Result ?? new List<WorldData>(0);
            RefreshWorldsListView();
        }

        public void Disconnect()
        {
            lastJoinedWorld = null;
            bridge.Disconnect();
        }
        #endregion

        #region Local World
        private IEnumerator LocalWorldRefresher()
        {
            while (true)
            {
                var task = ReplicationServerUtils.PingHttpServerAsync(RuntimeSettings.Instance.LocalHost,
                    RuntimeSettings.Instance.WorldsAPIPort);
                yield return new WaitUntil(() => task.IsCompleted);

                var result = task.Result;

                var lastWorld = localWorld;
                localWorld = result ? WorldData.GetLocalWorld(RuntimeSettings.Instance.LocalHost) : default;
                localWorldExists = localWorld.WorldId != 0u;

                if (lastWorld.WorldId != localWorld.WorldId)
                {
                    RefreshWorldsListView();
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
        #endregion

        #region Error Handling
        private void ShowError(string title, string message = "Unknown Error")
        {
            popupDialog.SetActive(true);
            popupTitleText.text = title;
            popupText.text = message;
        }

        private void HideError()
        {
            popupDialog.SetActive(false);
        }

        private void OnConnectionError(CoherenceBridge _, ConnectionException exception)
        {
            HideLoadingState();

            var (title, message) = exception.GetPrettyMessage();
            Debug.LogError(message, this);

            if (!ShouldTryToReconnect(exception))
            {
                ShowError(title, message);
                return;
            }

            // Try rejoining world running on local replication server
            var worldId = lastJoinedWorld.Value.WorldId;
            if (worldId == localWorld.WorldId)
            {
                reconnectionTriesLeft--;
                ShowLoadingState();
                bridge.JoinWorld(localWorld);
                return;
            }

            if (CloudWorlds is not { IsLoggedIn : true })
            {
                ShowError(title, message);
                return;
            }

            // Try refetching the worlds list from coherence Cloud
            // and rejoining the world if it still exists.
            reconnectionTriesLeft--;
            ShowLoadingState();
            CloudWorlds.FetchWorlds(OnCloudWorldsRefetched);

            bool ShouldTryToReconnect(Exception exception)
                // If the world was shut down by the backend (e.g. because of running out of memory),
                // try to reconnect in case the backend restarts the world.
                => exception is ConnectionClosedException { InnerException: SocketException { SocketErrorCode: SocketError.Shutdown } }
                             or ConnectionTimeoutException
                // If we keep getting an error every time we try to reconnect, give up after a few tries.
                && reconnectionTriesLeft > 0
                && lastJoinedWorld.HasValue;

            void OnCloudWorldsRefetched(RequestResponse<IReadOnlyList<WorldData>> requestResponse)
            {
                HideLoadingState();

                if (requestResponse.Status != RequestStatus.Success)
                {
                    Debug.LogException(requestResponse.Exception);
                    ShowError(title, message);
                    return;
                }

                availableCloudWorlds = requestResponse.Result ?? Array.Empty<WorldData>();
                RefreshWorldsListView();
                worldsListView.Selection = worldsListView.Views.FirstOrDefault(x => x.WorldData.WorldId == worldId);
                var updatedWorldData = availableCloudWorlds.FirstOrDefault(w => w.WorldId == worldId);
                if (updatedWorldData.WorldId != worldId)
                {
                    reconnectionTriesLeft--;
                    if (reconnectionTriesLeft > 0)
                    {
                        ShowLoadingState();
                        CloudWorlds.FetchWorlds(OnCloudWorldsRefetched);
                        return;
                    }

                    Debug.LogError($"Reconnection failed: world with id '{worldId}' no longer found.", this);
                    ShowError(title, message);
                    return;
                }

                ShowLoadingState();
                bridge.JoinWorld(updatedWorldData);
            }
        }

        private void OnBridgeDisconnected(CoherenceBridge _, ConnectionCloseReason reason) => UpdateDialogsVisibility();

        private void OnBridgeConnected(CoherenceBridge _)
        {
            reconnectionTriesLeft = MaxReconnectionAttempts;
            UpdateDialogsVisibility();
        }

        private static string GetErrorFromResponse<T>(RequestResponse<T> requestResponse)
        {
            if (requestResponse.Exception is not RequestException requestException)
            {
                return default;
            }

            return requestException.ErrorCode switch
            {
                ErrorCode.InvalidCredentials => "Invalid authentication credentials, please login again.",
                ErrorCode.TooManyRequests => "Too many requests. Please try again in a moment.",
                ErrorCode.ProjectNotFound => "Project not found. Please check that the runtime key is properly setup.",
                ErrorCode.SchemaNotFound => "Schema not found. Please check if the schema currently used by the project matches the one used by the replication server.",
                ErrorCode.RSVersionNotFound => "Replication server version not found. Please check that the version of the replication server is valid.",
                ErrorCode.SimNotFound => "Simulator not found. Please check that the slug and the schema are valid and that the simulator has been uploaded.",
                ErrorCode.MultiSimNotListening => "The multi-room simulator used for this room is not listening on the required ports. Please check your multi-room sim setup.",
                ErrorCode.RoomsSimulatorsNotEnabled => "Simulator not enabled. Please make sure that simulators are enabled in the Online Dashboard.",
                ErrorCode.RoomsSimulatorsNotUploaded => "Simulator not uploaded. You can use the coherence Hub to build and upload Simulators.",
                ErrorCode.RoomsVersionNotFound => "Version not found. Please make sure that client uses the correct 'sim-slug'.",
                ErrorCode.RoomsSchemaNotFound => "Schema not found. Please check if the schema currently used by the project matches the one used by the replication server.",
                ErrorCode.RoomsRegionNotFound => "Region not found. Please make sure that the selected region is enabled in the Online Dashboard.",
                ErrorCode.RoomsInvalidTagOrKeyValueEntry => "Validation of tag and key/value entries failed. Please check if number and size of entries is within limits.",
                ErrorCode.RoomsCCULimit => "Room ccu limit for project exceeded.",
                ErrorCode.RoomsNotFound => "Room not found. Please refresh room list.",
                ErrorCode.RoomsInvalidSecret => "Invalid room secret. Please make sure that the secret matches the one received on room creation.",
                ErrorCode.RoomsInvalidMaxPlayers => "Room Max Players must be a value between 1 and the upper limit configured on the project dashboard.",
                ErrorCode.InvalidMatchMakingConfig => "Invalid matchmaking configuration. Please make sure that the matchmaking feature was properly configured in the Online Dashboard.",
                ErrorCode.ClientPermission => "The client has been restricted from accessing this feature. Please check the Project Settings in the Online Dashboard.",
                ErrorCode.CreditLimit => "Monthly credit limit exceeded. Please check your organization credit usage in the Online Dashboard.",
                ErrorCode.InDeployment => "One or more online resources are currently being provisioned. Please retry the request.",
                ErrorCode.FeatureDisabled => "Requested feature is disabled, make sure you enable it in Project Settings in the Online Dashboard.",
                ErrorCode.InvalidRoomLimit => "Room max players limit must be between 1 and 100.",
                ErrorCode.LobbyInvalidAttribute => "A specified Attribute is invalid.",
                ErrorCode.LobbyNameTooLong => "Lobby name must be shorter than 64 characters.",
                ErrorCode.LobbyTagTooLong => "Lobby tag must be shorter than 16 characters.",
                ErrorCode.LobbyNotFound => "Requested Lobby wasn't found.",
                ErrorCode.LobbyAttributeSizeLimit => "A specified Attribute has surpassed the allowed limits. Lobby limit: 2048. Player limit: 256. Attribute size is calculated off key length + value length of all attributes combined.",
                ErrorCode.LobbyNameAlreadyExists => "A lobby with this name already exists.",
                ErrorCode.LobbyRegionNotFound => "Specified region for this Lobby wasn't found.",
                ErrorCode.LobbyInvalidSecret => "Invalid secret specified for lobby.",
                ErrorCode.LobbyFull => "This lobby is currently full.",
                ErrorCode.LobbyActionNotAllowed => "You're not allowed to perform this action on the lobby.",
                ErrorCode.LobbyInvalidFilter => "The provided filter is invalid. You can use Filter.ToString to debug the built filter you're sending.",
                ErrorCode.LobbyNotCompatible => "Schema not found. Please check if the schema currently used by the project matches the one used by the replication server.",
                ErrorCode.LobbySimulatorNotEnabled => "Simulator not enabled. Please make sure that simulators are enabled in the Online Dashboard.",
                ErrorCode.LobbySimulatorNotUploaded => "Simulator not uploaded. You can use the coherence Hub to build and upload Simulators.",
                ErrorCode.LobbyLimit => "You cannot join more than three lobbies simultaneously.",
                ErrorCode.LoginInvalidUsername => "Username given is invalid. Only alphanumeric, dashes and underscore characters are allowed. It must start with a letter and end with a letter/number. No double dash/underscore characters are allowed (-- or __).",
                ErrorCode.LoginInvalidPassword => "Password given is invalid. Password cannot be empty.",
                ErrorCode.RestrictedModeCapReached => "Total player capacity for restricted mode server reached.",
                ErrorCode.LoginDisabled => "This authentication method is disabled.",
                ErrorCode.InvalidConfig => "This authentication method has not been configured properly and thus can not be used.",
                ErrorCode.LoginInvalidApp => "The provided App ID is invalid.",
                ErrorCode.OneTimeCodeExpired => "The one-time code has already expired.",
                ErrorCode.OneTimeCodeNotFound => "The one-time code was not found.",
                ErrorCode.LoginNotFound => "No player account has been linked to the authentication method that was used.",
                _ => requestException.Message,
            };
        }
        #endregion

        #region Update UI
        private void RefreshWorldsListView()
        {
            var allWorlds = availableCloudWorlds.ToList();
            if (localWorldExists is true)
            {
                allWorlds.Add(localWorld);
            }

            noWorldsObject.SetActive(allWorlds.Count == 0);
            worldsListView.SetSource(allWorlds);
            worldTitleText.text = $"{initialWorldTitle} ({allWorlds.Count})";
        }

        private void UpdateDialogsVisibility()
        {
            HideLoadingState();
            connectDialog.SetActive(!bridge.IsConnected);
            disconnectDialog.SetActive(bridge.IsConnected);
        }

        private void HideLoadingState()
        {
            loadingSpinner.SetActive(false);
            joinButton.interactable = worldsListView != null && worldsListView.Selection != default
                                                             && worldsListView.Selection.WorldData.WorldId != default(WorldData).WorldId;
        }

        private void ShowLoadingState()
        {
            loadingSpinner.SetActive(true);
            joinButton.interactable = false;
            noWorldsObject.SetActive(false);
        }
        #endregion
    }

    internal class ListView
    {
        public ConnectDialogWorldView Template;
        public Action<ConnectDialogWorldView> onSelectionChange;

        public ConnectDialogWorldView Selection
        {
            get => selection;
            set
            {
                if (selection != value)
                {
                    selection = value;
                    lastSelectedId = selection == default ? default : selection.WorldData.WorldId;
                    onSelectionChange?.Invoke(Selection);
                    foreach (var viewRow in Views)
                    {
                        viewRow.IsSelected = selection == viewRow;
                    }
                }
            }
        }

        public List<ConnectDialogWorldView> Views { get; }
        private ConnectDialogWorldView selection;
        private ulong lastSelectedId;

        public ListView(int capacity = 50)
        {
            Views = new List<ConnectDialogWorldView>(capacity);
        }

        public void SetSource(IReadOnlyList<WorldData> dataSource)
        {
            Selection = default;
            Clear();

            if (dataSource.Count <= 0)
            {
                return;
            }

            var sortedData = dataSource.ToList();
            sortedData.Sort((worldA, worldB) =>
            {
                var strCompare = String.CompareOrdinal(worldA.Name, worldB.Name);
                if (strCompare != 0)
                {
                    return strCompare;
                }

                return (int)(worldA.WorldId - worldB.WorldId);
            });

            foreach (var data in sortedData)
            {
                var view = MakeViewItem(data);
                Views.Add(view);
                if (data.WorldId == lastSelectedId)
                {
                    Selection = view;
                }
            }
        }

        private ConnectDialogWorldView MakeViewItem(WorldData data, bool isSelected = false)
        {
            ConnectDialogWorldView view = Object.Instantiate(Template, Template.transform.parent);
            view.WorldData = data;
            view.IsSelected = isSelected;
            view.OnClick = () => Selection = view;
            view.gameObject.SetActive(true);
            return view;
        }

        public void Clear()
        {
            Selection = default;
            foreach (var view in Views)
            {
                Object.Destroy(view.gameObject);
            }
            Views.Clear();
        }
    }
}
