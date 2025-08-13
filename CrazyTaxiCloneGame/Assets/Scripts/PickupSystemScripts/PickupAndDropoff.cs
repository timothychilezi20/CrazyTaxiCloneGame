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

    private int currentPickupIndex = 0;

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

        currentPickupIndex = 0;
        ActivateNextPickupZone();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPassenger && activePickupZoneData.pickupZone == other.gameObject)
        {
            StartCoroutine(PickupPassenger());
        }
        else if (hasPassenger && activeDropoffZone == other.gameObject)
        {
            StartCoroutine(DropoffPassenger());
        }
    }

    void ActivateNextPickupZone()
    {
        if (pickupZonesData.Count == 0) return;

        if (activePickupZoneData != null)
        {
            activePickupZoneData.pickupZone.SetActive(false);
            activePickupZoneData.passengerModel.SetActive(false);
        }

        if (currentPickupIndex >= pickupZonesData.Count)
        {
            currentPickupIndex = 0; 
        }

        activePickupZoneData = pickupZonesData[currentPickupIndex];
        activePickupZoneData.pickupZone.SetActive(true);
        activePickupZoneData.passengerModel.SetActive(true);
    }

    void ActivateRandomDropoffZone()
    {
        if (dropoffZones.Count == 0) return;

        activeDropoffZone = dropoffZones[Random.Range(0, dropoffZones.Count)];
        activeDropoffZone.SetActive(true);
    }

    IEnumerator PickupPassenger()
    {
        yield return new WaitForSeconds(5f); 
        activePickupZoneData.passengerModel.SetActive(false);

        currentPassenger = Instantiate(passengerPrefab, passengerHoldPoint.position, passengerHoldPoint.rotation);
        currentPassenger.transform.SetParent(passengerHoldPoint);

        hasPassenger = true;

        activePickupZoneData.pickupZone.SetActive(false);
        ActivateRandomDropoffZone();
    }

    IEnumerator DropoffPassenger()
    {
        yield return new WaitForSeconds(5f);
        Vector3 dropPos = activeDropoffZone.transform.position;
        Quaternion dropRot = Quaternion.identity; 

        GameObject dropoffPassenger = Instantiate(passengerPrefab, dropPos, dropRot);

        Destroy(currentPassenger);
        hasPassenger = false;

        activeDropoffZone.SetActive(false);

        currentPickupIndex++;    
        ActivateNextPickupZone();
    }
}
