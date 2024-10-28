
using System.Collections.Generic;
using Coherence.Connection;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coherence.Samples.PlayerSpawner
{
    /// <summary>
    /// A simple game manager script, that manages the lifetime of the PlayerSpawner entity and initialises it.
    /// Because this entity instantiates the PlayerSpawner, whoever has authority on it also has it on the spawner. 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> availableSpawnPoints;
        [SerializeField] private CoherenceSync playerSpawnerPrefab;

        private CoherenceBridge coherenceBridge;
        private CoherenceSync coherenceSync;
        private CoherenceSync playerSpawnerInstance;

        private void Awake()
        {
            CoherenceBridgeStore.TryGetBridge(gameObject.scene, out coherenceBridge);
            coherenceBridge.onLiveQuerySynced.AddListener(OnConnected);
            coherenceBridge.onDisconnected.AddListener(OnDisconnected);
            
            coherenceSync = GetComponent<CoherenceSync>();
        }

        private void OnConnected(CoherenceBridge _)
        {
            if (!coherenceSync.HasStateAuthority)
            {
                return;
            }

            playerSpawnerInstance = Instantiate(playerSpawnerPrefab);
            playerSpawnerInstance.GetComponent<PlayerSpawner>().Init(availableSpawnPoints);
        }

        private void OnDisconnected(CoherenceBridge _, ConnectionCloseReason __)
        {
            if (playerSpawnerInstance != null)
            {
                Destroy(playerSpawnerInstance.gameObject);
            }
        }
    }
}
