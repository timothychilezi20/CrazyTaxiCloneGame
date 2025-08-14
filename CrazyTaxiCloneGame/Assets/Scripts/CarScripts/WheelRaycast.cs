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
    [SerializeField] private float turnspeed;

    [Header("Move Forces")]
    public Vector3 localmove;
    public float fx, fy;
    public float driveInput;
    [SerializeField] private float accelMultiplier = 3f;
    [HideInInspector] public bool isGrounded;    // Wheel contact state
    [HideInInspector] public Vector3 lastHitNormal; // For slope info
    [SerializeField] float maxSpeed = 50f;
    public float movespeed;

    [Header("Airborne Assist (world-down probe)")]
    [Tooltip("How far to look straight down (world) when wheel is not grounded.")]
    [SerializeField] private float airProbeLength = 12f;

    [Tooltip("Begin applying pre-landing force when within this distance to ground.")]
    [SerializeField] private float airAssistStart = 4f;

    [Tooltip("Strength of pre-landing catch force.")]
    [SerializeField] private float airAssistForce = 9000f;

    [Tooltip("Extra damping against downward velocity while in air and near ground.")]
    [SerializeField, Range(0f, 1f)] private float airAssistDamping = 0.25f;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        // Visual steer
        wheelangle = Mathf.Lerp(wheelangle, steerangle, Time.deltaTime * turnspeed);
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.x,
            transform.localRotation.y + wheelangle,
            transform.localRotation.z
        );

        // Debug main suspension ray (local down)
        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.red);

        // Debug world-down probe (always draw so you can see it in Scene)
        Debug.DrawRay(transform.position, Vector3.down * airProbeLength, Color.cyan);
    }

    void FixedUpdate()
    {
        // ===== 1) MAIN SUSPENSION RAY (local -up) =====
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

            // Suspension along ground normal
            suspensionForce = (springForce + damperForce) * hit.normal;

            // Local velocity at wheel contact
            localmove = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            // Driving (FWD only, as you had it) — independent of spring force
            if (fleft || fright)
                fx = driveInput * accelMultiplier;
            else
                fx = 0f;

            // Simple lateral friction proportional to lateral speed & spring force (your original)
            fy = localmove.x * springForce;

            // Project forward along surface to avoid pushing into slopes
            Vector3 forwardOnSurface = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;

            rb.AddForceAtPosition(
                suspensionForce + (fx * forwardOnSurface) + (fy * -transform.right),
                hit.point
            );
        }
        else
        {
            // ===== 2) AIRBORNE STATE =====
            isGrounded = false;
            lastHitNormal = transform.up;

            // EXTRA: World-down probe to anticipate ground and apply pre-landing force
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit downHit, airProbeLength))
            {
                // Only start assisting within airAssistStart meters from ground
                if (downHit.distance <= airAssistStart)
                {
                    // Ramp 0..1 as we approach ground (closer = stronger)
                    float t = Mathf.InverseLerp(airAssistStart, 0f, downHit.distance);

                    // Upward force along the ground normal (feels better on slopes)
                    Vector3 upAssist = downHit.normal * (airAssistForce * t);

                    // Damping against downward velocity (reduces slam)
                    float vRel = Vector3.Dot(rb.GetPointVelocity(transform.position), -downHit.normal); // >0 when moving down
                    if (vRel > 0f)
                    {
                        upAssist += downHit.normal * (vRel * airAssistForce * airAssistDamping * Time.fixedDeltaTime);
                    }

                    // Apply at wheel position for stability
                    rb.AddForceAtPosition(upAssist, transform.position, ForceMode.Force);
                }
            }
            else
            {
                // Optional tiny downward bias so the car doesn't float in rare cases
                rb.AddForce(Vector3.down * 300f, ForceMode.Force);
            }
        }
    }
}
