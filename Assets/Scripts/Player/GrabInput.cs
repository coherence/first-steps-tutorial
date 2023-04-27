using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GrabInput : MonoBehaviour
{
    public InputActionReference grabAction;
    public Move moveScript;
    
    private Grab _grabScript;
    private Grabbable _grabbableTarget;
    private bool _canToss;

    private void Awake()
    {
        _grabScript = GetComponent<Grab>();
    }

    private void OnEnable()
    {
        grabAction.asset.Enable();
        grabAction.action.performed += OnGrabActionPerformed;
    }

    private void OnDisable()
    {
        grabAction.action.performed -= OnGrabActionPerformed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_grabScript.IsCarryingSomething)
            return;

        // Found a grabbable object
        if (other.CompareTag("Grabbable"))
            _grabbableTarget = other.gameObject.GetComponent<Grabbable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_grabScript.IsCarryingSomething)
            return;

        if (other.gameObject.GetComponent<Grabbable>() == _grabbableTarget)
            _grabbableTarget = null;
    }

    /// <summary>
    /// Fires when the button is pressed.
    /// </summary>
    private void OnGrabActionPerformed(InputAction.CallbackContext obj)
    {
        if (!_grabScript.IsCarryingSomething)
        {
            if (_grabbableTarget == null)
                return;
            
            _canToss = false;
            
            // Attempt to pick up Grabbable
            _grabbableTarget.PickupValidated += OnPickUpValidated;
            _grabbableTarget.RequestPickup(); // This will fire an authority request if entity is remote
        }
        else if(_canToss)
        {
            // Release or throw
            float speed = moveScript.ThrowSpeed();
            _grabScript.Drop(speed);
            moveScript.ApplyThrowPushback(speed);
        }
    }

    /// <summary>
    /// Received a response from the grabbable we're trying to pick up.
    /// Since the object could be remote, this includes a request of authority,
    /// which might lead it to fail if the object is set to not concede authority.
    /// </summary>
    /// <param name="success">Whether the pickup was authorized or not.</param>
    private void OnPickUpValidated(bool success)
    {
        _grabbableTarget.PickupValidated -= OnPickUpValidated;
        _canToss = true;
            
        if (success)
        {
            PickUp();
        }
    }
    
    private void PickUp()
    {
        _grabScript.PickUp(_grabbableTarget);
        _grabbableTarget = null;
    }
}