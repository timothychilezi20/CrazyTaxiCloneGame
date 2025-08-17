using UnityEngine;
using UnityEngine.AI;

public class CarAlignToSlope : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, slopeRotation, Time.deltaTime * 5f);
        }

        if (agent.desiredVelocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.desiredVelocity.normalized, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
