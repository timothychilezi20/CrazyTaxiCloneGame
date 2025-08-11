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

    [Header("Temp test input")] [SerializeField]
    float steerinput;

    private float ackermanangleleft;

    private float ackermanangright;

    // Start is called before the first frame update
    void Start()
    {

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

    
