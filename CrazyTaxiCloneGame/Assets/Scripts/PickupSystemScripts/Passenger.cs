using UnityEngine;

public class Passenger : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.5f;

    private Transform target;
    private bool isFollowing = false;
    private PickupAndDropoff controller;

    public void StartFollowing(Transform followTarget)
    {
        target = followTarget;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    private void Update()
    {
        if (!isFollowing || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stoppingDistance)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
        else
        {
            isFollowing = false;
            controller = Object.FindFirstObjectByType<PickupAndDropoff>();
            controller.AttachPassenger();
        }
    }
}
