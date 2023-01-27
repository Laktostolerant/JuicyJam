using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sliding : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Transform playerObject;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;


    private void Start()
    {
        rb= GetComponent<Rigidbody>();
        playerMovement= GetComponent<PlayerMovement>();

        startYScale = playerObject.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if(Input.GetKeyUp(slideKey) && playerMovement.sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if(playerMovement.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        playerMovement.sliding = true;

        playerObject.localScale = new Vector3(playerObject.localScale.x, slideYScale, playerObject.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Sliding normal
        if (!playerMovement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        // Sliding down a slope
        else
        {
            rb.AddForce(playerMovement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if(slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        playerMovement.sliding = false;

        playerObject.localScale = new Vector3(playerObject.localScale.x, startYScale, playerObject.localScale.z);
    }
}
