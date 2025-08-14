using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turning : MonoBehaviour
{
    [Header("Car Specs")]
    public WheelRaycast[] wheels;
    [SerializeField] float wheelbase;
    [SerializeField] float rearTrack;
    [SerializeField] float Turnradius;
    public Rigidbody rb;
    private float clamp = 15f; // pitch/roll clamp

    [Header("Temp test input")]
    [SerializeField] float steerinput;

    private float ackermanangleleft;
    private float ackermanangright;

    void FixedUpdate()
    {
        // Count how many wheels are grounded
        int groundedCount = 0;
        foreach (WheelRaycast w in wheels)
        {
            if (w.isGrounded) groundedCount++;
        }

        // If airborne, auto-level chassis
        if (groundedCount == 0)
        {
            // 1️⃣ Keep car parallel to horizontal plane
            Quaternion snapRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            rb.MoveRotation(snapRotation);

// Stop all mid-air rotation
            rb.angularVelocity = Vector3.zero;

            // 2️⃣ Raycast downward to measure ground distance
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 50f))
            {
                float distToGround = hit.distance;

                // Optional: apply "catch" force as you get close to ground
                if (distToGround < 5f) // within 5 meters of landing
                {
                    float landingForce = Mathf.Lerp(0, 8000f, 1 - (distToGround / 5f));
                    rb.AddForce(Vector3.up * landingForce, ForceMode.Force);
                }
            }
        }
    }


    [SerializeField] private float turnTorque = 500f;

    void Update()
    {
        float steerInput = Input.GetAxis("Horizontal");
        float accelInput = Input.GetAxis("Vertical");

        // Apply drive to front wheels
        foreach (WheelRaycast w in wheels)
        {
            if (w.fleft || w.fright)
            {
                w.driveInput = accelInput; // Pass input to wheels
            }
        }

        // Apply rotation to the Rigidbody for steering
        if (Mathf.Abs(steerInput) > 0.01f)
        {
            rb.AddTorque(Vector3.up * steerInput * turnTorque, ForceMode.Force);
        }
    }

}
