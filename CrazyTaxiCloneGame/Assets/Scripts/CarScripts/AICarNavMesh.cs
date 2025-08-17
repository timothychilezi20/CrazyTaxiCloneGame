using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AICarNavMesh : MonoBehaviour
{
    public Transform[] destinations;       
    public float stopDistance = 2f;        
    public float minStopTime = 1f;         
    public float maxStopTime = 3f;
    public float slopeAlignSpeed = 5f;
    public float raycastHeight = 2f;
    public float raycastDistance = 5f;

    private NavMeshAgent agent;
    private bool isStopped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
        PickRandomDestination();
    }

    void Update()
    {
        if (!isStopped && !agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(WaitAndGo());
        }

        AlignToGround(); 
    }

    void PickRandomDestination()
    {
        if (destinations.Length == 0)
        {
            return;
        }

        if (!agent.isOnNavMesh)
        {
            return;
        }
        int randomIndex = Random.Range(0, destinations.Length);
        agent.SetDestination(destinations[randomIndex].position);
    }

    IEnumerator WaitAndGo()
    {
        isStopped = true;
        agent.isStopped = true;

        float waitTime = Random.Range(minStopTime, maxStopTime);
        yield return new WaitForSeconds(waitTime);

        agent.isStopped = false;
        isStopped = false;

        PickRandomDestination();
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


