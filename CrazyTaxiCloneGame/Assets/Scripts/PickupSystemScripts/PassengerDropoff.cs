using UnityEngine;

public class PassengerDropoff : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 2f;
    private float lifetime = 5f;
    private float timer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        targetPosition = transform.position + randomOffset;

        timer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
