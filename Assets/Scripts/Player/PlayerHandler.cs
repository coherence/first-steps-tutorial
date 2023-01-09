using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerHandler : MonoBehaviour
{
    public GameObject prefabToSpawn;
    
    [FormerlySerializedAs("cmVcam")] [Header("Camera")]
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
        Vector3 initialPosition = transform.position + Random.insideUnitSphere * 3f;
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
}
