using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AICarFollow : MonoBehaviour
{
    public Transform target;
    public float followDistance = 5f;
    public float stopBuffer = 1f;

    public float bumpForce = 15f;
    public float bumpUpward = 2f;
    public float recoveryDelay = 2f; 

    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isBumped = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        agent.updateRotation = false; 
    }

    void Update()
    {
        if (isBumped || target == null)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBumped)
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            Vector3 force = dir * bumpForce + Vector3.up * bumpUpward;

            StartCoroutine(ApplyImpact(force));
        }
    }

    private System.Collections.IEnumerator ApplyImpact(Vector3 force)
    {
        isBumped = true;

        agent.enabled = false;
        rb.isKinematic = false;

        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * 5f, ForceMode.Impulse);

        yield return new WaitForSeconds(recoveryDelay);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        agent.enabled = true;

        isBumped = false;
    }
}
