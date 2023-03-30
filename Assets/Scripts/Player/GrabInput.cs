using UnityEngine;
using UnityEngine.InputSystem;

public class GrabInput : MonoBehaviour
{
    public InputActionReference grabAction;
    
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
        grabAction.action.canceled += OnGrabActionCanceled;
    }

    private void OnDisable()
    {
        grabAction.action.performed -= OnGrabActionPerformed;
        grabAction.action.canceled -= OnGrabActionCanceled;
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
            
            // Attempt to pick up Grabbable
            _grabbableTarget.PickupValidated += OnPickUpValidated;
            _grabbableTarget.RequestPickup(); // This will fire an authority request if entity is remote
            _canToss = false;
        }
        else
        {
            // Grabbable already in hand
            _grabScript.StartChargingToss();
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

    /// <summary>
    /// Fires when the button is released.
    /// </summary>
    private void OnGrabActionCanceled(InputAction.CallbackContext obj)
    {
        if (_canToss)
        {
            if (_grabScript.IsCarryingSomething)
            {
                // Release or throw
                _grabScript.Drop();
            }
        }
        else
        {
            // Enables tossing only after the button has been released once
            _canToss = true;
        }
    }
}