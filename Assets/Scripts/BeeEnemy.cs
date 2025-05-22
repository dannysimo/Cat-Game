using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefabul pentru ac
    public Transform firePoint; // Punctul de tragere
    public float fireRate = 1.0f; // Intervalul dintre trageri
    public float projectileSpeed = 5.0f; // Viteza proiectilului
    public float tremorIntensity = 0.1f; // Intensitatea tremurului
    public float tremorSpeed = 5f; // Viteza tremurului

    private Transform player;
    private float nextFireTime = 0f;
    private Vector3 originalPosition;
    private Animator animator; // Referința la componenta Animator

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        originalPosition = transform.position;
        animator = GetComponent<Animator>(); // Inițializăm animatorul
    }

    void Update()
    {
        if (player != null)
        {
            // Calculăm direcția către jucător
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.z = 0; // Blocăm mișcarea pe axa Z

            // Flip sprite pe axa X în funcție de poziția jucătorului pe axa X
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1); // Resetăm flip-ul sprite-ului
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip sprite pe axa X
            }
        }

        // Calculăm mișcarea tremurătoare
        float tremorOffsetX = Mathf.Sin(Time.time * tremorSpeed) * tremorIntensity;
        float tremorOffsetY = Mathf.Cos(Time.time * tremorSpeed) * tremorIntensity;

        // Aplicăm mișcarea tremurătoare la poziția originală
        transform.position = originalPosition + new Vector3(tremorOffsetX, tremorOffsetY, 0);

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        if (player != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            
            Vector2 direction = (player.position - firePoint.position).normalized;

            
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        }
    }
}