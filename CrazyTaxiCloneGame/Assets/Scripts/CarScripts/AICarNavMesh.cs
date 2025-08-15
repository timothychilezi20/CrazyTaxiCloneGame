using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AICarNavMesh : MonoBehaviour
{
    public Transform[] destinations;       
    public float stopDistance = 2f;        
    public float minStopTime = 1f;         
    public float maxStopTime = 3f;         

    private NavMeshAgent agent;
    private bool isStopped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PickRandomDestination();
    }

    void Update()
    {
        if (!isStopped && !agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            StartCoroutine(WaitAndGo());
        }
    }

    void PickRandomDestination()
    {
        if (destinations.Length == 0) return;
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
}


