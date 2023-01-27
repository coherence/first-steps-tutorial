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

    private float _tossChargeTimer = 0;
    bool _isChargingToss = false;

    private void FixedUpdate()
    {
        if(IsCarryingSomething)
        {
            _grabbedObjectRB.MovePosition( Vector3.Lerp( _grabbedObject.transform.position, holdSocket.position, .85f ) );
            _grabbedObjectRB.MoveRotation( Quaternion.Lerp( _grabbedObject.transform.rotation, holdSocket.rotation, .85f ) );
            _grabbedObjectRB.velocity = Vector3.zero;
        }
    }

    private void Update()
    {
        if(_isChargingToss)
        {
            _tossChargeTimer += Time.deltaTime;
            holdSocket.Rotate( 0f, _tossChargeTimer * 666f * Time.deltaTime, 0 );
            if( _tossChargeTimer > tossTime ) _tossChargeTimer = tossTime;
        }
    }

    private float _tossForce => Mathf.Clamp(_tossChargeTimer,.2f, 1f);

    public void PickUp(Grabbable target)
    {
        animator.SetBool("CarryingBig", true);
        holdSocket.rotation = Quaternion.identity;
        _grabbedObjectRB = target.gameObject.GetComponent<Rigidbody>();
        _grabbedObjectCollider = target.GetComponent<Collider>();
        _grabbedObjectCollider.enabled = false;
        _grabbedObject = target;
        IsCarryingSomething = true;
    }

    public void Drop()
    {
        animator.SetBool("CarryingBig", false);
        _grabbedObjectCollider.enabled = true;
        _grabbedObjectRB.AddForce( transform.forward * (_tossForce * 12f), ForceMode.VelocityChange );
        _grabbedObjectRB.AddRelativeTorque( -Vector3.right * (_tossForce * 5f), ForceMode.VelocityChange );
        _grabbedObjectRB = null;
        _grabbedObjectCollider = null;
        IsCarryingSomething = false;
        _isChargingToss = false;

        _grabbedObject = null;
    }

    public void StartChargingToss()
    {
        _tossChargeTimer = 0f;
        _isChargingToss = true;
    }
}

