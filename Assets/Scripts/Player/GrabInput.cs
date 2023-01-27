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
        //grabAction.asset.Disable();
        grabAction.action.performed -= OnGrabActionPerformed;
        grabAction.action.canceled -= OnGrabActionCanceled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_grabScript.IsCarryingSomething)
            return;

        if (other.CompareTag("Grabbable")) _grabbableTarget = other.gameObject.GetComponent<Grabbable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_grabScript.IsCarryingSomething)
            return;

        if (other.gameObject.GetComponent<Grabbable>() == _grabbableTarget) _grabbableTarget = null;
    }

    private void OnGrabActionPerformed(InputAction.CallbackContext obj)
    {
        if (!_grabScript.IsCarryingSomething)
        {
            if (_grabbableTarget == null)
                return;
            
            // Pick up Grabbable
            _grabbableTarget.PickupValidated += OnPickUpValidated;
            _grabbableTarget.Pickup(); // This will fire an authority request
        }
        else
        {
            // Grabbable already in hand
            _grabScript.StartChargingToss();
        }
    }

    private void OnPickUpValidated(bool success)
    {
        _grabbableTarget.PickupValidated -= OnPickUpValidated;
            
        if (success)
        {
            PickUp();
        }
        else
        {
            // TODO: pickup failed. Display some feedback?
        }
    }

    private void PickUp()
    {
        _grabScript.PickUp(_grabbableTarget);
        _grabbableTarget = null;
        _canToss = false;
    }

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