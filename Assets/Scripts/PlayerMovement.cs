using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    bool isRunning;
    public float wallRunSpeed;
    public float swingSpeed;
    public float dashAbilitySpeed;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool doubleJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform orientation;
    private AudioSource playerAudio;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public bool wallrunning;
    public bool activeGrapple;
    public bool swinging;
    public bool dashing;

    public bool wallrunPowerActive;
    public bool grapplePowerActive;
    public bool swingPowerActive;
    public bool dashPowerActive;
    public float powerupDuration;

    public WallRunning wrScript;
    public Grappling grappleScript;
    public Swinging swingScript;
    public Dashing dashScript;

    public enum MovementState
    {
        freeze,
        walking,
        running,
        wallrunning,
        grappling,
        swinging,
        dashing,
        air
    }

    public bool freeze;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        isRunning = false;
        doubleJump = false;
        wallrunPowerActive = false;
        grapplePowerActive = false;
        swingPowerActive = false;
        dashPowerActive = false;
        wrScript = GetComponent<WallRunning>();
        grappleScript = GetComponent<Grappling>();
    }

    
    void Update()
    {
        PlayerInput();
        SpeedControl();
        Run();
        StateHandler();

        RaycastHit hit;
        //ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        grounded = Physics.SphereCast(transform.position + Vector3.up * 5, 3, Vector3.down, out hit, playerHeight, whatIsGround);


        //handles drag per ground check
        if (grounded && !activeGrapple)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jumping
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

            Invoke(nameof(ActivateDoubleJump), 0.4f);

            Invoke(nameof(DectivateDoubleJump), 0.9f);
        }
        else if (Input.GetKey(jumpKey) && doubleJump)
        {
            jumpForce = 14f;
            
            Jump();

            doubleJump = false;

            jumpForce = 18f;
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            desiredMoveSpeed = 0;
            rb.velocity = Vector3.zero;
        }
        
        // Mode - Running
        else if(grounded && Input.GetKey(runKey))
        {
            state = MovementState.running;
            desiredMoveSpeed = runSpeed;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        
        // Mode - Wallrunning
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
            doubleJump = false;
        }

        // Mode - Grappling
        else if (activeGrapple)
        {
            state = MovementState.grappling;
            doubleJump = false;
        }
        
        // Mode - Swinging
        else if (swinging)
        {
            state = MovementState.swinging;
            desiredMoveSpeed = swingSpeed;
        }

        // Mode - Dashing
        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashAbilitySpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        
        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < runSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                desiredMoveSpeed = runSpeed;
            }
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing)
        {
            keepMomentum = true;
        }

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopCoroutine(SmoothlyLerpMoveSpeed());
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopCoroutine(SmoothlyLerpMoveSpeed());
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private float speedChangeFactor;

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly transitions moveSpeed to desiredMoveSpeed
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        if (state == MovementState.dashing)
        {
            return;
        }
        
        if (activeGrapple)
        {
            return;
        }

        if (swinging)
        {
            return;
        }
        
        // calculates movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        if (!wallrunning)
        {
            rb.useGravity = !OnSlope();
        }
    }

    private void SpeedControl()
    {
        if (activeGrapple)
        {
            return;
        }
        
        // limits speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //velocity limiter
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

        // limit y velocity
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
    }

    public bool exitingSlope;

    private void Jump()
    {
        exitingSlope = true;
        
        //resets y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void ActivateDoubleJump()
    {
        doubleJump = true;
    }

    private void DectivateDoubleJump()
    {
        doubleJump = false;
    }

    private void Run()
    {
        if (Input.GetKey(runKey))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (isRunning == true)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    private bool enableMovementOnNextTouch;

    public void JumpToPosition(Vector3 targetPosition, float trajectoryheight)
    {
        activeGrapple = true;
        
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryheight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity) / 3);

        return velocityXZ + velocityY;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "WallrunPickup")
        {
            //playerAudio.PlayOneShot(pickupSound, 1.0f);
            wallrunPowerActive = true;
            //Destroy(collider.gameObject);
            //powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(WallrunPowerCooldown());
        }
        if (collider.tag == "GrapplePickup")
        {
            //playerAudio.PlayOneShot(pickupSound, 1.0f);
            grapplePowerActive = true;
            //Destroy(collider.gameObject);
            //powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(GrapplePowerCooldown());
        }
        if (collider.tag == "SwingPickup")
        {
            //playerAudio.PlayOneShot(pickupSound, 1.0f);
            swingPowerActive = true;
            //Destroy(collider.gameObject);
            //powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(SwingPowerCooldown());
        }
        if (collider.tag == "DashPickup")
        {
            //playerAudio.PlayOneShot(pickupSound, 1.0f);
            dashPowerActive = true;
            //Destroy(collider.gameObject);
            //powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(DashPowerCooldown());
        }
    }

    IEnumerator WallrunPowerCooldown()
    {
        yield return new WaitForSeconds(powerupDuration);
        wallrunPowerActive = false;
        //powerupIndicator.gameObject.SetActive(false);
        wrScript.StopWallRun();
    }

    IEnumerator GrapplePowerCooldown()
    {
        yield return new WaitForSeconds(powerupDuration);
        grapplePowerActive = false;
        //powerupIndicator.gameObject.SetActive(false);
        grappleScript.StopGrapple();
    }

    IEnumerator SwingPowerCooldown()
    {
        yield return new WaitForSeconds(powerupDuration);
        swingPowerActive = false;
        //powerupIndicator.gameObject.SetActive(false);
        swingScript.StopSwing();
    }

    IEnumerator DashPowerCooldown()
    {
        yield return new WaitForSeconds(powerupDuration);
        dashPowerActive = false;
        //powerupIndicator.gameObject.SetActive(false);
        dashScript.ResetDash();
    }
}
