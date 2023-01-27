using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHandler : MonoBehaviour
{
    public float spawnRadius = 1f;
    public GameObject prefabToSpawn;
    
    [Header("Camera")]
    public CinemachineVirtualCamera gameplayVCam;
    public bool lookAtPlayer;
    public bool followPlayer;

    private GameObject _player;

    private void Awake()
    {
        if (gameplayVCam != null) gameplayVCam.gameObject.SetActive(false);
    }

    public void SpawnPlayer()
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

    public void DespawnPlayer()
    {
        Destroy(_player);
        if (gameplayVCam != null) gameplayVCam.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
