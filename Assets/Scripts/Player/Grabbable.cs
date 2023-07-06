using System;
using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using UnityEngine;

/// <summary>
/// An object that can be grabbed (by activating the <see cref="Grab"/> action).
/// </summary>
public class Grabbable : MonoBehaviour
{
    /// <summary>
    /// This is a flag used to check if a grabbable can be picked up or not, set to sync.
    /// However, since syncing over the network takes time, it might still be false when
    /// a remote player is trying to pick the grabbable up, so we also filter authority
    /// requests with the <see cref="OnAuthorityRequested"/> callback.
    /// </summary>
    [Sync] public bool isBeingCarried;
    
    public event Action<bool> PickupValidated;
    
    private CoherenceSync _coherenceSync;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _pickupRequested;
    private float _lastAuthorityChangeTime;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _coherenceSync = GetComponent<CoherenceSync>();
    }
        
    private void OnEnable()
    {
        _coherenceSync.OnAuthorityRequested += OnAuthorityRequested;
        _coherenceSync.OnStateAuthority.AddListener(OnStateAuthority);
        _coherenceSync.OnStateRemote.AddListener(OnStateRemote);
    }

    private void OnDisable()
    {
        _coherenceSync.OnAuthorityRequested -= OnAuthorityRequested;
        _coherenceSync.OnStateAuthority.RemoveListener(OnStateAuthority);
        _coherenceSync.OnStateRemote.RemoveListener(OnStateRemote);
    }
    
    /// <summary>
    /// This method is called when another client requests authority. It will reject it if the grabbable
    /// has just been picked up by the local player. This avoids many race conditions,
    /// where another player would request authority in the same frame that the local player is picking
    /// the grabbable up, leading to a the object being in a broken state.
    /// </summary>
    private bool OnAuthorityRequested(ClientID requesterid, AuthorityType authoritytype, CoherenceSync sync)
    {
        return !isBeingCarried;
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
            _pickupRequested = true;
            _coherenceSync.RequestAuthority(AuthorityType.Full);
        }
    }

    /// <summary>
    /// Object can become authoritative in two ways: when picked up (from the ground, or stolen),
    /// and when it bumps into a player that doesn't have authority.
    /// </summary>
    private void OnStateAuthority()
    {
        _lastAuthorityChangeTime = Time.time;
        if(_pickupRequested)
        {
            // Authority change happened as a result of a pick up action
            _pickupRequested = false;
            ConfirmPickup();
        }
        else
        {
            // Authority change happened because of a collision
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            isBeingCarried = false;
        }
    }

    /// <summary>
    /// Like gaining authority, losing authority can happen because of two reasons: see <see cref="OnStateAuthority"/>.
    /// </summary>
    private void OnStateRemote()
    {
        _lastAuthorityChangeTime = Time.time;
        _rigidbody.isKinematic = true;
    }
    
    private bool CanChangeAuthority() => Time.time > _lastAuthorityChangeTime + 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (_coherenceSync.HasStateAuthority
            || isBeingCarried
            || !CanChangeAuthority()) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // If the player colliding is local, take authority
            CoherenceSync collidingPlayersSync = collision.gameObject.GetComponent<CoherenceSync>();
            if (collidingPlayersSync.HasStateAuthority)
            {
                // If player is this client's player Prefab, take authority
                if (collidingPlayersSync.HasStateAuthority)
                {
                    _coherenceSync.RequestAuthority(AuthorityType.Full);
                }
            }
        }
    }

    private void ConfirmPickup()
    {
        isBeingCarried = true;
        PickupValidated?.Invoke(true);
    }

    public void Release() => isBeingCarried = false;
}