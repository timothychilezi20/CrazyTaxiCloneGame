using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointing : MonoBehaviour
{
    public GameObject goal;
    Vector3 goalPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.LookAt(goal.transform.position);
    }
}
