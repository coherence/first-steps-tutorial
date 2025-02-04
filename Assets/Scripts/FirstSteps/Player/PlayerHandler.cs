using Cinemachine;
using Coherence.Connection;
using Coherence.Toolkit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Takes care of spawning and despawning the local player when connecting or disconnecting to the network.
    /// Also focuses the camera (a Cinemachine VCam) on the player.
    /// </summary>
    public class PlayerHandler : MonoBehaviour
    {
        public float spawnRadius = 1f;
        public GameObject prefabToSpawn;
    
        [Header("Camera")]
        public CinemachineVirtualCamera gameplayVCam;
        public bool lookAtPlayer;
        public bool followPlayer;

        private GameObject _player;
        private CoherenceBridge _bridge;

        private void Awake()
        {
            if (gameplayVCam != null) gameplayVCam.gameObject.SetActive(false);
        
            _bridge = FindObjectOfType<CoherenceBridge>();
            _bridge.onConnected.AddListener(OnConnection);
            _bridge.onDisconnected.AddListener(OnDisconnection);
        }

        private void OnConnection(CoherenceBridge bridge) => SpawnPlayer();
        private void OnDisconnection(CoherenceBridge bridge, ConnectionCloseReason reason) => DespawnPlayer();

        private void SpawnPlayer()
        {
            Vector3 initialPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            initialPosition.y = transform.position.y;

            _player = Instantiate(prefabToSpawn, initialPosition, Quaternion.identity);
            _player.name = "[local] Player";

            if (gameplayVCam != null)
            {
                if (followPlayer) gameplayVCam.Follow = _player.transform;
                if (lookAtPlayer) gameplayVCam.LookAt = _player.transform;
                gameplayVCam.gameObject.SetActive(true);
            }
        }

        private void DespawnPlayer()
        {
            Destroy(_player);
            if (gameplayVCam != null) gameplayVCam.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _bridge.onConnected.RemoveListener(OnConnection);
            _bridge.onDisconnected.RemoveListener(OnDisconnection);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
