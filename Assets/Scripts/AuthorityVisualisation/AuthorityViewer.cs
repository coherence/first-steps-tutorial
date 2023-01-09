using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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
