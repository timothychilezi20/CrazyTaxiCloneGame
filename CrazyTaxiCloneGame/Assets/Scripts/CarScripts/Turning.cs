using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turning : MonoBehaviour
{
    [Header("car specs")] public WheelRaycast[] wheels;
    [SerializeField] float wheelbase;
    [SerializeField] float rearTrack;
    [SerializeField] float Turnradius;
    [SerializeField] private Rigidbody carBody;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float uprightStrength;
    [SerializeField] private float uprightDamping;

    [Header("Temp test input")] [SerializeField]
    float steerinput;

    private float ackermanangleleft;

    private float ackermanangright;

    // Start is called before the first frame update
    void Start()
    {

    }
    // void FixedUpdate()
    // {
    //     
    //
    //     
    //     Vector3 currentUp = transform.up;
    //     Vector3 worldUp = Vector3.up;
    //
    //     // Axis around which we need to rotate to match world up
    //     Vector3 tiltAxis = Vector3.Cross(currentUp, worldUp);
    //
    //     // Torque proportional to angle difference
    //     Vector3 stabilizationTorque = tiltAxis * uprightStrength;
    //
    //     // Add some damping to prevent wobble
    //     stabilizationTorque -= rb.angularVelocity * uprightDamping;
    //
    //     rb.AddTorque(stabilizationTorque, ForceMode.Acceleration);
    // }
    void FixedUpdate()
    {
        Vector3 totalNormal  = Vector3.zero;
        int groundedCount = 0;

        foreach (WheelRaycast w in wheels)
        {
            if (w.isGrounded)
            {
                totalNormal  += w.lastHitNormal;
                groundedCount++;
            }
        }
        Vector3 avgNormal = transform.up; 
        if (groundedCount > 0)
        {
            avgNormal.Normalize();

           
            Quaternion targetRotation = Quaternion.FromToRotation(carBody.transform.up, avgNormal) * carBody.rotation;
            carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed));
        }
        Vector3 currentUp = transform.up; 
        Vector3 worldUp = Vector3.up;
            Vector3 tiltAxis = Vector3.Cross(currentUp, worldUp);
        
          
            Vector3 stabilizationTorque = tiltAxis * uprightStrength;
        
           
            stabilizationTorque -= carBody.angularVelocity * uprightDamping;
        
            carBody.AddTorque(stabilizationTorque, ForceMode.Acceleration);
        }


    // Update is called once per frame
    void Update()
    {
        steerinput = Input.GetAxis("Horizontal");
        if (steerinput > 0)
        {
            ackermanangleleft = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (Turnradius + (rearTrack / 2))) * steerinput;
            ackermanangright = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (Turnradius - (rearTrack / 2))) * steerinput;
        }
        else if (steerinput < 0)
        {
            ackermanangleleft = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (Turnradius - (rearTrack / 2))) * steerinput;
            ackermanangright = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (Turnradius + (rearTrack / 2))) * steerinput;
        }
        else
        {
            ackermanangleleft = 0;
            ackermanangright = 0;

        }


        foreach   (WheelRaycast w in wheels)
        {
            if (w.fleft)
            {
                w.steerangle = ackermanangleleft;
            }

            if (w.fright)
            {
                w.steerangle = ackermanangright;
            }
        }
    }
}

    
