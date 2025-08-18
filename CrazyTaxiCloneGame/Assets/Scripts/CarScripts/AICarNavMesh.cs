using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Rendering.RenderGraphModule;

public class AICarNavMesh : MonoBehaviour
{
    public Transform[] waypoints;       
    public float stopDistance = 2f;        
    public float minStopTime = 1f;         
    public float maxStopTime = 3f;
    public float slopeAlignSpeed = 5f;
    public float raycastHeight = 2f;
    public float raycastDistance = 5f;

    private NavMeshAgent agent;
    private int currentWaypoint = 0; 
    private bool isStopped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
        GoToNextWaypoint();
    }

    void Update()
    {
        if (!isStopped && !agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(WaitAndGo());
        }

        AlignToGround(); 
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0 || !agent.isOnNavMesh)
        {
            return;
        }

        agent.SetDestination(waypoints[currentWaypoint].position);
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    IEnumerator WaitAndGo()
    {
        isStopped = true;
        agent.isStopped = true;

        float waitTime = Random.Range(minStopTime, maxStopTime);
        yield return new WaitForSeconds(waitTime);

        agent.isStopped = false;
        isStopped = false;

        GoToNextWaypoint();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrafficLight"))
        {
            StartCoroutine(StopForTrafficLight());
        }
    }

    IEnumerator StopForTrafficLight()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(minStopTime, maxStopTime));
        agent.isStopped = false;
    }

    void AlignToGround()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, slopeAlignSpeed * Time.deltaTime);

            if (agent.velocity.sqrMagnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, slopeAlignSpeed * Time.deltaTime); 
            }
        }
    }
}


