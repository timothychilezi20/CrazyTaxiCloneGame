using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [HideInInspector] public Vector3 avgNormal; 
    [HideInInspector] public bool isGrounded;  
    [HideInInspector] public Vector3 lastHitNormal; 


    
    
    [Header ("Move Forward Shit")]
    public Vector3 localmove;

    public float fx, fy;
    
    void Start() {
        rb = transform.parent.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    private void Update()
    {
        wheelangle=Mathf.Lerp(wheelangle,steerangle,Time.deltaTime * turnspeed);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x,transform.localRotation.y+ wheelangle,transform.localRotation.z);
        Debug.DrawRay(transform.position,-transform.up*(springLength+wheelRadius),Color.red);
    }

    void FixedUpdate() {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius)) {
            isGrounded = true;
            lastHitNormal = hit.normal; 
            
            lastLength = springLength;

            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * avgNormal;

            localmove =transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            fx=Input.GetAxis("Vertical")*springForce;
            fy = localmove.x * springForce;
            rb.AddForceAtPosition(suspensionForce+(fx * Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized
                )+(fy*-transform.right), hit.point);
            
        }
        else
        {
            isGrounded = false;
            lastHitNormal = transform.up; 
        }

    }
}

