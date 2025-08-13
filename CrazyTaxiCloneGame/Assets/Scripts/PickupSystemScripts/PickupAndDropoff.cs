using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndDropoff : MonoBehaviour
{
    [System.Serializable]
    public class PickupZoneData
    {
        public GameObject pickupZone;
        public GameObject passengerModel;  
    }

    public List<PickupZoneData> pickupZonesData;
    public List<GameObject> dropoffZones;
    public Transform passengerHoldPoint;

    public GameObject passengerPrefab;  

    private GameObject currentPassenger;
    private PickupZoneData activePickupZoneData;
    private GameObject activeDropoffZone;
    private bool hasPassenger = false;


    private void Start()
    {
        foreach (var data in pickupZonesData)
        {
            data.pickupZone.SetActive(false);
            data.passengerModel.SetActive(false);
        }
        foreach (var dropoff in dropoffZones)
        {
            dropoff.SetActive(false);
        }

        ActivateRandomPickupZone();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPassenger && activePickupZoneData.pickupZone == other.gameObject)
        {
            PickupPassenger();
        }
        else if (hasPassenger && activeDropoffZone == other.gameObject)
        {
            DropoffPassenger();
        }
    }

    void ActivateRandomPickupZone()
    {
        if (pickupZonesData.Count == 0) return;

        if (activePickupZoneData != null)
        {
            activePickupZoneData.pickupZone.SetActive(false);
            activePickupZoneData.passengerModel.SetActive(false);
        }

        activePickupZoneData = pickupZonesData[Random.Range(0, pickupZonesData.Count)];
        activePickupZoneData.pickupZone.SetActive(true);
        activePickupZoneData.passengerModel.SetActive(true);
    }

    void ActivateRandomDropoffZone()
    {
        if (dropoffZones.Count == 0) return;

        activeDropoffZone = dropoffZones[Random.Range(0, dropoffZones.Count)];
        activeDropoffZone.SetActive(true);
    }

    void PickupPassenger()
    {
        activePickupZoneData.passengerModel.SetActive(false);

        currentPassenger = Instantiate(passengerPrefab, passengerHoldPoint.position, passengerHoldPoint.rotation);
        currentPassenger.transform.SetParent(passengerHoldPoint);

        hasPassenger = true;

        activePickupZoneData.pickupZone.SetActive(false);
        ActivateRandomDropoffZone();
    }

    void DropoffPassenger()
    {
        Vector3 dropPos = activeDropoffZone.transform.position;
        Quaternion dropRot = Quaternion.identity; // Or activeDropoffZone.transform.rotation

        GameObject dropoffPassenger = Instantiate(passengerPrefab, dropPos, dropRot);

        Destroy(currentPassenger);
        hasPassenger = false;

        activeDropoffZone.SetActive(false);

        ActivateRandomPickupZone();
    }

}

