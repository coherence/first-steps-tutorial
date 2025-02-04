using UnityEngine;
using UnityEngine.Splines;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// A moving platform.
    /// If a GameObject has this script on, the <see cref="Move"/> class will recognise it when landing a jump,
    /// and parent its GameObject to this one.
    /// </summary>
    public class FloatingPlatform : MonoBehaviour
    {
        public SplineContainer spline;
        public Vector3 moveOffset; // Moves to _startPos+moveOffset (unless there is a spline to use) 
        public AnimationCurve curve;
    
        [Header("Spline movement")]
        [Range(0f, 1f)] public float timer;
        public float moveTime = 2; // How many seconds does the entire movement take? 
        public float idleDelay = 2; // Platform chills at each end point for a while before starting to move again 
        public bool pingPong = true; // Resets to _startPos when done if not pingPong
        public bool rotateAlongSpline = false;

        private Vector3 _startPos;
        private float _idleTimer;
        private bool _returning; // Moves backwards when true
        private Vector3 _approximatedVelocity; // Can be used to set velocity of things jumping off the platform 

        private void Awake()
        {
            _startPos = transform.position;
            _idleTimer = idleDelay;
        }

        private void OnValidate()
        {
            if (spline != null)
            {
                Move();
            }
        }

        private void FixedUpdate()
        {
            if (_idleTimer > 0)
            {
                _idleTimer -= Time.deltaTime;
            }
            else
            {
                timer += Time.deltaTime / moveTime;
                if (timer >= 1)
                {
                    timer = 0;
                    if (pingPong) _returning = !_returning;
                    _idleTimer = idleDelay;
                }
            }

            // Movement calculations are done in FixedUpdate to match the physical movement of players
            // and the fact that Cinemachine is set to update in FixedUpdate
            Move();
        }

        private void Move()
        {
            Vector3 oldPos = transform.position;
            float t = timer;
            if (pingPong) t = _returning ? 1 - timer : timer;
            float z = curve.Evaluate(t);
            if (spline != null)
            {
                transform.position = spline.EvaluatePosition(z);
                if (rotateAlongSpline)
                {
                    Vector3 tangent = spline.EvaluateTangent(z);
                    if (tangent.sqrMagnitude > 0f)
                    {
                        transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);
                    }
                }
            }
            else
            {
                transform.position = Vector3.Lerp(_startPos, _startPos + moveOffset, z);
            }

            _approximatedVelocity = Vector3.Lerp(_approximatedVelocity,
                (transform.position - oldPos) * (1f / Time.deltaTime),
                .25f);
        }
    }
}