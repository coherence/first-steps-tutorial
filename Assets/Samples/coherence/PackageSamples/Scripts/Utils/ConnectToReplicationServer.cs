using Coherence.Cloud;
using Coherence.Connection;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace Coherence.Samples
{
    /// <summary>
    /// Utility class to perform a simple connection to the Replication Server. For more functionality,
    /// you can explore the UIs included in the package samples (coherence > Explore Samples...).
    /// </summary>
    public class ConnectToReplicationServer : MonoBehaviour
    {
        [SerializeField] private Text connectButtonText;

        private CoherenceBridge coherenceBridge;

        private void Awake()
        {
            // Fetch the CoherenceBridge component that we will use to connect to the Server.
            CoherenceBridgeStore.TryGetBridge(gameObject.scene, out coherenceBridge);
            coherenceBridge.onConnected.AddListener(OnConnectedToReplicationServer);
            coherenceBridge.onDisconnected.AddListener(OnDisconnectedFromReplicationServer);
            coherenceBridge.onConnectionError.AddListener(OnConnectionError);
        }

        private void OnConnectionError(CoherenceBridge _, ConnectionException exception)
        {
            Debug.LogError(
                "Error while connecting to the Replication Server. Make sure it is launched from coherence/Local Replication Server/Run For Worlds and you have latest baked code from coherence/bake.");
        }

        private void OnConnectedToReplicationServer(CoherenceBridge _)
        {
            Debug.Log("Connected successfully to the Replication Server.");
            connectButtonText.text = "Disconnect";
        }

        private void OnDisconnectedFromReplicationServer(CoherenceBridge _, ConnectionCloseReason reason)
        {
            Debug.Log("Disconnected from to the Replication Server.");
            connectButtonText.text = "Connect";
        }

        public void ConnectOrDisconnectReplicationServer()
        {
            if (!coherenceBridge.IsConnected)
            {
                Debug.Log(
                    "Connecting to the Local Worlds Replication Server. Make sure you have launched it from coherence/Local Replication Server/Run For Worlds and you have latest baked code from coherence/bake.");
                coherenceBridge.JoinWorld(WorldData.GetLocalWorld(RuntimeSettings.instance.LocalHost));
            }
            else
            {
                coherenceBridge.Disconnect();
            }
        }
    }
}
