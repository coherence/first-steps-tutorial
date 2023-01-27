using UnityEngine;

/// <summary>
/// Rotates the GameObject so that it's facing the Camera at all times. Used for world-space UI.
/// </summary>
public class CameraFacing : MonoBehaviour
{
    private bool _cameraFound;
    private Transform _camera;

    private void Start()
    {
        FindCamera();
    }

    private void LateUpdate()
    {
        if (_cameraFound)
        {
            Quaternion cameraRotation = _camera.rotation;
            transform.LookAt(transform.position + cameraRotation * Vector3.forward, cameraRotation * Vector3.up);
        }
        else FindCamera();
    }

    private void FindCamera()
    {
        Camera c = FindObjectOfType<Camera>();
        if (c != null)
        {
            _camera = c.transform;
            _cameraFound = true;
        }
    }
}
