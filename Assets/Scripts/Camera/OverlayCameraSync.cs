using Cinemachine;
using UnityEngine;

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