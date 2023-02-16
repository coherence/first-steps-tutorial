using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;

    private Vector2 _rawInput;
    private Move _characterController;
    private Transform _cameraTransform;
    
    private void Awake()
    {
        _characterController = GetComponent<Move>();
        _cameraTransform = FindObjectOfType<Camera>().transform;
    }
    private void OnEnable()
    {
        moveAction.asset.Enable();
        jumpAction.asset.Enable();
        sprintAction.asset.Enable();
        
        jumpAction.action.performed += JumpInputPerformed;
        jumpAction.action.canceled += JumpInputCanceled;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= JumpInputPerformed;
        jumpAction.action.canceled -= JumpInputCanceled;
    }
    
    private void JumpInputPerformed(InputAction.CallbackContext context) => Jump();
    private void JumpInputCanceled(InputAction.CallbackContext context) => CancelJump();

    private void Update()
    {
        SetMoveInput(moveAction.action.ReadValue<Vector2>());
        SetIsSprinting(sprintAction.action.IsPressed());
    }
    
    public void SetMoveInput(Vector2 moveInput)
    {
        _rawInput = moveInput;
        Vector3 forward = new Vector3( _cameraTransform.forward.x, 0, _cameraTransform.forward.z ).normalized;
        Vector3 right = new Vector3( _cameraTransform.right.x, 0, _cameraTransform.right.z ).normalized;
        _characterController.MoveInput = right * _rawInput.x + forward * _rawInput.y;
    }

    public void SetIsSprinting(bool isSprinting)
    {
        _characterController.IsSprinting = isSprinting;
    }

    public void Jump() => _characterController.TryJump();
    public void CancelJump() => _characterController.InterruptJump();
}