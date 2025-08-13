using UnityEngine;

public class Passenger : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float health;
    public float maxHealth = 3f;

    private Rigidbody rb;
    private Transform target;
    private Vector3 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }
}
