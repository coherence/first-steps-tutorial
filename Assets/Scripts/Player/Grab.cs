using System;
using Coherence.Toolkit;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public Animator animator;
    public Transform holdSocket;
    public float tossTime = 1f; // maximum "charge time" if holding the toss button down 

    public bool IsCarryingSomething { get; private set; }
    
    private Grabbable _grabbedObject;
    private Rigidbody _grabbedObjectRB;
    private Collider _grabbedObjectCollider;
    private CoherenceSync _grabbedObjectSync;
    private float _tossChargeTimer;
    private bool _isChargingToss;
    
    private float TossForce => Mathf.Clamp(_tossChargeTimer,.2f, 1f);

    private void Update()
    {
        if(_isChargingToss)
        {
            _tossChargeTimer += Time.deltaTime;
            holdSocket.Rotate( 0f, _tossChargeTimer * 666f * Time.deltaTime, 0 );
            if( _tossChargeTimer > tossTime ) _tossChargeTimer = tossTime;
        }
    }

    public void PickUp(Grabbable target)
    {
        animator.SetBool("CarryingBig", true);
        holdSocket.rotation = Quaternion.identity;
        _grabbedObjectRB = target.gameObject.GetComponent<Rigidbody>();
        _grabbedObjectRB.isKinematic = true;
        _grabbedObjectCollider = target.GetComponent<Collider>();
        _grabbedObjectCollider.enabled = false;
        _grabbedObject = target;
        _grabbedObject.transform.SetParent(holdSocket, false);
        _grabbedObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        _grabbedObjectSync = _grabbedObject.GetComponent<CoherenceSync>();
        _grabbedObjectSync.OnStateRemote.AddListener(OnGrabbableStolen);
        IsCarryingSomething = true;
    }

    /// <summary>
    /// Contains the actions that are performed on the grabbable.
    /// Only happens when the player
    /// </summary>
    public void Drop()
    {
        _grabbedObjectCollider.enabled = true;
        _grabbedObject.transform.SetParent(null, true);
        _grabbedObjectRB.isKinematic = false;
        _grabbedObjectRB.AddForce( transform.forward * (TossForce * 12f), ForceMode.VelocityChange );
        _grabbedObjectRB.AddRelativeTorque( -Vector3.right * (TossForce * 5f), ForceMode.VelocityChange );

        LetGo();
    }

    /// <summary>
    /// Contains the actions that are performed by the character on itself,
    /// that is, the ones that happen when the grabbable is let go but not thrown.
    /// For instance, when stolen.
    /// </summary>
    private void LetGo()
    {
        animator.SetBool("CarryingBig", false);
        _grabbedObjectSync.OnStateRemote.RemoveListener(OnGrabbableStolen);
        
        _grabbedObjectRB = null;
        _grabbedObjectCollider = null;
        _grabbedObject = null;
        _isChargingToss = false;
        IsCarryingSomething = false;
    }

    private void OnGrabbableStolen()
    {
        LetGo();
    }

    public void StartChargingToss()
    {
        _tossChargeTimer = 0f;
        _isChargingToss = true;
    }

    private void OnDisable()
    {
        // Since the player gets destroyed when exiting Play mode,
        // make sure to unparent the crate by dropping it
        if (IsCarryingSomething) Drop();
    }
}

