using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrostGuardian : MonoBehaviour, IDamageable
{
    Transform TargetPos;
    [SerializeField] private int health;
    private Vector3 direction;
    private RaycastHit2D ray;
    SpriteRenderer bodySprite;
    [SerializeField] float rayDistance = 30;
    Animator animator;
    [SerializeField] float Speed = 5f;
    public bool isRunning = true;
    private float lastDamageTime = 0f;
    private float damageCooldown = 1f;
    private Rigidbody2D rb;
    [SerializeField] private int damage = 20;

    [SerializeField] private GameObject instructorPrefab;
    [SerializeField] private GameObject healthItemPrefab;
    [SerializeField] private GameObject coinPrefab;
    private bool isDead = false;
    public GameObject bulletPrefab;
    public event System.Action OnDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodySprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return; // Skip all behavior if dead
        GetTargetPos();
        SetSpriteFlip();
        GetDirection();
        Run();
        Attack();
        Debug.DrawLine(transform.position, transform.position + direction * rayDistance, Color.red);
        ray = Physics2D.Raycast(transform.position, direction, rayDistance);
    }

    void GetTargetPos()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            TargetPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    ,
    void GetDirection()
    {
        if (TargetPos != null)
        {
            direction = (TargetPos.position - transform.position).normalized;
            Debug.Log($"Direction towards player: {direction}");
        }
    }

    void SetSpriteFlip()
    {
        if (TargetPos.position.x - transform.position.x < 0)
        {
            bodySprite.flipX = false;
        }
        else if (TargetPos.position.x - transform.position.x > 0)
        {
            bodySprite.flipX = true;
        }

    }

    void Attack()
    {
        if (ray.collider != null && ray.collider.CompareTag("Player"))
        {
            isRunning = false;
            animator.SetTrigger("Attack");
        }
        else
        {
            isRunning = true;
            animator.SetTrigger("run");
        }

    }

    void Run()
    {
        if (isRunning && TargetPos != null)
        {
            Debug.Log($"Boss moving towards {TargetPos.position}");
            transform.position = Vector2.MoveTowards(transform.position, TargetPos.position, Speed * Time.deltaTime);
        }
    }

    public void Damage(int damageAmount)
    {
        if (isDead) return; // Don't process damage if already dead

        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Prevent duplicate calls
        isDead = true;

        OnDeath?.Invoke();
        animator.SetTrigger("death");
        DropCoin();
        DropHealthItem();
        DropInstructor();
        GetComponent<Collider2D>().enabled = false; // Disable the collider
        rb.velocity = Vector2.zero; // Stop any movement
        rb.isKinematic = true; // Prevent physics interactions
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return; // Ignore interactions if dead

        if (other.gameObject.CompareTag("Player"))
        {
            // only apply damage if enough time has passed since the last damage 
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                TargetPos.GetComponent<NewPlayerMovement>()?.Damage(damage);
                TargetPos = null;
            }
        }

        else if (other.gameObject.CompareTag("Bullet"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                Damage(10);
                Destroy(other.gameObject);
            }
        }
    }

    private void DropInstructor()
    {
        if (instructorPrefab != null)
        {
            Instantiate(instructorPrefab, transform.position, Quaternion.identity);
        }
    }

    private void DropHealthItem()
    {
        if (healthItemPrefab != null)
        {
            float spawnOffset = UnityEngine.Random.Range(-3f, 1f);
            Vector2 spawnPosition = new Vector2(transform.position.x + spawnOffset, transform.position.y);

            Instantiate(healthItemPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void DropCoin()
    {
        if (coinPrefab != null)
        {
            float spawnOffset = UnityEngine.Random.Range(-3f, 1f);
            Vector2 spawnPosition = new Vector2(transform.position.x + spawnOffset, transform.position.y);

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

}