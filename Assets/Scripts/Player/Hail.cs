using Coherence;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hail : MonoBehaviour
{
    public InputActionReference hailAction;
    public Animator animator;
    public CoherenceSync sync;
    
    private bool _hasCoherenceSync;

    private void Awake()
    {
        _hasCoherenceSync = sync != null;
    }

    private void OnEnable()
    {
        hailAction.action.Enable();
        hailAction.action.performed += OnHailPerformed;
    }

    private void OnDisable()
    {
        hailAction.action.performed -= OnHailPerformed;
    }

    private void OnHailPerformed(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Hail");
        if (_hasCoherenceSync) sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Hail");
    }
}
