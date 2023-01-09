using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    private bool _cameraFound;
    private Transform _camera;

    private void LateUpdate()
    {
        if (_cameraFound)
        {
            transform.LookAt(transform.position + _camera.rotation * Vector3.forward, _camera.rotation * Vector3.up);
        }
        else
        {
            FindCamera();
        }
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
