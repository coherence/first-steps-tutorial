// Copyright (c) coherence ApS.
// See the license file in the package root for more information.

namespace Coherence.Samples.RoomsDialog
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Cloud;
    using Connection;
    using Runtime;
    using Toolkit;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class RoomsDialogUI : MonoBehaviour
    {
        [Header("References")]
        public GameObject connectDialog;
        public GameObject disconnectDialog;
        public GameObject createRoomPanel;
        public GameObject regionSection;
        public GameObject noRSPlaceholder;
        public GameObject noCloudPlaceholder;
        public GameObject noRoomsAvailable;
        public GameObject loadingSpinner;
        public GameObject noBridgeFound;
        public GameObject noProjectSelected;
        public GameObject noCloudLoginFound;
        public GameObject noCloudRegionsAvailable;
        private CloudState cloudState = CloudState.Default;
        private LocalState localState = LocalState.Default;
        private UIState onlineModeUIState = UIState.LoadingSpinner;
        private UIState localModeUIState = UIState.LoadingSpinner;
        private bool wasCloudModeEnabled = true;
        public Font boldFont;
        public Font normalFont;
        public Text cloudText;
        public Text lanText;
        public Text joinRoomTitleText;
        public ConnectDialogRoomView templateRoomView;
        public InputField roomNameInputField;
        public Toggle lanOnlineToggle;
        public InputField roomLimitInputField;
        public Dropdown regionDropdown;
        public Button refreshRegionsButton;
        public Button refreshRoomsButton;
        public Button joinRoomButton;
        public Button showCreateRoomPanelButton;
        public Button hideCreateRoomPanelButton;
        public Button createAndJoinRoomButton;
        public Button disconnectButton;
        public GameObject popupDialog;
        public Text popupText;
        public Text popupTitleText;
        public Button popupDismissButton;

        private IRoomsService activeRoomsService;
        private bool isLocalRoomsServiceOnline;
        private ReplicationServerRoomsService replicationServerRoomsService;
        private string initialJoinRoomTitle;
        private ListView roomsListView;
        private bool joinNextCreatedRoom;
        private ulong lastCreatedRoomUid;
        private Coroutine localToggleRefresher;
        private CoherenceBridge bridge;
        [MaybeNull] private CloudRooms cloudRooms;
        [MaybeNull] private RegionsService regionsService;
        [MaybeNull] private CloudRoomsService cloudRoomsService;
        private CancellationTokenSource cloudModeCancellationTokenSource;
        private CancellationTokenSource localModeCancellationTokenSource;
        private IReadOnlyList<string> cloudRegionOptions = Array.Empty<string>();
        private IReadOnlyList<RoomData> rooms = Array.Empty<RoomData>();

        private bool IsLoggedIn => cloudRooms is { IsLoggedIn: true };
        private bool IsSelectedRoomServiceReady => cloudRoomsService is not null && IsCloudModeEnabled ? IsLoggedIn : isLocalRoomsServiceOnline;

        private int RoomMaxPlayers => int.TryParse(roomLimitInputField.text, out var limit) ? limit : 10;
        private bool IsCloudModeEnabled
        {
            // Default to Cloud mode when the LAN/Online toggle hasn't been wired up (e.g. it was removed from a customized copy of this sample).
            get => !lanOnlineToggle || !lanOnlineToggle.isOn;
            set
            {
                if (lanOnlineToggle)
                {
                    lanOnlineToggle.isOn = !value;
                }
            }
        }

        private void SetUIState(UIState state)
        {
            if (IsCloudModeEnabled)
            {
                onlineModeUIState = state;
            }
            else
            {
                localModeUIState = state;
            }

            if (state is UIState.Ready or UIState.CreatingRoom or UIState.NoRoomsExist)
            {
                ExitLoadingState();
            }
            else
            {
                SetRoomSectionButtonInteractability(false);
            }

            loadingSpinner.SetActive(state is UIState.LoadingSpinner);
            noRSPlaceholder.SetActive(state is UIState.NoReplicationServerFound);
            noRoomsAvailable.SetActive(state is UIState.NoRoomsExist);
            createRoomPanel.SetActive(state is UIState.CreatingRoom);
            noBridgeFound.SetActive(state is UIState.NoBridgeFound);
            noCloudPlaceholder.SetActive(state is UIState.CloudRoomsNotAvailable);
            noProjectSelected.SetActive(state is UIState.NoProjectSelected);
            noCloudLoginFound.SetActive(state is UIState.NoCloudLoginFound);
            noCloudRegionsAvailable.SetActive(state is UIState.NoCloudRegionsAvailable);
        }

        private void Awake()
        {
            if (SimulatorUtility.IsSimulator)
            {
                gameObject.SetActive(false);
            }
        }

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
                               "Add one via 'GameObject > coherence > Bridge'.", this);
                SetUIState(UIState.NoBridgeFound);
                return;
            }

            replicationServerRoomsService ??= new();

            disconnectDialog.SetActive(false);
            SetUIState(UIState.LoadingSpinner);

            bridge.onConnected.AddListener(OnBridgeConnected);
            bridge.onDisconnected.AddListener(OnBridgeDisconnected);
            bridge.onConnectionError.AddListener(OnConnectionError);
            localToggleRefresher = StartCoroutine(LocalToggleRefresher());
        }

        private async void Start()
        {
            if (lanOnlineToggle)
            {
                lanOnlineToggle.onValueChanged.AddListener(OnModeChanged);
            }
            joinRoomButton.onClick.AddListener(() => JoinRoom(roomsListView.Selection.RoomData));
            showCreateRoomPanelButton.onClick.AddListener(ShowCreateRoomPanel);
            hideCreateRoomPanelButton.onClick.AddListener(HideCreateRoomPanel);
            createAndJoinRoomButton.onClick.AddListener(CreateRoomAndJoin);
            regionDropdown.onValueChanged.AddListener(OnCloudRoomsRegionSelectionChanged);
            refreshRegionsButton.onClick.AddListener(RefreshCloudRoomsRegions);
            refreshRoomsButton.onClick.AddListener(OnRefreshRoomsButtonClicked);
            disconnectButton.onClick.AddListener(bridge.Disconnect);
            popupDismissButton.onClick.AddListener(HideError);

            popupDialog.SetActive(false);
            templateRoomView.gameObject.SetActive(false);
            EnterLoadingState();

            roomsListView = new ListView
            {
                Template = templateRoomView,
                onSelectionChange = view =>
                {
                    joinRoomButton.interactable = view != default && view.RoomData.UniqueId != default(RoomData).UniqueId;
                }
            };

            initialJoinRoomTitle = joinRoomTitleText.text;
            isLocalRoomsServiceOnline = await replicationServerRoomsService.IsOnline();
            if (!this)
            {
                return;
            }

            // Set the Local tab active by default if a local replication server is online.
            if (isLocalRoomsServiceOnline)
            {
                IsCloudModeEnabled = false;
                return;
            }

            // Leave the Online tab active and log in to coherence Cloud if no local replication server is online.
            SetCloudState(CloudState.LoggingIn);
        }

        private void OnDisable()
        {
            if (bridge)
            {
                bridge.onConnected.RemoveListener(OnBridgeConnected);
                bridge.onDisconnected.RemoveListener(OnBridgeDisconnected);
                bridge.onConnectionError.RemoveListener(OnConnectionError);
            }

            if (localToggleRefresher != null)
            {
                StopCoroutine(localToggleRefresher);
            }

            cloudModeCancellationTokenSource?.Dispose();
            cloudModeCancellationTokenSource = null;
            localModeCancellationTokenSource?.Dispose();
            localModeCancellationTokenSource = null;
        }

        private void OnDestroy() => replicationServerRoomsService?.Dispose();

        private bool TryFindCoherenceCloudLogin([MaybeNullWhen(false), NotNullWhen(true)] out CoherenceCloudLogin cloudLogin)
            => cloudLogin = FindAnyObjectByType<CoherenceCloudLogin>(FindObjectsInactive.Exclude);

        private async Task LogInToCoherenceCloud()
        {
            if (!TryFindCoherenceCloudLogin(out var cloudLogin))
            {
                SetUIState(UIState.NoCloudLoginFound);
                return;
            }

            if (cloudLogin.IsLoggedIn)
            {
                var services = cloudLogin.Services;
                cloudRooms = services.Rooms;
                regionsService = services.Regions;
                SetCloudState(CloudState.FetchingRegions);
                return;
            }

            if (string.IsNullOrEmpty(RuntimeSettings.Instance.ProjectID))
            {
                SetUIState(UIState.NoProjectSelected);
                return;
            }

            EnterLoadingState();

            var loginOperation = await cloudLogin.LogInAsync(GetOrCreateCloudModeCancellationToken());
            if (loginOperation.IsCompletedSuccessfully)
            {
                var services = loginOperation.Result.Services;
                cloudRooms = services.Rooms;
                regionsService = services.Regions;
                SetCloudState(CloudState.FetchingRegions);
                return;
            }

            if (loginOperation.HasFailed)
            {
                SetUIState(UIState.CloudRoomsNotAvailable);

                var error = loginOperation.Error;
                var errorMessage = error.Type switch
                {
                    LoginErrorType.SchemaNotFound => "Logging in failed because local schema has not been uploaded to the Cloud.\n\nYou can upload local schema via <b>coherence > Upload Schema</b>.",
                    LoginErrorType.NoProjectSelected => "Logging in failed because no project was selected.\n\nYou can select a project via <b>coherence > Hub > Cloud</b>.",
                    LoginErrorType.ServerError => "Logging in failed because of a server error.",
                    LoginErrorType.InvalidCredentials => "Logging in failed because invalid credentials were provided.",
                    LoginErrorType.InvalidResponse => "Logging in failed because was unable to deserialize the response from the server.",
                    LoginErrorType.TooManyRequests => "Logging in failed because too many requests have been sent within a short amount of time.\n\nPlease slow down the rate of sending requests, and try again later.",
                    LoginErrorType.ConnectionError => "Logging in failed because of connection failure.",
                    LoginErrorType.AlreadyLoggedIn => $"The cloud services are already connected to a player account. You have to call {nameof(PlayerAccount)}.{nameof(PlayerAccount.Logout)}. before attempting to log in again.",
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
            }
        }

        private void OnRefreshRoomsButtonClicked()
        {
            if (IsCloudModeEnabled)
            {
                SetCloudState(CloudState.FetchingRooms);
            }
            else
            {
                SetLocalState(LocalState.FetchingRooms);
            }
        }

        private void RefreshRooms()
        {
            if (!IsSelectedRoomServiceReady)
            {
                return;
            }

            refreshRoomsButton.interactable = false;
            EnterLoadingState();
            activeRoomsService.FetchRooms(OnRoomsFetched, null, GetOrCreateActiveCancellationToken());
        }

        private void CreateRoom()
        {
            var options = new RoomCreationOptions
            {
                Name = roomNameInputField.text,
                MaxClients = RoomMaxPlayers,
            };
            activeRoomsService?.CreateRoom(OnRoomCreated, options, GetOrCreateActiveCancellationToken());
            HideCreateRoomPanel();
        }

        private void JoinRoom(RoomData roomData)
        {
            EnterLoadingState();
            bridge.JoinRoom(roomData);
        }

        private void CreateRoomAndJoin()
        {
            joinNextCreatedRoom = true;
            CreateRoom();
        }

        private void RefreshCloudRoomsRegions()
        {
            if (!IsLoggedIn)
            {
                return;
            }

            EnterLoadingState();
            regionsService.FetchRegions(OnCloudRoomsRegionsChanged, GetOrCreateCloudModeCancellationToken());
        }

        private CancellationToken GetOrCreateActiveCancellationToken() => GetOrCreateActiveCancellationTokenSource().Token;
        private CancellationToken GetOrCreateCloudModeCancellationToken() => GetOrCreateCloudModeCancellationTokenSource().Token;
        private CancellationTokenSource GetOrCreateActiveCancellationTokenSource() => IsCloudModeEnabled ? GetOrCreateCloudModeCancellationTokenSource() : GetOrCreateLocalModeCancellationTokenSource();
        private CancellationTokenSource GetOrCreateCloudModeCancellationTokenSource() => cloudModeCancellationTokenSource ??= new();
        private CancellationTokenSource GetOrCreateLocalModeCancellationTokenSource() => localModeCancellationTokenSource ??= new();

        private IEnumerator LocalToggleRefresher()
        {
            while (true)
            {
                var task = replicationServerRoomsService.IsOnline();
                yield return new WaitUntil(() => task.IsCompleted);

                isLocalRoomsServiceOnline = task.Result;

                HandleLocalServerStatus(isLocalRoomsServiceOnline);

                yield return new WaitForSeconds(1f);
            }
        }

        private void SetLocalState(LocalState state)
        {
            if (localState == state && !wasCloudModeEnabled)
            {
                return;
            }

            localState = state;
            if (IsCloudModeEnabled)
            {
                return;
            }

            wasCloudModeEnabled = false;

            switch (state)
            {
                case LocalState.Default:
                    EnterLoadingState();
                    return;
                case LocalState.Offline:
                    SetUIState(UIState.NoReplicationServerFound);
                    return;
                case LocalState.Ready:
                    SetUIState(UIState.Ready);
                    return;
                case LocalState.FetchingRooms:
                    RefreshRooms();
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private async void SetCloudState(CloudState state)
        {
            if (cloudState == state && wasCloudModeEnabled)
            {
                 return;
            }

            cloudState = state;
            if (!IsCloudModeEnabled)
            {
                return;
            }

            wasCloudModeEnabled = true;

            switch (state)
            {
                case CloudState.Default:
                    SetCloudState(CloudState.LoggingIn);
                    return;
                case CloudState.LoggingIn:
                    await LogInToCoherenceCloud();
                    return;
                case CloudState.FetchingRegions:
                    RefreshCloudRoomsRegions();
                    return;
                case CloudState.FetchingRooms:
                    RefreshRooms();
                    return;
                case CloudState.Ready:
                    ExitLoadingState();
                    SetUIState(UIState.Ready);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void HandleLocalServerStatus(bool isLocalRoomsServiceOnline)
        {
            if (IsCloudModeEnabled)
            {
                return;
            }

            if (localState is LocalState.Default or LocalState.Ready or LocalState.Offline)
            {
                var newStateState = isLocalRoomsServiceOnline ? LocalState.Ready : LocalState.Offline;
                if (localState != newStateState)
                {
                    SetLocalState(newStateState);
                }
            }

            if (isLocalRoomsServiceOnline)
            {
                SetActiveRoomService(replicationServerRoomsService, isLocal: true);
            }
        }

        private void SetActiveRoomService([MaybeNull] IRoomsService service, bool isLocal)
        {
            if (activeRoomsService == service)
            {
                return;
            }

            activeRoomsService = service;
            if (isLocal)
            {
                if (isLocalRoomsServiceOnline)
                {
                    SetLocalState(LocalState.FetchingRooms);
                }
            }
            else if (!IsLoggedIn)
            {
                SetCloudState(CloudState.LoggingIn);
            }
            else if (cloudRegionOptions.Count is 0)
            {
                SetCloudState(CloudState.FetchingRegions);
            }
            else
            {
                SetCloudState(CloudState.FetchingRooms);
            }
        }

        private void OnRoomCreated(RequestResponse<RoomData> requestResponse)
        {
            if (requestResponse.Status != RequestStatus.Success)
            {
                joinNextCreatedRoom = false;

                var errorMessage = GetErrorFromResponse(requestResponse);
                ShowError("Error creating room", errorMessage);
                Debug.LogException(requestResponse.Exception);
                return;
            }

            var createdRoom = requestResponse.Result;
            if (joinNextCreatedRoom)
            {
                joinNextCreatedRoom = false;
                JoinRoom(createdRoom);
            }
            else
            {
                lastCreatedRoomUid = createdRoom.UniqueId;
                RefreshRooms();
            }
        }

        private void OnCloudRoomsRegionsChanged(RequestResponse<Region[]> requestResponse)
        {
            if (requestResponse.Status != RequestStatus.Success)
            {
                if (cloudRegionOptions.Count > 0)
                {
                    SetCloudState(CloudState.Ready);
                    SetRoomSectionButtonInteractability(true);
                }
                else
                {
                    SetUIState(UIState.NoCloudRegionsAvailable);
                }

                var errorMessage = GetErrorFromResponse(requestResponse);
                ShowError("Error refreshing regions", errorMessage);
                Debug.LogException(requestResponse.Exception);
                return;
            }

            var dropdownOptions = new List<Dropdown.OptionData>();

            cloudRegionOptions = requestResponse.Result.Select(x => x.Name).ToArray();
            foreach (var region in cloudRegionOptions)
            {
                dropdownOptions.Add(new(region));
            }

            regionDropdown.options = dropdownOptions;
            if (cloudRegionOptions.Count > 0 && IsCloudModeEnabled && IsLoggedIn)
            {
                regionDropdown.captionText.text = cloudRegionOptions[0];
                cloudRoomsService = cloudRooms.GetRoomServiceForRegion(cloudRegionOptions[0]);
                SetActiveRoomService(cloudRoomsService, isLocal: false);
                SetCloudState(CloudState.FetchingRooms);
            }
        }

        private void OnRoomsFetched(RequestResponse<IReadOnlyList<RoomData>> requestResponse)
        {
            refreshRoomsButton.interactable = true;
            loadingSpinner.SetActive(false);
            ExitLoadingState();

            joinRoomTitleText.text = initialJoinRoomTitle + " (0)";

            if (IsCloudModeEnabled)
            {
                SetCloudState(CloudState.Ready);
            }
            else
            {
                SetLocalState(LocalState.Ready);
            }

            var roomsExist = requestResponse is { Status: RequestStatus.Success, Result: { Count : > 0 } };
            SetUIState(roomsExist ? UIState.Ready : UIState.NoRoomsExist);

            if (requestResponse.Status is not RequestStatus.Success)
            {
                roomsListView.Clear();
                var errorMessage = GetErrorFromResponse(requestResponse);
                ShowError("Error fetching rooms", errorMessage);
                Debug.LogException(requestResponse.Exception);
                return;
            }

            rooms = requestResponse.Result;

            if (rooms.Count is 0)
            {
                roomsListView.Clear();
                return;
            }

            if (IsCloudModeEnabled)
            {
                SetCloudState(CloudState.Ready);
            }
            else
            {
                SetLocalState(LocalState.Ready);
            }

            roomsListView.SetSource(rooms, lastCreatedRoomUid);
            lastCreatedRoomUid = 0;
            joinRoomTitleText.text = $"{initialJoinRoomTitle} ({rooms.Count})";

            joinRoomButton.interactable = roomsListView.Selection != default;
        }

        private void ShowError(string title, string message = "Unknown Error")
        {
            popupDialog.SetActive(true);
            popupTitleText.text = title;
            popupText.text = message;
        }

        private void HideError() => popupDialog.SetActive(false);

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
                ErrorCode.ClientPermission => "The client has been restricted from accessing this feature. Please check the Project Settings on the Online Dashboard.",
                ErrorCode.CreditLimit => "Monthly credit limit exceeded. Please check your organization credit usage in the Online Dashboard.",
                ErrorCode.InDeployment => "One or more online resources are currently being provisioned. Please retry the request.",
                ErrorCode.FeatureDisabled => "Requested feature is disabled, make sure you enable it in the Game Services section of the Online Dashboard.",
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
                ErrorCode.RestrictedModeCapReached => "Total user capacity for restricted mode server reached.",
                ErrorCode.LoginDisabled => "This authentication method is disabled.",
                ErrorCode.LoginInvalidApp => "The provided App ID is invalid.",
                ErrorCode.LoginNotFound => "No player account has been linked to the authentication method that was used.",
                ErrorCode.OneTimeCodeExpired => "The one-time code has already expired.",
                ErrorCode.OneTimeCodeNotFound => "The one-time code was not found.",
                ErrorCode.IdentityLimit => "Unique identity limit reached.",
                ErrorCode.IdentityNotFound => "Identity not found.",
                ErrorCode.IdentityRemoval => "Tried to unlink last authentication method from player account.",
                ErrorCode.IdentityTaken => "Identity already linked to another player account.",
                ErrorCode.IdentityTotalLimit => "Maximum allowed identity limit reached.",
                ErrorCode.InvalidConfig => "Invalid configuration. Please make sure that all the necessary information has been provided in Online Dashboard.",
                ErrorCode.InvalidInput => "Invalid input. Please make sure to provide all required arguments.",
                ErrorCode.PasswordNotSet => "Password has not been set for the player account.",
                ErrorCode.UsernameNotAvailable => "The username is already taken by another player account.",
                _ => requestException.Message,
            };
        }

        private void OnConnectionError(CoherenceBridge _, ConnectionException exception)
        {
            ExitLoadingState();
            RefreshRooms();

            var (title, message) = exception.GetPrettyMessage();

            Debug.LogError(message, this);
            ShowError(title, message);
        }

        private void OnBridgeDisconnected(CoherenceBridge _, ConnectionCloseReason reason) => UpdateDialogsVisibility();
        private void OnBridgeConnected(CoherenceBridge _) => UpdateDialogsVisibility();

        private void OnModeChanged(bool localMode)
        {
            cloudModeCancellationTokenSource?.Dispose();
            cloudModeCancellationTokenSource = null;
            localModeCancellationTokenSource?.Dispose();
            localModeCancellationTokenSource = null;
            GetOrCreateActiveCancellationTokenSource();

            regionDropdown.interactable = !localMode;
            regionSection.SetActive(!localMode);
            noRSPlaceholder.SetActive(localMode);
            noRoomsAvailable.SetActive(false);
            noCloudPlaceholder.SetActive(!localMode && !IsLoggedIn);
            HideCreateRoomPanel();

            cloudText.font = localMode ? normalFont : boldFont;
            lanText.font = localMode ? boldFont : normalFont;

            if (localMode)
            {
                SetUIState(localModeUIState);
                SetActiveRoomService(replicationServerRoomsService, isLocal: true);
                SetLocalState(localState);
                return;
            }

            SetUIState(onlineModeUIState);
            SetActiveRoomService(cloudRoomsService, isLocal: false);
            SetCloudState(cloudState);
        }

        [return: MaybeNull]
        private string GetCloudRoomsRegion(int index) => index < cloudRegionOptions.Count ? cloudRegionOptions[index] : null;

        private void ShowCreateRoomPanel() => createRoomPanel.SetActive(true);
        private void HideCreateRoomPanel() => createRoomPanel.SetActive(false);

        private void UpdateDialogsVisibility()
        {
            connectDialog.SetActive(!bridge.IsConnected);
            disconnectDialog.SetActive(bridge.IsConnected);

            if (!bridge.IsConnected)
            {
                RefreshRooms();
            }
        }

        private void ExitLoadingState()
        {
            loadingSpinner.SetActive(false);
            SetRoomSectionButtonInteractability(true);
        }

        private void EnterLoadingState()
        {
            SetUIState(UIState.LoadingSpinner);
            SetRoomSectionButtonInteractability(false);
        }

        private void SetRoomSectionButtonInteractability(bool interactable)
        {
            showCreateRoomPanelButton.interactable = interactable;
            refreshRegionsButton.interactable = interactable;

            refreshRoomsButton.interactable = interactable;
            joinRoomButton.interactable = interactable && roomsListView is not null && roomsListView.Selection && roomsListView.Selection.RoomData.UniqueId != default(RoomData).UniqueId;;
        }

        private void OnCloudRoomsRegionSelectionChanged(int selectedIndex)
        {
            if (!IsLoggedIn || GetCloudRoomsRegion(selectedIndex) is not { } region)
            {
                return;
            }

            SetActiveRoomService(cloudRooms.GetRoomServiceForRegion(region), isLocal: false);
            RefreshRooms();
        }

        private enum CloudState
        {
            Default,
            LoggingIn,
            FetchingRegions,
            FetchingRooms,
            Ready
        }

        private enum LocalState
        {
            Default,
            Offline,
            FetchingRooms,
            Ready
        }

        private enum UIState
        {
            LoadingSpinner,
            Ready,
            CreatingRoom,

            NoRoomsExist,
            NoReplicationServerFound,
            NoBridgeFound,
            NoCloudLoginFound,
            NoProjectSelected,
            NoCloudRegionsAvailable,
            CloudRoomsNotAvailable,
        }

        private class ListView
        {
            public ConnectDialogRoomView Template;
            public Action<ConnectDialogRoomView> onSelectionChange;

            public ConnectDialogRoomView Selection
            {
                get => selection;
                set
                {
                    if (selection != value)
                    {
                        selection = value;
                        lastSelectedId = selection == default ? default : selection.RoomData.UniqueId;
                        onSelectionChange?.Invoke(Selection);
                        foreach (var viewRow in Views)
                        {
                            viewRow.IsSelected = selection == viewRow;
                        }
                    }
                }
            }

            public List<ConnectDialogRoomView> Views { get; }
            private ConnectDialogRoomView selection;
            private HashSet<ulong> displayedIds = new();
            private ulong lastSelectedId;

            public ListView(int capacity = 50)
            {
                Views = new List<ConnectDialogRoomView>(capacity);
            }

            public void SetSource(IReadOnlyList<RoomData> dataSource, ulong idToSelect = default)
            {
                if (dataSource.Count == Views.Count && dataSource.All(s => displayedIds.Contains(s.UniqueId)))
                {
                    return;
                }

                displayedIds = new HashSet<ulong>(dataSource.Select(d => d.UniqueId));

                Clear();

                if (dataSource.Count <= 0)
                {
                    return;
                }

                var sortedData = dataSource.ToList();
                sortedData.Sort((roomA, roomB) =>
                {
                    var strCompare = String.CompareOrdinal(roomA.RoomName, roomB.RoomName);
                    if (strCompare != 0)
                    {
                        return strCompare;
                    }

                    return (int)(roomA.UniqueId - roomB.UniqueId);
                });

                if (idToSelect == default && lastSelectedId != default)
                {
                    idToSelect = lastSelectedId;
                }

                foreach (var data in sortedData)
                {
                    var view = MakeViewItem(data);
                    Views.Add(view);
                    if (data.UniqueId == idToSelect)
                    {
                        Selection = view;
                    }
                }
            }

            private ConnectDialogRoomView MakeViewItem(RoomData data, bool isSelected = false)
            {
                var view = Instantiate(Template, Template.transform.parent);
                view.RoomData = data;
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
                    Destroy(view.gameObject);
                }

                Views.Clear();
            }
        }
    }
}
