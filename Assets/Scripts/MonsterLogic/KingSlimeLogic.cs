using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    public int health = 100;
    public Transform target;
    public float speed = 3f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;

    public float stoppingDistance = 0f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetTarget(); // Initialize the target
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(target.position, transform.position);

            if (distanceToTarget > stoppingDistance)
            {
                // Calculate direction to the player
                Vector2 direction = (target.position - transform.position).normalized;

                // Move
                rb.velocity = direction * speed;
            }
            else
            {
                // Stop the boss when
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget(); // Continuously look for the player
        }

        if (target != null && Vector2.Distance(target.position, transform.position) <= stoppingDistance)
        {
            Attack();
        }
    }

    private void GetTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            target = player.transform;
        }
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Boss attacks the player!");
            lastAttackTime = Time.time;

            // Implement attack logic here (e.g., instantiate bullets or deal damage to player)
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject); //killing the player
            target = null;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Damage(10); // Take damage when hit by a bullet
            Destroy(other.gameObject);
        }
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject); // Destroy the boss
    }
}
