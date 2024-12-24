using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target2 : MonoBehaviour, IDamageable, IFreezeable
{
    public event System.Action OnDeath;
    public int health = 100;
    public Transform target;
    public float speed = 3f;
    public float attackRange = 1.5f;
    public int damage = 20;
    public float attackCooldown = 1f;

    private Rigidbody2D rb;
    private float timeSinceLastAttack;
    private bool isFrozen = false;

    public GameObject healthItemPrefab;
    public GameObject coinPrefab;

    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timeSinceLastAttack = 0f;
    }

    private void Update()
    {
        if (isFrozen) return;

        timeSinceLastAttack += Time.deltaTime;

        if (!target)
        {
            GetTarget();
        }
        else
        {
            if (Vector2.Distance(target.position, transform.position) <= attackRange)
            {
                AttemptAttack();
            }
            else
            {
                // Stop the attack animation if the target is out of range
                if (anim.GetBool("attacking"))
                {
                    anim.SetBool("attacking", false); // Stop attacking animation
                    anim.SetBool("moving", true);     // Transition to moving animation
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (target != null && Vector2.Distance(target.position, transform.position) > attackRange)
        {
            Vector2 moveDirection = (target.position - transform.position).normalized;
            rb.velocity = moveDirection * speed;

            // Update animations
            SetAnimationDirection(moveDirection);
            anim.SetBool("moving", true); // Transition to walking animation
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("moving", false); // Transition to idle animation
        }
    }

    private void AttemptAttack()
    {
        // Check if enough time has passed for the next attack
        if (timeSinceLastAttack >= attackCooldown)
        {
            // Calculate the direction to the target
            Vector2 attackDirection = (target.position - transform.position).normalized;

            // Log distance to check if the attack range condition is met
            float distanceToTarget = Vector2.Distance(target.position, transform.position);
            Debug.Log($"Distance to target: {distanceToTarget} | Attack range: {attackRange}");

            // Check if the target is within the attack range
            if (distanceToTarget <= attackRange)
            {
                // Set direction for attack animation
                SetAnimationDirection(attackDirection);

                // Trigger the attack animation if it's not already playing
                if (!anim.GetBool("attacking"))  // Prevent retriggering the attack animation
                {
                    anim.SetBool("attacking", true);  // Trigger attack animation
                    anim.SetBool("moving", false);    // Stop moving animation
                    anim.SetTrigger("attack");        // Trigger the attack animation

                    timeSinceLastAttack = 0f;

                    // Deal damage to the player if the target is in range
                    target.GetComponent<NewPlayerMovement>()?.Damage(damage);
                }
            }
            else
            {
                // Ensure attack animation stops if the target is out of range
                if (anim.GetBool("attacking"))
                {
                    anim.SetBool("attacking", false);  // Stop attack animation
                    anim.SetBool("moving", true);      // Transition to moving animation
                }
            }
        }
        else
        {
            // Ensure attack animation stops after cooldown period
            if (anim.GetBool("attacking"))
            {
                anim.SetBool("attacking", false);  // Stop attack animation if cooldown isn't met
                anim.SetBool("moving", true);      // Transition to moving animation
            }
        }
    }

    private void SetAnimationDirection(Vector2 direction)
    {
        direction.Normalize();

        // Debug log to see values of moveX and moveY
        Debug.Log($"SetAnimationDirection: {direction.x}, {direction.y}");

        // Prioritize vertical movement over horizontal if Y direction is greater
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) // Prioritize vertical movement
        {
            if (direction.y > 0)
            {
                anim.SetFloat("moveX", 0f);
                anim.SetFloat("moveY", 1f); // Upward
            }
            else
            {
                anim.SetFloat("moveX", 0f);
                anim.SetFloat("moveY", -1f); // Downward
            }
        }
        else // Prioritize horizontal movement
        {
            if (direction.x > 0)
            {
                anim.SetFloat("moveX", 1f);
                anim.SetFloat("moveY", 0f);
            }
            else
            {
                anim.SetFloat("moveX", -1f);
                anim.SetFloat("moveY", 0f);
            }
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
        OnDeath?.Invoke();
        Debug.Log(gameObject.name + " has died!");

        DropHealthItem();
        DropCoin();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.transform;
        }
    }

    private void DropHealthItem()
    {
        if (healthItemPrefab != null)
        {
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
    }

    private void DropCoin()
    {
        if (coinPrefab != null)
        {
            float spawnOffset = Random.Range(-3f, 1f);
            Vector2 spawnPosition = new Vector2(transform.position.x + spawnOffset, transform.position.y);

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;

        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            Debug.Log($"{gameObject.name} is frozen.");
        }
        else
        {
            Debug.Log($"{gameObject.name} is unfrozen.");
        }
    }
}
