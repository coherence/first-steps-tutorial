using System.Linq;
using Coherence.Connection;
using Coherence.Toolkit;
using Coherence.Toolkit.Bindings.TransformBindings;
using UnityEngine;

namespace Coherence.Samples.ServerAuthoritative
{
    /// <summary>
    /// A movement script which executes both on the Client and on the server (Simulator).
    /// The Client sends its inputs via the <see cref="CoherenceInput"/> component but also applies them to itself immediately ("prediction").
    /// The Simulator elaborates the player's position, and syncs it back. The player receives it and applies it to itself in <see cref="OnPositionReceived"/>,
    /// performing a simple reconciliation when the value is too different.
    /// 
    /// This version has simple prediction, so the response for the player should be immediate.
    /// </summary>
    public class ServerAuthoritativeMovementWithPrediction : MonoBehaviour
    {
        public float speed = 10f;
        public Transform serverGhost;

        private CoherenceSync coherenceSync;
        private CoherenceInput coherenceInput;

        private void Awake()
        {
            coherenceSync = GetComponent<CoherenceSync>();
            coherenceInput = GetComponent<CoherenceInput>();

            coherenceSync.CoherenceBridge.onConnected.AddListener(EnableGhost);
            coherenceSync.CoherenceBridge.onDisconnected.AddListener(DisableGhost);
            
            PositionBinding positionBinding = coherenceSync.Bindings.First(b => b is PositionBinding) as PositionBinding;
            positionBinding!.OnNetworkSampleReceived += OnPositionReceived;
        }

        // Enable or disable the ghost object to match when the player is connected
        private void EnableGhost(CoherenceBridge _) => serverGhost.gameObject.SetActive(true);
        private void DisableGhost(CoherenceBridge arg0, ConnectionCloseReason arg1) => serverGhost.gameObject.SetActive(false);

        private void OnPositionReceived(object sampleData, bool stopped, long simulationFrame)
        {
            Debug.Log($"Received position, {coherenceSync.CoherenceBridge.NetworkTime.ClientSimulationFrame.Frame - simulationFrame} frames behind.");
            
            const float mispredictionThreshold = 3f;

            Vector3 simulatorPosition = (Vector3)sampleData;
            serverGhost.position = simulatorPosition;
            float distance = (simulatorPosition - transform.position).magnitude;

            // Too far from Simulator, teleport
            if (distance > mispredictionThreshold) transform.position = simulatorPosition;
        }

        private void Update()
        {
            if (coherenceSync.HasInputAuthority) SendInputToSimulator(); // Client
            else if (coherenceSync.HasStateAuthority) ProcessClientInput(); // Simulator
        }
        
        private void SendInputToSimulator()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Move(input);
            
            coherenceInput.SetAxis2D("Movement", input);
        }

        private void ProcessClientInput()
        {
            Vector2 receivedInput = coherenceInput.GetAxis2D("Movement");
            Move(receivedInput);
        }

        private void Move(Vector2 movement)
        {
            Vector3 movement3D = new Vector3(movement.x, 0f, movement.y);

            if (movement.sqrMagnitude <= 0.01f) return;

            transform.Translate(movement3D * speed * Time.deltaTime, Space.World);
            transform.LookAt(transform.position - movement3D, Vector3.up);
        }

        private void OnDestroy()
        {
            if (coherenceSync.CoherenceBridge == null) return;
            
            coherenceSync.CoherenceBridge.onConnected.RemoveListener(EnableGhost);
            coherenceSync.CoherenceBridge.onDisconnected.RemoveListener(DisableGhost);
        }
    }
}