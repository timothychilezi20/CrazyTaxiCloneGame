using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AICarFollow : MonoBehaviour
{
    public Transform target;
    public float followDistance = 5f;
    public float stopBuffer = 1f;

    private NavMeshAgent agent; 
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
    }

    void Update()
    {
        if (target == null)
        {
            return; 
        }

        Vector3 followPos = target.position - target.forward * followDistance;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > followDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(followPos);
        }
        else if (distance <= followDistance - stopBuffer)
        {
            agent.isStopped = true;
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
