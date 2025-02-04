using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Handles the player-initiated action of performing a wave animation.
    /// </summary>
    public class Wave : MonoBehaviour
    {
        public InputActionReference waveAction;
        public Animator animator;
        public CoherenceSync sync;

        private void OnEnable()
        {
            waveAction.action.Enable();
            waveAction.action.performed += OnHailPerformed;
        }

        private void OnDisable()
        {
            waveAction.action.performed -= OnHailPerformed;
        }

        private void OnHailPerformed(InputAction.CallbackContext obj)
        {
            animator.SetTrigger("Wave");
            sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Wave");
        }
    }
}
