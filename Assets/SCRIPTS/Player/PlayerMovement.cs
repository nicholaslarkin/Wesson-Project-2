using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Physics Settings")]
    public float acceleration = 20f;
    public float deceleration = 5f;  // Deceleration variable
    public float maxSpeed = 10f;
    public float airControl = 0.5f;
    public float gravityMultiplier = 2f;
    public float groundCheckDistance = 0.2f;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;
    public LayerMask groundLayer;

    [Header("Camera Settings")]
    public Transform playerCamera;  // Reference to the camera

    [Header("Animator Settings")]
    public Animator animator;  // Reference to Animator to control blend tree

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 groundNormal;
    private Vector2 move;  // Updated to use Vector2 for Input System

    private float currentSpeed;  // Store current speed to pass to blend tree

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // We handle gravity manually

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Input is now handled by the OnMove method, no need to do anything here.

        // Update the speed value for the Animator
        UpdateSpeed();
    }

    void FixedUpdate()
    {
        GroundCheck();
        ApplyMovement();
        ApplyGravity();
    }

    void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
    }

    void ApplyMovement()
    {
        // Calculate the move direction relative to the camera
        Vector3 cameraForward = playerCamera.forward;
        Vector3 cameraRight = playerCamera.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Determine the intended move direction based on input and camera direction
        Vector3 moveDirection = (cameraForward * move.y + cameraRight * move.x).normalized;

        if (isGrounded)
        {
            // Apply movement force on the ground
            rb.AddForce(moveDirection * acceleration, ForceMode.Acceleration);

            // Apply friction when no input
            if (move.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
            }

            // Rotate towards the movement direction
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            }
        }
        else
        {
            // Apply air control using the same camera-relative move direction
            rb.AddForce(moveDirection * acceleration * airControl, ForceMode.Acceleration);
        }

        // Limit max speed
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    // Input System method for handling movement input
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        move = callbackContext.ReadValue<Vector2>();
    }

    // Calculate and update the current speed based on horizontal velocity
    void UpdateSpeed()
    {
        // Calculate the horizontal speed (ignoring the Y-axis for vertical movement)
        currentSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        // Pass the speed to the Animator (if it's assigned)
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);  // Set this value in the blend tree
        }
    }
}

