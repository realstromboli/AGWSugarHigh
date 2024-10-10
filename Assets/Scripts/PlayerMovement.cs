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

    public bool wallrunPowerActive;
    public bool grapplePowerActive;
    public float powerupDuration;

    public WallRunning wrScript;
    public Grappling grappleScript;

    public enum MovementState
    {
        freeze,
        walking,
        running,
        wallrunning,
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
        wrScript = GetComponent<WallRunning>();
        grappleScript = GetComponent<Grappling>();
    }

    
    void Update()
    {
        PlayerInput();
        SpeedControl();
        Run();
        StateHandler();

        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

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

            Invoke(nameof(DectivateDoubleJump), 1.0f);
        }
        else if (Input.GetKey(jumpKey) && doubleJump)
        {
            jumpForce = 14f;
            
            Jump();

            doubleJump = false;

            jumpForce = 18f;
        }
    }

    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }
        
        // Mode - Running
        else if(grounded && Input.GetKey(runKey))
        {
            state = MovementState.running;
            moveSpeed = runSpeed;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
        }

        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallRunSpeed;
            doubleJump = false;
        }
    }

    private void MovePlayer()
    {
        if (activeGrapple)
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
        if (grounded)
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
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity) / 4);

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
}
