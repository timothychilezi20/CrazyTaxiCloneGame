using UnityEngine;
using System;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    
    public WheelCollider[] wheel_col;
    public Transform[] wheels;
    public float torque=100;
    public float angle=45;

    private void FixedUpdate()
    {
       
        for(int i=0;i<wheel_col.Length;i++)
        {
            wheel_col[i].motorTorque=Input.GetAxis("Vertical")*torque;
            if(i==0||i==2)
            {
                wheel_col[i].steerAngle=Input.GetAxis("Horizontal")*angle;
            }
            var pos=transform.position;
            var rot=transform.rotation;
            wheel_col[i].GetWorldPose(out pos,out rot);
            wheels[i].position=pos;
            wheels[i].rotation=rot;
            
        }
        if(Input.anyKeyDown) 
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                foreach(var i in wheel_col)
                {
                    i.brakeTorque=2000;
                }
            }
            else{   //reset the brake torque when another key is pressed
                foreach(var i in wheel_col)
                {
                    i.brakeTorque=0;
                }
                
            }
        }
        
       
        
    }
}