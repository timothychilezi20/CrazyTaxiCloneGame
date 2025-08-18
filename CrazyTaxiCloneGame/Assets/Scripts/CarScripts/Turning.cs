using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turning : MonoBehaviour
{
    [Header("car specs")] public WheelRaycast[] wheels;
   float wheelbase;
    float rearTrack;
    [SerializeField] float Turnradius;

    [Header("Temp test input")] [SerializeField]
    float steerinput;

    private float ackermanangleleft;

    private float ackermanangright;

    // Start is called before the first frame update
    void Start()
    {
        CalculateWheelbaseAndRearTrack();
    }
    void CalculateWheelbaseAndRearTrack()
    {
        Vector3 frontPos = Vector3.zero;
        Vector3 rearLeftPos = Vector3.zero;
        Vector3 rearRightPos = Vector3.zero;

        foreach (WheelRaycast w in wheels)
        {
            if (w.fleft || w.fright)
                frontPos = w.transform.localPosition;
            if (w.bleft)
                rearLeftPos = w.transform.localPosition;
            if (w.Bright)
                rearRightPos = w.transform.localPosition;
        }

        wheelbase = Mathf.Abs(frontPos.z - rearLeftPos.z); // front to rear
        rearTrack = Mathf.Abs(rearRightPos.x - rearLeftPos.x); // side to side

        Debug.Log("Calculated Wheelbase: " + wheelbase);
        Debug.Log("Calculated RearTrack: " + rearTrack);
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

    
