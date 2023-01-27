using UnityEngine;
using UnityEngine.InputSystem;

public class RobotArm : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference rotateBaseAction;
    public InputActionReference climbAction;
    public InputActionReference protrudeAction;

    [Header("Parts")]
    public Transform armBase;
    public Transform handIKtarget;

    [Header("Speed")]
    public float baseRotationSpeed = 70f;
    public float moveSpeed = 3f;

    private float _currentHeadXAngle;

    private void OnEnable()
    {
        rotateBaseAction.action.Enable();
        climbAction.action.Enable();
        protrudeAction.action.Enable();
    }

    private void Update()
    {
        // Rotate arm base left/right
        float currentRotation = rotateBaseAction.action.ReadValue<float>() * baseRotationSpeed * Time.deltaTime;
        armBase.Rotate(0f, currentRotation, 0f, Space.Self);

        // Move IK target up/down and back/forward
        float climbMovement = climbAction.action.ReadValue<float>() * moveSpeed * Time.deltaTime;
        float protrudeMovement = protrudeAction.action.ReadValue<float>() * moveSpeed * Time.deltaTime;
        handIKtarget.Translate(-protrudeMovement, climbMovement, 0f, armBase);

        // Limit floor position
        Vector3 handLocalPosition = handIKtarget.localPosition;
        if (handLocalPosition.y < 3.3f)
        {
            handLocalPosition.y = 3.3f;
            handIKtarget.localPosition = handLocalPosition;
        }

        // Capping the distance
        Vector3 baseToHand = handIKtarget.position - armBase.position;
        if(baseToHand.magnitude > 6.5f)
        {
            baseToHand = baseToHand.normalized * 6.5f;
            handIKtarget.position = armBase.position + baseToHand;
        }
        else if(baseToHand.magnitude < 4f)
        {
            baseToHand = baseToHand.normalized * 4f;
            handIKtarget.position = armBase.position + baseToHand;
        }

        // Auto-rotating the hand based on its height
        float yPositionRatio = Mathf.InverseLerp(3.3f, 7f, handIKtarget.localPosition.y);
        float resultingXRotation = Mathf.Lerp(90f, -110f, yPositionRatio);
        handIKtarget.localEulerAngles = new Vector3(resultingXRotation, -90f, 0f);
    }
}
