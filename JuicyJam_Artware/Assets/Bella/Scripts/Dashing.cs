using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform cam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCooldownTimer;

    [Header("Camera Effects")]
    public CameraController camController;
    public float dashFOV;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVelocity = true;

    private void Start()
    {
        rb= GetComponent<Rigidbody>();
        pm= GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dash();

        if(dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Dash", gameObject);

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        camController.DoFov(dashFOV);

        Transform forwardT;

        if (useCameraForward)
            forwardT = cam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        deleyedForceToApply = forceToApply;
        Invoke(nameof(DeleyedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 deleyedForceToApply;

    private void DeleyedDashForce()
    {
        if (resetVelocity)
            rb.velocity = Vector3.zero;

        rb.AddForce(deleyedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0;

        camController.DoFov(80f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        // Getting Inputs
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if(allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
