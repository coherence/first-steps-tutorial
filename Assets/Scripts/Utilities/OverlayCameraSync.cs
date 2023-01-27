using Cinemachine;
using UnityEngine;

/// <summary>
/// Utility class used to synchronise Camera properties of the overlay Camera, with the main Camera.
/// The overlay Camera is in charge of rendering 3D world-space UI.
/// </summary>
public class OverlayCameraSync : MonoBehaviour
{
    public Camera mainCamera;
    
    private CinemachineBrain _cmBrain;
    private Camera _thisCamera;

    private void Awake()
    {
        _thisCamera = GetComponent<Camera>();
        _cmBrain = mainCamera.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        SyncValues();
    }

    private void LateUpdate()
    {
        SyncValues();
    }

    private void SyncValues()
    {
        if (_cmBrain.IsBlending)
        {
            _thisCamera.nearClipPlane = mainCamera.nearClipPlane;
            _thisCamera.farClipPlane = mainCamera.farClipPlane;
            _thisCamera.fieldOfView = mainCamera.fieldOfView;
        }
    }
}