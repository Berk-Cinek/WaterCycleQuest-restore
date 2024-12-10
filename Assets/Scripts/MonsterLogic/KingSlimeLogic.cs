using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    public int health = 100;
    public float speed = 3f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;

    public float stoppingDistance = 0f;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    private float jumpCooldownTimer = 10f;

    public float jumpHeight = 15f; 
    public float jumpDuration = 10f;
    public float slamDelay = 5f; 
    public float slamRadius = 2f;
    public int damage = 20;
    private Vector3 originalPosition;

    [SerializeField] private Transform playerTransform;
    private float lastDamageTime = 0f;  
    public float damageCooldown = 1f;   


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetTarget(); // Initialize target
        originalPosition = transform.position; // Save starting position
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

        Debug.Log("Slime is jumping up!");
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, offScreenPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Delay and track
        Debug.Log("Tracking player's position for the slam.");
        Vector3 slamPosition = playerTransform.position;
        float markerFollowTime = slamDelay;

        while (markerFollowTime > 0f)
        {
            // Continuously update the slam
            slamPosition = playerTransform.position;
            Debug.Log("Tracking player at position: " + slamPosition);
            markerFollowTime -= Time.deltaTime;
            yield return null;
        }

        // Slam down at the last tracked position
        elapsedTime = 0f;
        Debug.Log("Slime is slamming down at position: " + slamPosition);
        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(offScreenPosition, slamPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Slam completed at position: " + slamPosition);
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // only apply damage if enough time has passed since the last damage 
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time; // Update
                Damage(10); 
                Destroy(other.gameObject); // Killing the player
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
}
