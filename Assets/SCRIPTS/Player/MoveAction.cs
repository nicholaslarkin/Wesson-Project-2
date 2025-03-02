using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerPhysics;

public class MoveAction : PlayerAction
{
    [SerializeField] Transform cameraTransform;

    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float minTurnSpeed;
    [SerializeField] float maxTurnSpeed;
    [SerializeField, Range(0, 1)] float turnDeceleration;
    [SerializeField] float brakeSpeed;
    [SerializeField, Range(0, 1)] float softBrakeThreshold;
    [SerializeField] float brakeThreshold;
    [SerializeField] float brakeTime;

    bool braking;
    float brakeTimer;

    Vector2 move;

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        move = callbackContext.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        playerPhysics.onPlayerPhysicsUpdate += Move;
    }

    void OnDisable()
    {
        playerPhysics.onPlayerPhysicsUpdate -= Move;
    }

    void FixedUpdate()
    {
        RotateCharacter();
    }

    void Move()
    {
        Vector3 moveVector = GetMoveVector(cameraTransform, groundInfo.normal, move);

        bool wasBraking = braking;
        braking = groundInfo.grounded;
        braking &= playerPhysics.speed > rb.sleepThreshold;
        braking &= (braking && brakeTime > 0 && brakeTimer > 0)
            || Vector3.Dot(moveVector.normalized, playerPhysics.horizontalVelocity.normalized) < -brakeThreshold;

        if (braking)
        {
            brakeTimer -= Time.deltaTime;
        }

        if (braking && wasBraking)
        {
            brakeTimer = brakeTime;
        }

        if (braking)
        {
            Decelerate(brakeSpeed);
        }
        else if (move.magnitude > 0)
        {
            if (Vector3.Dot(moveVector.normalized, playerPhysics.horizontalVelocity.normalized) >= (groundInfo.grounded ? -softBrakeThreshold : 0))
            {
                Accelerate(acceleration);
            }
            else
            {
                Decelerate(brakeSpeed);
            }
        }
        else
        {
            Decelerate(deceleration);
        }

        // **Rotate Character Towards Movement Direction**
        //RotateCharacter();

        void Accelerate(float speed)
        {
            float maxRadDelta = Mathf.Lerp(minTurnSpeed, maxTurnSpeed, playerPhysics.speed / maxSpeed) * Mathf.PI * Time.deltaTime;
            float maxDistDelta = speed * Time.deltaTime;

            Vector3 velocity = Vector3.RotateTowards(playerPhysics.horizontalVelocity, moveVector * maxSpeed, maxRadDelta, maxDistDelta);
            velocity -= velocity * (Vector3.Angle(playerPhysics.horizontalVelocity, velocity) / 180 * turnDeceleration);

            rb.linearVelocity = velocity + playerPhysics.verticalVelocity;
        }

        void Decelerate(float speed)
        {
            rb.linearVelocity = Vector3.MoveTowards(playerPhysics.horizontalVelocity, Vector3.zero, speed * Time.deltaTime)
                + playerPhysics.verticalVelocity;
        }
    }

    void RotateCharacter()
    {
        Vector3 horizontalVelocity = playerPhysics.horizontalVelocity;
        horizontalVelocity.y = 0; // Ignore vertical movement for rotation

        if (horizontalVelocity.sqrMagnitude > 0.01f) // Ensure movement before rotating
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity, groundInfo.normal);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, maxTurnSpeed * Time.fixedDeltaTime));
        }

        /*Vector3 horizontalVelocity = playerPhysics.horizontalVelocity;
        horizontalVelocity.y = 0; // Ignore vertical movement for rotation

        if (horizontalVelocity.sqrMagnitude > 0.01f) // Ensure movement before rotating
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity, groundInfo.normal);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, maxTurnSpeed * Time.deltaTime));
        }*/
    }

    Vector3 GetMoveVector(Transform relativeTo, Vector3 upNormal, Vector2 move)
    {
        Vector3 rightNormal = Vector3.Cross(upNormal, relativeTo.forward);

        Vector3 forwardNormal = Vector3.Cross(relativeTo.right, upNormal);

        Vector3.OrthoNormalize (ref upNormal, ref forwardNormal, ref rightNormal);

        return (rightNormal * move.x) + (forwardNormal * move.y);
    }
}
