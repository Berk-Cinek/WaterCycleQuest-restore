using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 3f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    [SerializeField] private GameObject instructorPrefab;
    private float dashCooldownTimer;
    [SerializeField] private float dashCooldown = 0.2f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float detectionRange = 10f;

    [SerializeField] private float stoppingDistance = 0f;
    [SerializeField] private float lastAttackTime;
    [SerializeField] private float jumpCooldownTimer = 10f;

    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private float jumpDuration = 10f;
    [SerializeField] private float slamDelay = 5f;
    [SerializeField] private int damage = 20;
    private Vector3 originalPosition;

    [SerializeField] private Transform playerTransform;
    private float lastDamageTime = 0f;  
    public float damageCooldown = 1f;   


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetTarget();
        originalPosition = transform.position;
        dashCooldownTimer = dashCooldown;
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            float distanceToTarget = Vector2.Distance(playerTransform.position, transform.position);

            if (distanceToTarget > stoppingDistance)
            {
                // Calculate direction to the player
                Vector2 direction = (playerTransform.position - transform.position).normalized;

                // Move
                rb.velocity = direction * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
    
    private void Update()
    {
        if (!playerTransform)
        {
            GetTarget(); // Continuously look for the player
        }

        if (isJumping())
        {
            StartCoroutine(JumpAttack());
        }
        else if (Vector2.Distance(playerTransform.position, transform.position) <= detectionRange && dashCooldownTimer <= 0f)
        {
            Debug.Log("is dashing");
            dashCooldownTimer -= Time.deltaTime;

            if (dashCooldownTimer <= 0f)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void GetTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Player found and assigned to target: " + playerTransform.position);
        }
        else
        {
            Debug.LogWarning("Player not found! Ensure the player GameObject is tagged correctly.");
        }
    }

    private IEnumerator JumpAttack()
    {
        // Jump
        Vector3 startPosition = transform.position;
        Vector3 offScreenPosition = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, offScreenPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Delay and track
        Vector3 slamPosition = playerTransform.position;
        float markerFollowTime = slamDelay;

        while (markerFollowTime > 0f)
        {
            // Continuously update the slam
            slamPosition = playerTransform.position;
            markerFollowTime -= Time.deltaTime;
            yield return null;
        }

        // Slam down at the last tracked position
        elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(offScreenPosition, slamPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }


    private IEnumerator Dash()
    {
        rb.velocity = (playerTransform.position - transform.position).normalized * dashSpeed;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        dashCooldownTimer = dashCooldown;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // only apply damage if enough time has passed since the last damage 
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time; // Update
                playerTransform.GetComponent<NewPlayerMovement>()?.Damage(damage); 
                playerTransform = null;
            }
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                Damage(10);
                //yağıza sor tek yiyor
                Destroy(other.gameObject); 
            }
        }
    }

    public void StartJumpAttack()
    {
        StartCoroutine(JumpAttack());
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
        Destroy(gameObject);
        DropCoin();
    }
    public bool isJumping()
    {
        if (jumpCooldownTimer <= 0f)
        {
            jumpCooldownTimer = 10f;
            return true;
        }
        else
        {
            jumpCooldownTimer -= Time.deltaTime;
            return false;
        }
    }

    private void DropCoin()
    {
        if (instructorPrefab != null)
        {
            float spawnOffset = Random.Range(-3f, 1f);
            Vector2 spawnPosition = new Vector2(transform.position.x + spawnOffset, transform.position.y);

            Instantiate(instructorPrefab, spawnPosition, Quaternion.identity);
        }
    }
}