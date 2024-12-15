using UnityEngine;

public class EnemyBehavior_2 : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float walkSpeed = 2f;        // Movement speed
    public float attackRange = 1.5f;    // Range to stop and attack
    public int maxHealth = 100;         // Max health for the enemy

    private int currentHealth;          // Current health
    private bool isDead = false;        // To check if the enemy is dead

    [Header("Components")]
    private Animator animator;          // Animator for animations
    private Transform player;           // Reference to the player's position
    private Rigidbody2D rb;             // Rigidbody2D for movement

    void Start()
    {
        // Get required components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Find player GameObject with the tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player has the tag 'Player'.");
        }

        // Initialize health
        currentHealth = maxHealth;
    }

    void Update()
    {
        // If enemy is dead, stop updating
        if (isDead) return;

        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Stop moving and attack player
            animator.SetBool("inRange", true);
            rb.velocity = Vector2.zero; // Stop movement
            AttackPlayer();
        }
        else
        {
            // Move toward the player
            animator.SetBool("inRange", false);
            Patrol();
        }
    }

    void Patrol()
    {
        // Set walking animation
        animator.SetBool("canWalk", true);

        // Calculate direction toward the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the enemy
        rb.velocity = new Vector2(direction.x * walkSpeed, direction.y * walkSpeed);

        // Flip the sprite based on movement direction
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing right
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing left
        }
    }

    void AttackPlayer()
    {
        // Play attack animation
        animator.SetTrigger("attackTrigger");
    }

    public void TakeDamage(int damage)
    {
        // Reduce health
        currentHealth -= damage;
        Debug.Log("Enemy took damage! Current Health: " + currentHealth);

        // Play hit animation
        animator.SetTrigger("takeHitTrigger");

        // Check if health is zero or below
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Mark as dead
        isDead = true;

        // Stop all movement
        rb.velocity = Vector2.zero;

        // Play death animation
        animator.SetTrigger("deathTrigger");

        // Destroy the enemy GameObject after 2 seconds
        Destroy(gameObject, 2f);
    }

    // Detect collision with player's attack
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack")) // Ensure the attack has the correct tag
        {
            TakeDamage(20); // Adjust the damage value as needed
        }
    }
}
