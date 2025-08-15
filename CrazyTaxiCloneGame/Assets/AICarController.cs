using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 10f;
    public float turnSpeed = 5f;
    public float waypointReachDistance = 2f;

    private int currentWaypointIndex = 0; 

    void Update()
    {
       // if (waypoints.Length > 0)
       // {
        //    return;
       // }

       // Vector3 targetPos = waypoints[currentWaypointIndex].position;
       // Vector3 moveDirection = (targetPos - transform.position).normalized;

        //Quaternion targetRotation = Quaternion.LookRotation(moveDirection); 
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position,  speed * Time.deltaTime);
        transform.LookAt(waypoints[0].position);    
        //if(Vector3.Distance(transform.position, targetPos) < waypointReachDistance)
       // {
        //    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        //}
    }
}
