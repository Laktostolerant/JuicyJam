using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float dashSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;
    public float groundDrag;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("WallRun")]
    public bool wallrunning;

    [Header("Sliding")]
    public bool sliding;

    [Header("Dashing")]
    public bool dashing;

    [Header("Fill in Inspector")]
    public Transform orientation;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        wallRunning,
        crouching,
        sliding,
        dashing,
        air
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }
    private void Update()
    {
        // Ground check
        bool groundedCopy = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        if(!groundedCopy && isGrounded)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Landing", gameObject);
        }
        MyInput();
        SpeedControl();
        StateHandler();

        // Handle Drag
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching || state == MovementState.sliding || state == MovementState.wallRunning)
            rb.drag = groundDrag;
        else
            rb.drag = 0;        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Jump", gameObject);
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // Stop Crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // State - Dashing
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
        }

        // State - WallRunning
        else if (wallrunning)
        {
            state = MovementState.wallRunning;
            desiredMoveSpeed = wallRunSpeed;
        }

        // State - Sliding
        else if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        // State - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // State - Sprinting
        else if (isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // State - Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // State - Air
        else
        {
            state = MovementState.air;
            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;

        }

        // Check if desiredMoveSpeed has changes drastically
        if (Math.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // Smoothly lerp movementSpeed to desired value
        float time = 0;
        float differance = Math.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < differance)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / differance);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else if (state == MovementState.dashing)
            {                
                time += Time.deltaTime * dashSpeedChangeFactor;
            }
            else
            {               
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        keepMomentum = false;
    }
    private void MovePlayer()
    {
        if (state == MovementState.dashing) return;

        // Movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            if (state == MovementState.walking)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step", gameObject);
            }
            else if (state == MovementState.sprinting)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step 2", gameObject);
            }
            else if (state == MovementState.crouching)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step 4", gameObject);
            }

            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if (isGrounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);

            //Makes sure to only play the stepping sound when the player is moving
            if (verticalInput != 0 || horizontalInput != 0)
            {

                if (state == MovementState.walking)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step", gameObject);
                }
                else if (state == MovementState.sprinting)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step 2", gameObject);
                }
                else if(state == MovementState.crouching)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step 4", gameObject);
                }
            }
        }
        // in air
        else if (!isGrounded)
            rb.AddForce(40f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);

        // Turn gravity off while on a slope
        if (!wallrunning)
        {
            rb.useGravity = !OnSlope();
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Step 3", gameObject);
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        // limit Y velocity
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        //Is not when you land but when the jump has been reset
        //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Landing", gameObject);
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
