using Coherence.Toolkit;
using UnityEngine;

namespace Coherence.Samples.ServerAuthoritative
{
    /// <summary>
    /// A movement script which executes both on the Client and on the server (Simulator).
    /// The Client sends its inputs via the <see cref="CoherenceInput"/> component, and the Simulator
    /// elaborates the player's position, and syncs it back. 
    /// 
    /// As this version has no prediction, the player will naturally notice a delay in the input.
    /// Check <see cref="ServerAuthoritativeMovementWithPrediction"/> for a more advanced and responsive version.
    /// </summary>
    public class ServerAuthoritativeMovement : MonoBehaviour
    {
        public float speed = 10f;

        private CoherenceSync coherenceSync;
        private CoherenceInput coherenceInput;

        private void Awake()
        {
            coherenceSync = GetComponent<CoherenceSync>();
            coherenceInput = GetComponent<CoherenceInput>();
        }

        private void Update()
        {
            if (coherenceSync.HasInputAuthority) SendInputToSimulator(); // Client
            else if (coherenceSync.HasStateAuthority) ProcessClientInput(); // Simulator
        }
        
        private void SendInputToSimulator()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            coherenceInput.SetAxis2D("Movement", input);
        }

        private void ProcessClientInput()
        {
            Vector2 movement = coherenceInput.GetAxis2D("Movement");
            Vector3 movement3D = new Vector3(movement.x, 0f, movement.y);

            if (movement.sqrMagnitude <= 0.01f) return;

            transform.Translate(movement3D * speed * Time.deltaTime, Space.World);
            transform.LookAt(transform.position - movement3D, Vector3.up);
        }
    }
}