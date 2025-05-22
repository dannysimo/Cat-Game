using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    public float distanceThreshold = 0.1f;
    public int damage = 1; // Damage pe care îl provoacă inamicul

    private Transform targetPoint;

    void Start()
    {
        // Setăm punctul țintă inițial la pointA
        targetPoint = pointA;
    }

    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, targetPoint.position) < distanceThreshold)
        {
            targetPoint = targetPoint == pointA ? pointB : pointA;
        }

        // Schimbăm direcția în care privește sprite-ul inamicului
        Vector3 scale = transform.localScale;
        if (targetPoint.position.x < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x); // Privește la stânga
        }
        else
        {
            scale.x = Mathf.Abs(scale.x); // Privește la dreapta
        }
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}