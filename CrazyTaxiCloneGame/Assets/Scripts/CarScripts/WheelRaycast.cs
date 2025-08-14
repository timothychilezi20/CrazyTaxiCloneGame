using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRaycast : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springForce;
    private float damperForce;
    private float springVelocity;

    private Vector3 suspensionForce;

    [Header("Wheel")]
    public float wheelRadius;

    public bool fright;
    public bool Bright;
    public bool fleft;
    public bool bleft;
    public float steerangle;
    private float wheelangle;
    public float movespeed;
    [SerializeField] private float turnspeed;

    [Header("Move Forces")]
    public Vector3 localmove;
    public float fx, fy;
     public float driveInput;
     [SerializeField] private float accelMultiplier = 3f;
    [HideInInspector] public bool isGrounded;    // Wheel contact state
    [HideInInspector] public Vector3 lastHitNormal; // For slope info
    [SerializeField] float maxSpeed = 50f;
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        wheelangle = Mathf.Lerp(wheelangle, steerangle, Time.deltaTime * turnspeed);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x,
            transform.localRotation.y + wheelangle,
            transform.localRotation.z);

        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.red);
    }

    void FixedUpdate()
    {
        // Main suspension ray
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            isGrounded = true;
            lastHitNormal = hit.normal;

            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            // Suspension force along ground normal
            suspensionForce = (springForce + damperForce) * hit.normal;


            // Local velocity at wheel contact
            localmove = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            // Driving and lateral forces
            if (fleft || fright)
            {
                fx = movespeed *  accelMultiplier ; // Drive only on front wheels
            }
            else
            {
                fx = 0f; // No drive on rear wheels
            }

            fy = localmove.x * springForce;

            // Project forward direction along surface
            Vector3 forwardOnSurface = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;

            rb.AddForceAtPosition(
                suspensionForce + (fx * forwardOnSurface) + (fy * -transform.right),
                hit.point
            );
        }
        else
        {
            isGrounded = false;
            lastHitNormal = transform.up;

            // Backup raycast to keep car from floating away
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit backupHit,
                (maxLength + wheelRadius) * 3))
            {
                rb.AddForce(Vector3.down * 300f, ForceMode.Force);
            }
        }
    }
}
