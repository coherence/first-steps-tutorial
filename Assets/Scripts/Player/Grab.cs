using UnityEngine;

public class Grab : MonoBehaviour
{
    public Animator animator;
    public Transform holdSocket;

    public bool IsCarryingSomething { get; private set; }
    
    private Grabbable _grabbedObject;
    private Rigidbody _grabbedObjectRB;
    private Collider _grabbedObjectCollider;

    public void PickUp(Grabbable target)
    {
        animator.SetBool("CarryingBig", true);
        _grabbedObject = target;
        _grabbedObjectRB = _grabbedObject.GetComponent<Rigidbody>();
        _grabbedObjectCollider = _grabbedObject.GetComponent<Collider>();
        _grabbedObjectRB.isKinematic = true;
        _grabbedObjectCollider.enabled = false;
        _grabbedObject.transform.SetParent(holdSocket, false);
        _grabbedObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        IsCarryingSomething = true;
    }

    /// <summary>
    /// Contains the actions that are performed on the grabbable.
    /// Only happens when the player presses the input to throw/drop the interactable.
    /// </summary>
    public void Drop(float throwStrength = 0f)
    {
        _grabbedObject.transform.SetParent(null, true);
        _grabbedObjectRB.isKinematic = false;
        _grabbedObjectRB.AddForce(throwStrength * transform.forward, ForceMode.VelocityChange );
        _grabbedObjectRB.AddTorque( -transform.right * throwStrength * 1f, ForceMode.VelocityChange );
        _grabbedObject.Release();
        
        LetGo();
    }

    /// <summary>
    /// Contains the actions that are performed by the character on itself,
    /// that is, the ones that happen when the grabbable is let go but not intentionally thrown.
    /// For instance, when stolen.
    /// </summary>
    private void LetGo()
    {
        animator.SetBool("CarryingBig", false);
        _grabbedObjectCollider.enabled = true;
        
        _grabbedObjectRB = null;
        _grabbedObjectCollider = null;
        _grabbedObject = null;
        IsCarryingSomething = false;
    }

    private void OnDisable()
    {
        // Since the player gets destroyed when exiting Play mode,
        // make sure to unparent the crate by dropping it
        if (IsCarryingSomething) Drop();
    }
}