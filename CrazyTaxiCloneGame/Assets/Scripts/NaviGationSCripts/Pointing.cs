using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointing : MonoBehaviour
{
    public PickupAndDropoff pickAndDrop; 
  
    void Update()
    {
        Transform target = null; 

        if (!pickAndDrop.HasPassenger())
        {
            if (pickAndDrop.GetActivePickupZone() != null)
            {
                target = pickAndDrop.GetActivePickupZone().transform;
            }
        }

        else
        {
            if (pickAndDrop.GetActiveDropoffZone() != null)
            {
                target = pickAndDrop.GetActiveDropoffZone().transform; 
            }
        }

        if (target != null)
        {
            transform.LookAt(target.position);
        }
    }
}
