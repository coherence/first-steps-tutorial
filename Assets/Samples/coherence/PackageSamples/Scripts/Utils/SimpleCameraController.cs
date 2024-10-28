using UnityEngine;

namespace Coherence.Samples
{
    /// <summary>
    /// A simple fly-through camera script.
    /// </summary>
    public class SimpleCameraController : MonoBehaviour
    {
        public float mainSpeed = 1.0f;
        public float camSens = 0.25f;
        public bool invertY = true;

        private float totalRun = 1.0f;

        private void Update()
        {
            RotateCamera();

            Vector3 p = GetBaseInput();

            if (p.sqrMagnitude <= 0)
            {
                return;
            }

            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p *= mainSpeed * Time.deltaTime;

            transform.Translate(p);
        }

        private void RotateCamera()
        {
            if (Input.GetMouseButton(1))
            {
                float mouseMoveY = invertY ? -1 * Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
                float mouseMoveX = Input.GetAxis("Mouse X");

                Vector3 mouseMove = new Vector3(mouseMoveY, mouseMoveX, 0) * camSens;
                transform.eulerAngles = transform.eulerAngles + mouseMove;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private Vector3 GetBaseInput()
        {
            Vector3 pVelocity = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                pVelocity += Vector3.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                pVelocity += Vector3.back;
            }

            if (Input.GetKey(KeyCode.A))
            {
                pVelocity += Vector3.left;
            }

            if (Input.GetKey(KeyCode.D))
            {
                pVelocity += Vector3.right;
            }

            return pVelocity;
        }
    }
}
