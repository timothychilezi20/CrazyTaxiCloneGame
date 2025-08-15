using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndDropoff : MonoBehaviour
{
    [System.Serializable]
    public class PickupZoneData
    {
        public GameObject pickupZone;
        public Passenger passengerScript; 
    }

    public List<PickupZoneData> pickupZonesData;
    public List<GameObject> dropoffZones;
    public Transform passengerHoldPoint;
    public GameObject passengerPrefab;

    private GameObject currentPassenger;
    private PickupZoneData activePickupZoneData;
    private GameObject activeDropoffZone;
    private bool hasPassenger = false;

    private int currentPickupIndex = 0;

    public GameObject pointer; 

    private void Start()
    {
        foreach (var data in pickupZonesData)
        {
            data.pickupZone.SetActive(false);

            if (data.passengerScript != null)
            {
                data.passengerScript.gameObject.SetActive(false);
            }
        }

        foreach (var dropoff in dropoffZones)
        {
            dropoff.SetActive(false);
        }

        ActivateNextPickupZone();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!hasPassenger && activePickupZoneData.pickupZone == other.gameObject)
        {
            activePickupZoneData.passengerScript.StartFollowing(passengerHoldPoint); 
        }
       
        else if (hasPassenger && activeDropoffZone == other.gameObject)
        {
            StartCoroutine(DropoffPassenger());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!hasPassenger && activePickupZoneData.pickupZone == other.gameObject)
        {
            activePickupZoneData.passengerScript.StopFollowing();
        }
    }

    void ActivateNextPickupZone()
    {
        if (pickupZonesData.Count == 0) return;

        
        if (activePickupZoneData != null)
        {
            activePickupZoneData.pickupZone.SetActive(false);

            if (activePickupZoneData.passengerScript != null)
            {
                activePickupZoneData.passengerScript.gameObject.SetActive(false);
                
            }
               
        }

        if (currentPickupIndex >= pickupZonesData.Count)
        {
            currentPickupIndex = 0;
        }
           

        activePickupZoneData = pickupZonesData[currentPickupIndex];

        activePickupZoneData.pickupZone.SetActive(true);

        if (activePickupZoneData.passengerScript != null)
        {
            activePickupZoneData.passengerScript.gameObject.SetActive(true);
        }
            
    }

    void ActivateRandomDropoffZone()
    {
        if (dropoffZones.Count == 0) return;

        activeDropoffZone = dropoffZones[Random.Range(0, dropoffZones.Count)];
        activeDropoffZone.SetActive(true);
    }

    public void AttachPassenger()
    {
        activePickupZoneData.passengerScript.gameObject.SetActive(false);
        currentPassenger = Instantiate(passengerPrefab, passengerHoldPoint.position, passengerHoldPoint.rotation);
        currentPassenger.transform.SetParent(passengerHoldPoint);

        hasPassenger = true;
        activePickupZoneData.pickupZone.SetActive(false);
        ActivateRandomDropoffZone();
    }

    IEnumerator DropoffPassenger()
    {
        yield return new WaitForSeconds(2f);

        Instantiate(passengerPrefab, activeDropoffZone.transform.position, Quaternion.identity);

        Destroy(currentPassenger);
        hasPassenger = false;

        activeDropoffZone.SetActive(false);

        currentPickupIndex++;
        ActivateNextPickupZone();
    }

    public bool HasPassenger()
    {
        return hasPassenger;
    }

    public GameObject GetActivePickupZone()
    {
        return activePickupZoneData != null ? activePickupZoneData.pickupZone : null;
    }

    public GameObject GetActiveDropoffZone()
    {
        return activeDropoffZone; 
    }
}
