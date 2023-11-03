// Copyright (c) coherence ApS.
// See the license file in the package root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coherence.Cloud;
using Coherence.Connection;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Coherence.Samples.RoomsDialog
{
    public class RoomsDialogUI : MonoBehaviour
    {
#region References
        [Header("References")]
        public GameObject connectDialog;
        public GameObject disconnectDialog;
        public GameObject createRoomPanel;
        public GameObject regionSection;
        public GameObject noRSPlaceholder;
        public GameObject noCloudPlaceholder;
        public GameObject noRoomsAvailable;
        public GameObject loadingSpinner;
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
#endregion

        private IRoomsService selectedRoomService;

        private CloudRooms cloudRooms;
        private ReplicationServerRoomsService replicationServerRoomsService;

        private int UserRoomLimit => int.TryParse(roomLimitInputField.text, out var limit) ? limit : 10;

        private string initialJoinRoomTitle;
        private ListView roomsListView;
        private bool joinNextCreatedRoom = false;
        private ulong lastCreatedRoomUid;
        private Coroutine localToggleRefresher;
        private Coroutine cloudServiceReady;
        private CoherenceBridge bridge;

#region Unity Events
        private void OnEnable()
        {
            if (bridge == null)
            {
                if (!CoherenceBridgeStore.TryGetBridge(gameObject.scene, out bridge))
                {
                    Debug.LogError(
                        $"Couldn't find a {nameof(CoherenceBridge)} in your scene. This dialog will not function properly.");
                    return;
                }
            }

            cloudRooms ??= bridge.CloudService.Rooms;
            replicationServerRoomsService ??= new ReplicationServerRoomsService();

            disconnectDialog.SetActive(false);
            bridge.onConnected.AddListener(_ => UpdateDialogsVisibility());
            bridge.onDisconnected.AddListener((_, __) => UpdateDialogsVisibility());
            bridge.onConnectionError.AddListener(OnConnectionError);

            var uploadedSchemaToCloud = RuntimeSettings.instance.UploadedSchema;

            if (!string.IsNullOrEmpty(RuntimeSettings.instance.ProjectID) && uploadedSchemaToCloud)
            {
                cloudServiceReady = StartCoroutine(WaitForCloudService());
            }
            else if (regionDropdown.gameObject.activeInHierarchy)
            {
                noCloudPlaceholder.SetActive(true);

                if (!uploadedSchemaToCloud)
                {
                    noCloudPlaceholder.GetComponentInChildren<Text>().text =
                        "coherence Cloud Rooms are not available.\n" +
                        "Your latest Schema has not been uploaded to your Project. You can upload your current Schema from the coherence Cloud Tab in the Hub Window.";
                }
            }

            localToggleRefresher = StartCoroutine(LocalToggleRefresher());
        }

        private void OnDisable()
        {
            if(localToggleRefresher != null)
            {
                StopCoroutine(localToggleRefresher);
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
        }

        void Start()
        {
            lanOnlineToggle.onValueChanged.AddListener(OnToggleChanged);
            joinRoomButton.onClick.AddListener(() => JoinRoom(roomsListView.Selection.RoomData));
            showCreateRoomPanelButton.onClick.AddListener(ShowCreateRoomPanel);
            hideCreateRoomPanelButton.onClick.AddListener(HideCreateRoomPanel);
            createAndJoinRoomButton.onClick.AddListener(CreateRoomAndJoin);
            regionDropdown.onValueChanged.AddListener(OnRegionChanged);
            refreshRegionsButton.onClick.AddListener(RefreshRegions);
            refreshRoomsButton.onClick.AddListener(RefreshRooms);
            disconnectButton.onClick.AddListener(bridge.Disconnect);
            popupDismissButton.onClick.AddListener(HideError);

            popupDialog.SetActive(false);
            noRSPlaceholder.SetActive(false);
            noRoomsAvailable.SetActive(false);
            joinRoomButton.interactable = false;
            showCreateRoomPanelButton.interactable = false;
            refreshRegionsButton.interactable = false;
            templateRoomView.gameObject.SetActive(false);
            roomsListView = new ListView
            {
                Template = templateRoomView,
                onSelectionChange = view =>
                {
                    joinRoomButton.interactable = view != default && view.RoomData.UniqueId != default(RoomData).UniqueId;
                }
            };

            initialJoinRoomTitle = joinRoomTitleText.text;
        }
#endregion

#region Cloud & Replication Server Requests
        private IEnumerator WaitForCloudService()
        {
            ShowLoadingState();

            while (!cloudRooms.IsLoggedIn)
            {
                yield return null;
            }

            HideLoadingState();

            RefreshRegions();
            cloudServiceReady = null;
        }

        private void RefreshRooms()
        {
            if (selectedRoomService == null)
            {
                return;
            }

            ShowLoadingState();
            noRoomsAvailable.SetActive(false);
            refreshRoomsButton.interactable = false;
            selectedRoomService.FetchRooms(OnFetchRooms);
        }

        private void CreateRoom()
        {
            var options = RoomCreationOptions.Default;
            options.KeyValues.Add(RoomData.RoomNameKey, roomNameInputField.text);
            options.MaxClients = UserRoomLimit;

            selectedRoomService?.CreateRoom(OnRoomCreated, options);
            HideCreateRoomPanel();
        }

        private void JoinRoom(RoomData roomData)
        {
            ShowLoadingState();
            bridge.JoinRoom(roomData);
        }

        private void CreateRoomAndJoin()
        {
            joinNextCreatedRoom = true;
            CreateRoom();
        }

        private void RefreshRegions()
        {
            ShowLoadingState();
            cloudRooms.RefreshRegions(OnRegionsChanged);
        }
#endregion

#region Local Replication Server
        private IEnumerator LocalToggleRefresher()
        {
            while (true)
            {
                var task = replicationServerRoomsService.IsOnline();
                yield return new WaitUntil(() => task.IsCompleted);

                var result = task.Result;

                HandleLocalServerStatus(result);

                yield return new WaitForSeconds(1f);
            }
        }

        private void HandleLocalServerStatus(bool result)
        {
            if (result && lanOnlineToggle.isOn && !noRSPlaceholder.activeInHierarchy)
            {
                return;
            }

            noRSPlaceholder.SetActive(lanOnlineToggle.isOn && !result);

            if (noRSPlaceholder.activeInHierarchy)
            {
                noRoomsAvailable.SetActive(false);
            }

            if (result && lanOnlineToggle.isOn)
            {
                selectedRoomService = replicationServerRoomsService;
                RefreshRooms();
            }
        }
#endregion

#region Request Callbacks
        private void OnRoomCreated(RequestResponse<RoomData> requestResponse)
        {
            if (!AssertRequestResponse("Error during room creation", requestResponse.Status, requestResponse.Exception))
            {
                joinNextCreatedRoom = false;
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

        private void OnRegionsChanged(RequestResponse<IReadOnlyList<string>> requestResponse)
        {
            HideLoadingState();

            if (!AssertRequestResponse("Error while fetching room regions", requestResponse.Status, requestResponse.Exception))
            {
                return;
            }

            var options = new List<Dropdown.OptionData>();
            var regions = requestResponse.Result;
            foreach (var region in regions)
            {
                options.Add(new Dropdown.OptionData(region));
            }

            regionDropdown.options = options;

            if (regions.Count > 0 && !lanOnlineToggle.isOn)
            {
                regionDropdown.captionText.text = regions[0];
                selectedRoomService = cloudRooms.GetRoomServiceForRegion(regions[0]);
                RefreshRooms();
            }
        }

        private void OnFetchRooms(RequestResponse<IReadOnlyList<RoomData>> requestResponse)
        {
            var rooms = requestResponse.Result;
            refreshRoomsButton.interactable = true;
            loadingSpinner.SetActive(false);
            HideLoadingState();

            joinRoomTitleText.text = initialJoinRoomTitle + " (0)";
            noRoomsAvailable.SetActive(requestResponse.Status != RequestStatus.Success || requestResponse.Result.Count == 0);

            if (!AssertRequestResponse("Error while fetching available rooms", requestResponse.Status, requestResponse.Exception))
            {
                roomsListView.Clear();
                return;
            }

            if (rooms.Count == 0)
            {
                roomsListView.Clear();
                return;
            }

            roomsListView.SetSource(rooms, lastCreatedRoomUid);
            lastCreatedRoomUid = default; // selection was already set.
            joinRoomTitleText.text = $"{initialJoinRoomTitle} ({rooms.Count})";

            joinRoomButton.interactable = roomsListView.Selection != default;
        }
#endregion

#region Error Handling
        private void ShowError(string title, string message = "Unknown Error")
        {
            popupDialog.SetActive(true);
            popupTitleText.text = title;
            popupText.text = message;
            Debug.LogError(message);
        }

        private void HideError()
        {
            popupDialog.SetActive(false);
        }

        private bool AssertRequestResponse(string title, RequestStatus status, Exception exception)
        {
            if (status == RequestStatus.Success)
            {
                return true;
            }

            ShowError(title, exception?.Message);

            return false;
        }

        private void OnConnectionError(CoherenceBridge bridge, ConnectionException exception)
        {
            HideLoadingState();
            RefreshRooms();
            ShowError("Error connecting to Room", exception?.Message);
        }
#endregion

#region Update UI
        private void OnToggleChanged(bool isLanToggled)
        {
            regionDropdown.interactable = !isLanToggled;
            regionSection.SetActive(!isLanToggled);
            noRSPlaceholder.SetActive(isLanToggled);
            noRoomsAvailable.SetActive(false);
            noCloudPlaceholder.SetActive(!isLanToggled && !cloudRooms.IsLoggedIn);
            HideCreateRoomPanel();
            loadingSpinner.SetActive(false);

            cloudText.font = isLanToggled ? normalFont : boldFont;
            lanText.font = isLanToggled ? boldFont : normalFont;

            selectedRoomService = null;

            if (isLanToggled)
            {
                RefreshLocalToggle();
            }
            else if (cloudRooms.IsLoggedIn)
            {
                var currentRegion = regionDropdown.options[regionDropdown.value].text;
                selectedRoomService = cloudRooms.GetRoomServiceForRegion(currentRegion);
                RefreshRooms();
            }
        }
        private void ShowCreateRoomPanel()
        {
            createRoomPanel.SetActive(true);
        }

        private void HideCreateRoomPanel()
        {
            createRoomPanel.SetActive(false);
        }

        private async void RefreshLocalToggle()
        {
            var result = await replicationServerRoomsService.IsOnline();

            HandleLocalServerStatus(result);
        }

        private void UpdateDialogsVisibility()
        {
            connectDialog.SetActive(!bridge.isConnected);
            disconnectDialog.SetActive(bridge.isConnected);

            if (!bridge.isConnected)
            {
                RefreshRooms();
            }
        }

        private void HideLoadingState()
        {
            loadingSpinner.SetActive(false);
            showCreateRoomPanelButton.interactable = true;
            refreshRegionsButton.interactable = true;
            joinRoomButton.interactable = roomsListView != null && roomsListView.Selection != default
                                                                && roomsListView.Selection.RoomData.UniqueId != default(RoomData).UniqueId;
        }

        private void ShowLoadingState()
        {
            loadingSpinner.SetActive(true);
            showCreateRoomPanelButton.interactable = false;
            refreshRegionsButton.interactable = false;
            joinRoomButton.interactable = false;
        }

        private void OnRegionChanged(int region)
        {
            if (!cloudRooms.IsLoggedIn)
            {
                return;
            }

            var regionText = regionDropdown.options[region].text;

            selectedRoomService = cloudRooms.GetRoomServiceForRegion(regionText);
            RefreshRooms();
        }
#endregion
    }

    internal class ListView
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
        private HashSet<ulong> displayedIds = new HashSet<ulong>();
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
            ConnectDialogRoomView view = Object.Instantiate(Template, Template.transform.parent);
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
                Object.Destroy(view.gameObject);
            }
            Views.Clear();
        }
    }
}
