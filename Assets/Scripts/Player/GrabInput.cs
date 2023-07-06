using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player-initiated input to grab and throw items when a button is pressed.
/// This is disabled on remote players.
/// </summary>
public class GrabInput : MonoBehaviour
{
    public InputActionReference grabAction;
    public Move moveScript;
    
    private Grab _grabScript;
    private Grabbable _potentialTarget;
    private Grabbable _grabbable;
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
            _potentialTarget = other.gameObject.GetComponent<Grabbable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_grabScript.IsCarryingSomething)
            return;

        if (other.gameObject.GetComponent<Grabbable>() == _potentialTarget)
            _potentialTarget = null;
    }

    /// <summary>
    /// Fires when the button is pressed.
    /// </summary>
    private void OnGrabActionPerformed(InputAction.CallbackContext obj)
    {
        if (!_grabScript.IsCarryingSomething)
        {
            // Pick up
            if (_potentialTarget != null
                && !_potentialTarget.isBeingCarried)
            {
                _canToss = false;
            
                // Pick up Grabbable
                _grabbable = _potentialTarget;
                _grabbable.PickupValidated += OnPickUpValidated;
                _grabbable.RequestPickup(); // This will fire an authority request if the entity is remote
            }
        }
        else if(_canToss)
        {
            // Release or throw
            float speed = moveScript.ThrowSpeed();
            _grabScript.Drop(speed);
            moveScript.ApplyThrowPushback(speed);
            _grabbable = null;
        }
    }

    /// <summary>
    /// Received a response from the grabbable we're trying to pick up.
    /// Since the object could be remote, this includes a request of authority.
    /// </summary>
    /// <param name="success">Whether the pickup was authorized or not.</param>
    private void OnPickUpValidated(bool success)
    {
        _grabbable.PickupValidated -= OnPickUpValidated;
        _canToss = true;
            
        if (success)
        {
            // The object was just laying around
            _grabScript.PickUp(_grabbable);
            _potentialTarget = null;
        }
        else
        {
            // Pickup can fail when a grabbable that was free up to a moment ago JUST
            // got picked up by another player on the network.
            // Locally this client is not aware yet because the Grabbable.isBeingCarried property
            // hasn't synced yet, but when requesting authority for the pickup - they get rejected
            // in the Grabbable.OnAuthorityRequested callback.
            _grabbable = null;
        }
    }
}