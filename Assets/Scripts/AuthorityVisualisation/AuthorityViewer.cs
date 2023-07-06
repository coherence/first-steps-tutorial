using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Switches the URP Renderer back and forth between regular one and authority view, when pressing Tab.
/// In authority view, all materials are swapped according to their layer (<see cref="AuthorityBasedLayer"/>)
/// which will display authority as a colour (blue = authority, orange = remote, white = not synced).
/// </summary>
public class AuthorityViewer : MonoBehaviour
{
    public InputActionReference authorityAction;

    private UniversalAdditionalCameraData _cameraData;
    private int _currentRenderer;

    private void Start()
    {
        _cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
    }

    private void OnEnable()
    {
        authorityAction.asset.Enable();
    }

    private void Update()
    {
        if (authorityAction.action.WasPressedThisFrame())
        {
            _currentRenderer = (_currentRenderer + 1) % 2;
            _cameraData.SetRenderer(_currentRenderer);
        }
    }
}
