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
    public class WorldDialogUI : MonoBehaviour
    {
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
        private IReadOnlyList<WorldData> availableCloudWorlds = new List<WorldData>();
        private Coroutine cloudServiceReady;
        private WorldsService cloudWorlds;
        private string initialWorldTitle;
        private WorldData localWorld;
        private Coroutine localWorldRefresher;
        private ListView worldsListView;

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

            cloudWorlds = bridge.CloudService.Worlds;

            bridge.onConnected.AddListener(_ => UpdateDialogsVisibility());
            bridge.onDisconnected.AddListener((_, __) => UpdateDialogsVisibility());
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
        }

        void Start()
        {
            initialWorldTitle = worldTitleText.text;
            refreshWorldsButton.onClick.AddListener(RefreshWorlds);
            joinButton.onClick.AddListener(OnClickJoin);
            popupDismissButton.onClick.AddListener(HideError);

            popupDialog.SetActive(false);
            templateWorldView.gameObject.SetActive(false);
            worldsListView = new ListView
            {
                Template = templateWorldView,
                onSelectionChange = view =>
                {
                    joinButton.interactable = view != default && view.WorldData.WorldId != default(WorldData).WorldId;
                }
            };
        }
        #endregion

        #region Cloud Requests
        private IEnumerator WaitForCloudService()
        {
            ShowLoadingState();

            while (!cloudWorlds.IsLoggedIn)
            {
                yield return null;
            }

            HideLoadingState();

            RefreshWorlds();
            cloudServiceReady = null;
        }

        private void RefreshWorlds()
        {
            if (cloudWorlds.IsLoggedIn)
            {
                ShowLoadingState();
                cloudWorlds.FetchWorlds(OnWorldsFetched);
            }
        }
        #endregion

        #region Request Callbacks
        private void OnClickJoin()
        {
            ShowLoadingState();
            bridge.JoinWorld(worldsListView.Selection.WorldData);
        }

        private void OnWorldsFetched(RequestResponse<IReadOnlyList<WorldData>> requestResponse)
        {
            HideLoadingState();

            if (requestResponse.Status != RequestStatus.Success)
            {
                ShowError("Error fetching Worlds", requestResponse.Exception?.Message);
                return;
            }

            availableCloudWorlds = requestResponse.Result;
            RefreshWorldsListView();
        }

        public void Disconnect()
        {
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
            Debug.LogError(message);
        }

        private void HideError()
        {
            popupDialog.SetActive(false);
        }

        private void OnConnectionError(CoherenceBridge bridge, ConnectionException exception)
        {
            HideLoadingState();
            ShowError("Error connecting to World", exception?.Message);
        }
        #endregion

        #region Update UI
        private void RefreshWorldsListView()
        {
            var allWorlds = availableCloudWorlds.ToList();
            if (localWorld.WorldId != default(WorldData).WorldId)
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
