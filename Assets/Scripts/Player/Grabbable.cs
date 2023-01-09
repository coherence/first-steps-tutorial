using System;
using Coherence;
using Coherence.Toolkit;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool IsBeingCarried { get; private set; }
    public event Action<bool> PickupValidated;
    
    private CoherenceSync _coherenceSync;
    private bool _isWaitingForConfirmation;

    private void Awake()
    {
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
    /// Adds a layer of network validation to base function <see cref="Grabbable.Pickup"/>.
    /// Scripts need to listen to <see cref="PickupValidated"/> to get the asynchronous result.
    /// </summary>
    public void Pickup()
    {
        if (_coherenceSync.HasStateAuthority)
        {
            ConfirmPickup();
            PickupValidated?.Invoke(true);
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

    private void OnStateAuthority()
    {
        if(_isWaitingForConfirmation)
        {
            _isWaitingForConfirmation = false;
            ConfirmPickup();
            PickupValidated?.Invoke(true);
        }
    }

    private void OnStateRemote()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this Client already has authority over this object, no check is needed
        if (IsBeingCarried || _coherenceSync.HasStateAuthority)
        {
            return;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<CoherenceSync>(out CoherenceSync playerCoherenceSync))
            {
                // If player Prefab is this client's player Prefab, give it authority over this pickup
                if (playerCoherenceSync.HasStateAuthority)
                {
                    _coherenceSync.RequestAuthority(AuthorityType.Full);
                }
            }
        }
    }

    private void ConfirmPickup() => IsBeingCarried = true;
    private void ConfirmLetGo() => IsBeingCarried = false;
}