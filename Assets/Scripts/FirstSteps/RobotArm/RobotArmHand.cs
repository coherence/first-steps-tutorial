using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Handles player-initiated input to pick up and release the crate, and also its detection.
    /// </summary>
    public class RobotArmHand : MonoBehaviour
    {
        public InputActionReference pickupAction;
        public Animator animatorComponent;
        public CoherenceSync sync;
    
        [Header("Transported object")]
        public Transform grabbableObject;
        public bool isCarryingObject;
    
        [Header("Parent position")]
        public Vector3 localPositionOnAttach = new Vector3(0f, 0.25f, 0f);
        public Vector3 localRotationOnAttach = new Vector3(-180f, 180f, 180f);
    
        private void OnEnable()
        {
            pickupAction.action.Enable();
            pickupAction.action.performed += OnPickupPerformed;
        }

        private void OnDisable()
        {
            pickupAction.action.Disable();
            pickupAction.action.performed -= OnPickupPerformed;
        }

        private void OnPickupPerformed(InputAction.CallbackContext obj)
        {
            if (isCarryingObject)
            {
                // Drop it
                animatorComponent.SetBool("ClawsOpen", true);
                grabbableObject.SetParent(null, true);
                grabbableObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbableObject = null;
                isCarryingObject = false;
            }
            else
            {
                if (grabbableObject != null)
                {
                    // Pickup
                    animatorComponent.SetBool("ClawsOpen", false);
                    grabbableObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbableObject.SetParent(transform, true);
                    grabbableObject.localPosition = localPositionOnAttach;
                    grabbableObject.localEulerAngles = localRotationOnAttach;
                    isCarryingObject = true;
                }
                else
                {
                    // Missed pickup
                    animatorComponent.SetTrigger("Miss");
                    sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Miss");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Grabbable"))
            {
                grabbableObject = other.attachedRigidbody.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Grabbable"))
            {
                grabbableObject = null;
            }
        }
    }
}
