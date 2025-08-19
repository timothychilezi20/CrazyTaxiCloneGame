using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelRaycast : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Suspension")]
    public float restLength=0.5f;
    public float springTravel=0.2f;
    public float springStiffness=50000f;
    public float damperStiffness=4000;
    [HideInInspector] public bool grounded;
    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springForce;
    private float damperForce;
    private float springVelocity;

    private Vector3 suspensionForce;

    [Header("Wheel")]
    public float wheelRadius=0.25f;
    public bool fright;
    public bool Bright;
    public bool fleft;
    public bool bleft;
    public float steerangle;
    private float wheelangle;
    [SerializeField] private float turnspeed=5;

    [Header("Engine Settings")]
    [SerializeField] private float maxEngineForce = 1000f;   // peak engine force
    [SerializeField] private float accelerationRate = 2f;    // throttle ramp up
    [SerializeField] private float decelerationRate = 3f;    // throttle ramp down
    [SerializeField] private float maxSpeed = 50f;           // max forward speed (m/s)

    private float throttleInput;
    private float currentThrottle;
    private float forwardSpeedBeforeCollision;

    [Header("Collision Settings")]
    [SerializeField] private float wallBounceFactor = 0.5f;   // keep 50% speed
    [SerializeField] private float carCollisionFactor = 0.7f; // keep 70% speed
    [SerializeField] private float roadFrictionFactor = 0.95f; // tiny slowdown

    // Debug
    public Vector3 localmove;
    public float fx, fy;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        // Smoothly rotate steering wheel
        wheelangle = Mathf.Lerp(wheelangle, steerangle, Time.deltaTime * turnspeed);
        transform.localRotation = Quaternion.Euler(0, wheelangle, 0);

        // Debug ray for suspension
        Debug.DrawRay(transform.position, -transform.up * (springLength + wheelRadius), Color.red);

        // Track forward speed (local z-axis = car forward direction)
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        forwardSpeedBeforeCollision = localVel.z;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            grounded = true;
            lastLength = springLength;

            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * hit.normal;

            // Throttle input ramping
            throttleInput = Input.GetAxis("Vertical"); // -1 to 1
            currentThrottle = Mathf.MoveTowards(
                currentThrottle,
                throttleInput,
                (throttleInput > currentThrottle ? accelerationRate : decelerationRate) * Time.fixedDeltaTime
            );

            // Calculate engine force
            float forwardVel = Vector3.Dot(rb.linearVelocity, transform.forward);
            if (Mathf.Abs(forwardVel) < maxSpeed)
                fx = currentThrottle * maxEngineForce;
            else
                fx = 0f;

            // Lateral (sideways) friction force
            localmove = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            fy = localmove.x * springForce;

            // Apply forces at wheel contact
            Vector3 driveForce = (fx * transform.forward) + (fy * -transform.right);
            rb.AddForceAtPosition(suspensionForce, hit.point);
            rb.AddForceAtPosition(driveForce, hit.point);
        }
        else
        {
            grounded = false;
        }
    }

    public float GetSuspensionTravel()
    {
        // Returns 0 when fully extended, 1 when fully compressed
        return 1f - ((springLength - minLength) / (maxLength - minLength));
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Road")
        {
            rb.linearVelocity *= roadFrictionFactor;
        }
        else if (tag == "Wall" || (tag == "Car"))
        {
            Vector3 vel = rb.linearVelocity;
            vel = Vector3.Reflect(vel, collision.contacts[0].normal); // bounce
            vel *= wallBounceFactor;
            rb.linearVelocity = vel;
        }
        else 
        {
            rb.linearVelocity *= carCollisionFactor;
        }
    }
}
