using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAction : PlayerAction
{
    [SerializeField] int jumps;
    [SerializeField] float jumpForce;
    [SerializeField] float airJumpForce;

    int currentJumps;

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            Jump();
            Debug.Log("Jump!");
        }
    }

    void OnEnable()
    {
        playerPhysics.onGroundEnter += OnGroundEnter;
    }

    void OnDisable()
    {
        playerPhysics.onGroundEnter -= OnGroundEnter;
    }

    void OnGroundEnter()
    {
        currentJumps = jumps;
    }

    void Jump()
    {
        if (groundInfo.grounded)
        {
            currentJumps = jumps;
        }

        if (currentJumps <= 0) return;

        currentJumps--;

        float jumpForce = groundInfo.grounded ? this.jumpForce : airJumpForce;

        rb.linearVelocity = (groundInfo.normal * jumpForce)
            + playerPhysics.horizontalVelocity;
    }
}
