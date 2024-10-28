using System.Collections.Generic;
using Coherence.Connection;
using Coherence.Toolkit;
using UnityEngine;
    
namespace Coherence.Samples.PlayerSpawner
{
    /// <summary>
    /// A player spawner script. This is created by the GameManager, and is a unique entity.
    /// It oversees players joining and leaving the game by keeping an eye on ClientConnections.
    /// Both the already existing ones, in <see cref="AssignSpawnPointToExistingClients"/>;
    /// and new ones (aka players who just joined) in <see cref="AssignSpawnPointToClient"/>.
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private CoherenceSync prefabToSpawn;

        private CoherenceBridge coherenceBridge;
        private CoherenceSync coherenceSync;
        private CoherenceSync localPlayerInstance;
        private Queue<Transform> unusedSpawnPoints;
        private HashSet<ClientID> assignedClients;

        private void Awake()
        {
            CoherenceBridgeStore.TryGetBridge(gameObject.scene, out coherenceBridge);
            coherenceBridge.onDisconnected.AddListener(OnDisconnected);
            coherenceSync = GetComponent<CoherenceSync>();
        }

        public void Init(List<Transform> spawnPoints)
        {
            unusedSpawnPoints = new Queue<Transform>(spawnPoints);
            assignedClients = new HashSet<ClientID>();

            coherenceBridge.ClientConnections.OnCreated += AssignSpawnPointToClient;
            Vector3 spawnPoint = GetSpawnPointPosition();
            SpawnPrefab((uint)coherenceBridge.Client.ClientID, spawnPoint);

            AssignSpawnPointToExistingClients();
        }

        private void AssignSpawnPointToExistingClients()
        {
            foreach (CoherenceClientConnection clientConnection in coherenceBridge.ClientConnections.GetAll())
            {
                AssignSpawnPointToClient(clientConnection);
            }
        }

        private void AssignSpawnPointToClient(CoherenceClientConnection clientConnection)
        {
            ClientID clientID = clientConnection.ClientId;

            bool hasIndexAssigned = assignedClients.Contains(clientID);
            if (hasIndexAssigned) return;

            Vector3 spawnPosition = GetSpawnPointPosition();
            coherenceSync.SendCommand(SpawnPrefab, MessageTarget.Other, (uint)clientID, spawnPosition);

            assignedClients.Add(clientID);
        }

        private Vector3 GetSpawnPointPosition()
        {
            if (unusedSpawnPoints.Count == 0)
            {
                Debug.LogWarning($"No more spawn points available! Assigning {Vector3.zero}");
                return Vector3.zero;
            }

            Transform spawnPoint = unusedSpawnPoints.Dequeue();
            return spawnPoint.transform.position;
        }
        
        /// <summary>
        /// A Network Command, invoked by the PlayerSpawner that has authority, and sent directly
        /// to each Client that has just joined that needs an instance of the player Prefab.
        /// </summary>
        [Command]
        public void SpawnPrefab(uint clientId, Vector3 worldPosition)
        {
            if ((uint)coherenceBridge.Client.ClientID != clientId) return;

            localPlayerInstance = Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);
        }

        private void OnDisconnected(CoherenceBridge _, ConnectionCloseReason __)
        {
            if (localPlayerInstance != null)
            {
                Destroy(localPlayerInstance.gameObject);
            }

            assignedClients = null;
            unusedSpawnPoints = null;
            coherenceBridge.ClientConnections.OnCreated -= AssignSpawnPointToClient;
        }
    }
}
