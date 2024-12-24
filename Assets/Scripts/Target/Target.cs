using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable, IFreezeable
{
    private Animator animator;
    public event System.Action OnDeath;
    public int health = 100;
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.0025f;
    private SpriteRenderer bodySprite;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;

    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;

    public float fireRate;
    private float timeToFire;

    public Transform firingPoint;
    public GameObject healthItemPrefab;
    public GameObject coinPrefab;

    private bool isFrozen = false;
    private bool isDead = false; // Added to prevent actions after death

    private void Start()
    {
        bodySprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GetTarget();
    }

    private void Update()
    {
        if (isFrozen || isDead) return;

        SetSpriteFlip();

        if (!target)
        {
            GetTarget();
        }

        if (Vector2.Distance(target.position, transform.position) <= distanceToShoot)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (isFrozen || isDead) return;

        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
            {
                animator.SetBool("canWalk", true);
                Vector2 direction = (target.position - transform.position).normalized;
                rb.velocity = direction * speed;
            }
            else
            {
                animator.SetBool("canWalk", false);
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void SetSpriteFlip()
    {
        if (target && target.position.x - transform.position.x < 0)
        {
            bodySprite.flipX = true;
        }
        else if (target && target.position.x - transform.position.x > 0)
        {
            bodySprite.flipX = false;
        }
    }

    private void Shoot()
    {
        if (isFrozen || isDead) return;

        if (timeToFire <= 0f)
        {
            animator.SetBool("inRange", true);
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            Debug.Log("Shoot");
            timeToFire = fireRate;
        }
        else
        {
            timeToFire -= Time.deltaTime;
        }
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched the enemy!");

            // Damage the player instead of destroying instantly
            NewPlayerMovement player = other.GetComponent<NewPlayerMovement>();
            if (player != null)
            {
                player.Damage(10); // Adjust damage as needed
            }

            animator.SetTrigger("takeHitTrigger"); // Trigger the enemy's hit animation
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit the enemy!");
            Destroy(other.gameObject);
            Damage(10); // Apply damage to the enemy when hit by a bullet
        }
    }

    public void Damage(int damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining health: " + health);

        animator.SetTrigger("takeHitTrigger"); // Display hit animation

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("deathTrigger");
        OnDeath?.Invoke();
        Debug.Log(gameObject.name + " has died!");

        rb.velocity = Vector2.zero; // Stop movement
        rb.isKinematic = true; // Disable physics interactions
        GetComponent<Collider2D>().enabled = false; // Disable collider
        DropHealthItem();
        DropCoin();

        // Optionally destroy the object after the death animation
        Destroy(gameObject, 1.5f); // Adjust the delay to match the death animation
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
