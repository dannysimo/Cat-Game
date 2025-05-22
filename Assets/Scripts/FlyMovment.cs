using System;
using UnityEngine;

public class FlyEnemy : MonoBehaviour, IDamageable
{
    public event Action OnDeath; // Eventul pentru notificarea morții

    public float speed = 2f;
    public float detectionRange = 5f;
    public float wobbleIntensity = 0.5f;
    public float wobbleSpeed = 2f;
    public int damage = 1;
    public float knockbackForce = 2f; // Forța de recul

    private Transform player;
    private Vector3 originalPosition;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPosition = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            FlyTowardsPlayer();
        }
        else
        {
            FlyRandomly();
        }
    }

    void FlyTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 wobble = new Vector3(
            Mathf.Sin(Time.time * wobbleSpeed) * wobbleIntensity,
            Mathf.Cos(Time.time * wobbleSpeed) * wobbleIntensity,
            0
        );
        transform.position += (direction + wobble) * speed * Time.deltaTime;
    }

    void FlyRandomly()
    {
        Vector3 randomDirection = new Vector3(
            Mathf.PerlinNoise(Time.time, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time) - 0.5f,
            0
        ).normalized;
        Vector3 wobble = new Vector3(
            Mathf.Sin(Time.time * wobbleSpeed) * wobbleIntensity,
            Mathf.Cos(Time.time * wobbleSpeed) * wobbleIntensity,
            0
        );
        transform.position += (randomDirection + wobble) * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    public void TakeDamage(int amount)
    {
        animator.SetTrigger("TakeDamage");
        ApplyKnockback();
    }

    public void Die()
    {
        OnDeath?.Invoke(); // Declanșarea evenimentului

        animator.SetTrigger("Die");
        Destroy(gameObject, 0.70f);
    }

    private void ApplyKnockback()
    {
        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}