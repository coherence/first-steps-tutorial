using UnityEngine;

namespace Coherence.Samples
{
    /// <summary>
    /// Simple Client-side movement script, used in multiple samples.
    /// </summary>
    public class SimpleMovement : MonoBehaviour
    {
        public float speed = 10f;

        private void Update()
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            if (movement.sqrMagnitude <= 0.01f) return;

            transform.Translate(movement * speed * Time.deltaTime, Space.World);
            transform.LookAt(transform.position - movement, Vector3.up);
        }
    }
}
