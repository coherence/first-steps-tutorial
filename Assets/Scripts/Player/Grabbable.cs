using System;
using Coherence;
using Coherence.Toolkit;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public event Action<bool> PickupValidated;
    
    private CoherenceSync _coherenceSync;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _isWaitingForConfirmation;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _coherenceSync = GetComponent<CoherenceSync>();
    }
        
    private void OnEnable()
    {
        _coherenceSync.OnStateAuthority.AddListener(OnStateAuthority);
        _coherenceSync.OnStateRemote.AddListener(OnStateRemote);
        _coherenceSync.OnAuthorityRequestRejected.AddListener(OnAuthorityRequestRejected);
    }

    private void OnDisable()
    {
        _coherenceSync.OnStateAuthority.RemoveListener(OnStateAuthority);
        _coherenceSync.OnStateRemote.RemoveListener(OnStateRemote);
        _coherenceSync.OnAuthorityRequestRejected.RemoveListener(OnAuthorityRequestRejected);
    }
        
    /// <summary>
    /// Verifies that the object can be picked up. If the player attempting the action has authority, it succeeds instantly.
    /// If not, it requests it. Scripts need to listen to <see cref="PickupValidated"/> to get the asynchronous result.
    /// </summary>
    public void RequestPickup()
    {
        if (_coherenceSync.HasStateAuthority)
        {
            ConfirmPickup();
        }
        else
        {
            _isWaitingForConfirmation = true;
            _coherenceSync.RequestAuthority(AuthorityType.Full);
        }
    }

    private void OnAuthorityRequestRejected(AuthorityType authType)
    {
        PickupValidated?.Invoke(false);
    }

    /// <summary>
    /// Object can become authoritative in two ways: when picked up (from the ground, or stolen),
    /// and when it bumps into a player that doesn't have authority.
    /// </summary>
    private void OnStateAuthority()
    {
        if(_isWaitingForConfirmation)
        {
            // Authority change happened as a result of a pick up action
            _isWaitingForConfirmation = false;
            ConfirmPickup();
        }
        else
        {
            // Authority change happened because of a collision
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }

    /// <summary>
    /// Like gaining authority, losing authority can happen because of two reasons: see <see cref="OnStateAuthority"/>.
    /// </summary>
    private void OnStateRemote()
    {
        _rigidbody.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this Client already has authority over this object, no check is needed
        if (_coherenceSync.HasStateAuthority)
        {
            return;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<CoherenceSync>(out CoherenceSync playerCoherenceSync))
            {
                // If player Prefab is this client's player Prefab, give it authority over this grabbable
                if (playerCoherenceSync.HasStateAuthority)
                {
                    _coherenceSync.RequestAuthority(AuthorityType.Full);
                }
            }
        }
    }

    private void ConfirmPickup()
    {
        PickupValidated?.Invoke(true);
    }
}