using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumping : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] int jumps;
    [SerializeField] float jumpForce;
    [SerializeField] float airJumpForce;
    [SerializeField] LayerMask groundLayer;  // Layer for ground detection
    [SerializeField] float groundCheckDistance = 0.2f;  // Distance for ground check
    [SerializeField] float airControlFactor = 0.5f; // Air control multiplier
    [SerializeField] Transform cameraTransform; // Reference to the camera's transform
    [SerializeField] float rotationSpeed = 10f; // Speed of rotation

    private int currentJumps;
    private bool isGrounded;
    private Rigidbody rb;

    private Vector2 moveInput;

    [Header("Animator Settings")]
    public Animator animator;  // Reference to Animator to control blend tree
    public GameObject sparkL;
    public GameObject sparkR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sparkL.SetActive(false);
        sparkR.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        moveInput = callbackContext.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Jump();
            Debug.Log("Jump!");
        }
    }

    void Update()
    {
        // Check if the player is grounded
        GroundCheck();

        // Rotate the character to face the movement direction
        RotatePlayer();
    }

    void GroundCheck()
    {
        // Raycast downwards from the player's position to check for ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            if (!isGrounded) // This ensures it only triggers when landing
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
                sparkL.SetActive(false);
                sparkR.SetActive(false);
            }
            isGrounded = true;
        }
        else
        {
            animator.SetBool("isFalling", true);
            isGrounded = false;
        }
    }

    void Jump()
    {
        // If grounded, reset the jump count
        if (isGrounded)
        {
            currentJumps = jumps;
        }

        // If no jumps are left, do nothing
        if (currentJumps <= 0) return;


        // Perform the jump
        currentJumps--;

        if (currentJumps == 0)
        {
            animator.SetBool("isDJ", true);
            SoundManager.Instance.PlaySFXWithSource(15, 0.15f);
            SoundManager.Instance.PlaySFXWithSource(16, 0.05f);
            SoundManager.Instance.PlaySFXWithSource(Random.Range(11, 14), 0.5f);
        }
        else
        {
            animator.SetBool("isDJ", false);
            SoundManager.Instance.PlaySFXWithSource(15, 0.15f);
            SoundManager.Instance.PlaySFXWithSource(16, 0.05f);
            SoundManager.Instance.PlaySFXWithSource(Random.Range(7, 10), 0.5f);
        }

        // Apply the jump force
        float jumpForceToApply = isGrounded ? jumpForce : airJumpForce;

        animator.SetBool("isJumping", true);

        // Preserve the horizontal velocity and apply the jump force only to the vertical component (Y axis)
        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);  // Reset only vertical velocity
        rb.AddForce(Vector3.up * jumpForceToApply, ForceMode.Impulse);

        sparkL.SetActive(true);
        sparkR.SetActive(true);

        // Apply directional movement when in the air, allowing some control
        if (!isGrounded && moveInput.magnitude > 0)
        {
            //animator.SetBool("isFalling", true);

            // Calculate direction relative to the camera and character orientation
            Vector3 moveDirection = (cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x).normalized;

            // Apply air control in the direction of input, adjusting for air control factor
            rb.AddForce(moveDirection * airControlFactor, ForceMode.VelocityChange);
        }
    }

    void RotatePlayer()
    {
        // If there is input, rotate the player towards the movement direction
        if (moveInput.magnitude > 0)
        {
            // Get the movement direction relative to the camera
            Vector3 moveDirection = (cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x).normalized;
            moveDirection.y = 0; // Ignore vertical axis

            // Calculate the rotation we want to face
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Rotate smoothly toward the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}