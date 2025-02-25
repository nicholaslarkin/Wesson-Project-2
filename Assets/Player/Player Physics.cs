using System;
using System.Collections;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public Rigidbody rb;
    public LayerMask Ground;

    [Header("Moving")]
    public Vector3 horizontalVelocity => Vector3.ProjectOnPlane(rb.linearVelocity, rb.transform.up); //prevents losing horizontal momentum
    public Vector3 verticalVelocity => Vector3.Project(rb.linearVelocity, rb.transform.up); //allows movement in the air
    public float verticalSpeed => Vector3.Dot(rb.linearVelocity, rb.transform.up);

    public float speed => horizontalVelocity.magnitude;

    public Action onPlayerPhysicsUpdate;


    [Header("Gravity")]
    [SerializeField] float gravity; //gravity of the player

    [Header("GroundCheck")]
    [SerializeField] float groundDistance; //raycast for ground check

    public struct GroundInfo
    {
        public bool grounded; //states whether the raycast has detected ground or not
        public Vector3 normal; //normal of the ground for slopes
        public Vector3 point; //for anchoring w/ collider; helps with stairs
    }

    [HideInInspector] public GroundInfo groundInfo;

    public Action onGroundEnter;
    public Action onGroundExit;

    #region FixedUpdate
    void FixedUpdate()
    {
        onPlayerPhysicsUpdate?.Invoke();

        if (!groundInfo.grounded)
        {
            Gravity();
        }

        if (groundInfo.grounded && verticalSpeed < rb.sleepThreshold)
        {
            rb.linearVelocity = horizontalVelocity;
        }

        StartCoroutine(LateFixedUpdateRoutine());  

        IEnumerator LateFixedUpdateRoutine()
        {
            yield return new WaitForFixedUpdate();

            LateFixedUpdate();
        }
    }
    #endregion

    #region Gravity
    void Gravity()
    {
        rb.linearVelocity -= Vector3.up * gravity * Time.deltaTime;
    }
    #endregion

    #region LateFixedUpdate
    void LateFixedUpdate()
    {
        GroundCheck();
        Snap();

        if (groundInfo.grounded)
        {
            rb.linearVelocity = horizontalVelocity;
        }
    }
    #endregion

    #region GroundCheck
    void GroundCheck()
    {
        float maxDistance = Mathf.Max(rb.centerOfMass.y, 0) + (rb.sleepThreshold * Time.fixedDeltaTime);

        if (groundInfo.grounded && verticalSpeed < rb.sleepThreshold)
        {
            maxDistance += groundDistance;
        }

        bool grounded = Physics.Raycast(rb.worldCenterOfMass, -rb.transform.up, out RaycastHit hit, maxDistance, Ground, QueryTriggerInteraction.Ignore);

        Vector3 point = grounded ? hit.point : rb.transform.position;

        Vector3 normal = grounded ? hit.normal : Vector3.up;

        if(grounded != groundInfo.grounded)
        {
            if (grounded)
            {
                onGroundEnter?.Invoke();
            }
            else
            {
                onGroundExit?.Invoke();
            }
        }

        groundInfo = new()
        {
            point = point,
            normal = normal,
            grounded = grounded,
        };
    }
    #endregion

    #region Snap
    void Snap()
    {
        rb.transform.up = groundInfo.normal;

        Vector3 goal = groundInfo.point;

        Vector3 difference = goal - rb.transform.position;

        if (rb.SweepTest(difference, out _, difference.magnitude, QueryTriggerInteraction.Ignore))
        {
            return;
        }

        rb.transform.position = goal;
    }
    #endregion
}
