using Coherence;
using Coherence.Toolkit;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Animator animator;

    [field: Header("Inputs")] public Vector3 MoveInput { get; set; }
    public bool IsSprinting { get; set; }

    [Header("Horizontal movement")] public float movementSpeed = 8f;
    public float runMultiplier = 1.3f;
    public float rotationSpeed = 4f;
    public float acceleration = 2f;
    public float airAcceleration = .5f;
    public float deceleration = 8f;
    public float airDeceleration = 0;

    [Header("Jump")] public float weight = 2f;
    public float jumpLength = .5f; // time for a "full jump", releasing button within this time limits jump height
    public float jumpForce = 10;
    public float jumpCutOff = .25f; // when releasing button mid-jump, up-vel is multiplied by this value
    public float coyoteTime = .125f;
    public float maxFallSpeed = 1;

    [Header("Spring")] public float cruiseHeight = .5f;
    public float raycastLength = .8f;
    public float springStrength = 200f;
    public float dampenFactor = 20f;
    public LayerMask walkableLayers;

    [Header("Debug")] public bool debug = false;

    private bool _hasAnimator;
    private bool _hasCoherenceSync;
    private bool _isGrounded;
    private bool _isOnMovingPlatform;
    private FloatingPlatform _movingPlatform;
    private float _currentSpeed;
    private Rigidbody _rigidbody;
    private CoherenceSync _sync;
    private Vector3 _horizontalVelocity;
    private Vector3 _verticalVelocity;
    private Vector3 _springVector;
    private Vector3 _previousInputVector;
    private float _previousInputMagnitude;
    private float _timeSinceGrounded = 0;
    private float _jumpTimer = 0;
    private bool _isFalling = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _sync = GetComponent<CoherenceSync>();
        _hasCoherenceSync = _sync != null;

        _hasAnimator = animator != null;
    }

    private void Update()
    {
        UpdateJumping();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rigidbody.velocity;

        VerticalMovement(velocity);

        // Horizontal movement
        float inputMagnitude = MoveInput.magnitude;

        float inputAcceleration = inputMagnitude > _previousInputMagnitude
            ?
            _isGrounded ? acceleration : airAcceleration
            :
            _isGrounded
                ? deceleration
                : airDeceleration;

        Vector3 lerpedInputVector = Vector3.Lerp(_previousInputVector, MoveInput, Time.deltaTime * inputAcceleration);
        float lerpedMagnitude = lerpedInputVector.magnitude;

        _currentSpeed = IsSprinting ? movementSpeed * runMultiplier : movementSpeed;
        _horizontalVelocity = lerpedInputVector * _currentSpeed;

        // Caching for next frame
        _previousInputMagnitude = lerpedMagnitude;
        _previousInputVector = lerpedInputVector;

        // Cap vertical velocity when falling 
        if (_isFalling && _verticalVelocity.magnitude > maxFallSpeed)
            _verticalVelocity = _verticalVelocity.normalized * maxFallSpeed;

        // Apply both components to find final frame velocity
        _rigidbody.velocity = _horizontalVelocity + _verticalVelocity;
        if (!_isGrounded) _verticalVelocity = Vector3.Project(_rigidbody.velocity, Vector3.up);

        if (debug)
        {
            Debug.DrawRay(transform.position, _horizontalVelocity, Color.green, Time.deltaTime, false);
            Debug.DrawRay(transform.position, _verticalVelocity, Color.red, Time.deltaTime, false);
            Debug.DrawRay(transform.position, Vector3.up * -cruiseHeight, Color.blue, Time.deltaTime, false);
        }

        if (inputMagnitude != 0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(MoveInput);
            transform.rotation = Quaternion.Lerp(_rigidbody.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }

        if (_hasAnimator) UpdateAnimatorParameters();
    }

    private void VerticalMovement(Vector3 velocity)
    {
        // Vertical movement
        float rayLength = _isGrounded ? raycastLength : cruiseHeight;
        if (_jumpTimer > 0)
        {
            // Is jumping up
            ApplyGravity();
        }
        else
        {
            // Check for ground
            bool wasGrounded = _isGrounded;
            Ray ray = new(transform.position, Vector3.down);
            _isGrounded = Physics.Raycast(ray, out RaycastHit raycastHit, rayLength, walkableLayers,
                QueryTriggerInteraction.Ignore);
            if (_jumpTimer > 0)
                _isGrounded = false;

            if (_isGrounded)
            {
                // Just landed on a moving platform?
                if (!wasGrounded || _movingPlatform == null)
                {
                    if (raycastHit.rigidbody != null)
                    {
                        _isOnMovingPlatform = raycastHit.rigidbody.TryGetComponent(out FloatingPlatform platform);
                        _movingPlatform = _isOnMovingPlatform ? platform : null;
                    }
                    if (_isOnMovingPlatform) GetOntoPlatform();
                }
                
                // Ground spring
                float delta = raycastHit.distance - cruiseHeight;
                float spring = delta * springStrength - -velocity.y * dampenFactor;

                _springVector = spring * ray.direction;

                _verticalVelocity = Vector3.Lerp(_verticalVelocity, _springVector, Time.deltaTime * 20f);
                _timeSinceGrounded = 0;
            }
            else
            {
                if(wasGrounded) LeaveGround();
                // Free falling
                ApplyGravity();
                if (_hasAnimator) animator.SetBool("IsFalling", _isFalling);
            }
        }

        if (debug) Debug.DrawRay(transform.position, Vector3.up * -rayLength, Color.cyan, Time.deltaTime, false);
    }

    private void UpdateAnimatorParameters()
    {
        float animationSpeed = _horizontalVelocity.magnitude / _currentSpeed;

        animator.SetFloat("MoveSpeed", animationSpeed);
        animator.SetBool("Grounded", _isGrounded);
    }


    private void ApplyGravity()
    {
        _verticalVelocity += Vector3.up * (-weight * Time.deltaTime);
        _timeSinceGrounded += Time.deltaTime;
    }

    public void TryJump()
    {
        bool canJump = _isGrounded || _timeSinceGrounded < coyoteTime;
        if (canJump) Jump();
    }

    public void InterruptJump()
    {
        _jumpTimer = -1;
        _verticalVelocity *= jumpCutOff;
    }

    private void UpdateJumping()
    {
        if (_jumpTimer > 0)
        {
            _jumpTimer -= Time.deltaTime;
            if (_jumpTimer < 0f) InterruptJump();
        }

        _isFalling = !_isGrounded && Vector3.Dot(_rigidbody.velocity, Vector3.up) < 0;
    }

    private void Jump()
    {
        if (_isGrounded && _hasAnimator)
        {
            animator.SetTrigger("Jump");
            if (_hasCoherenceSync) _sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Jump");
        }
        _jumpTimer = jumpLength;

        _verticalVelocity = Vector3.up * jumpForce;
        Vector3 d = _rigidbody.velocity;
        d -= Vector3.Project(d, Vector3.up);
        d += _verticalVelocity;
        _rigidbody.velocity = d;
        
        LeaveGround();

        _isGrounded = false;
    }

    private void GetOntoPlatform()
    {
        transform.SetParent(_movingPlatform.transform, true);
    }

    private void LeaveGround()
    {
        _isGrounded = false;
        if (_isOnMovingPlatform)
        {
            transform.SetParent(null, true);
            _movingPlatform = null;
            _isOnMovingPlatform = false;
        }
    }
}