using System.Collections;
using UnityEngine;

public class Target3 : MonoBehaviour, IDamageable, IFreezeable
{
    private bool isAnimated = false;
    private Animator animator;
    public event System.Action OnDeath;
    public int health = 100;
    public Transform target;
    public float speed = 2f;
    public float dashSpeed = 10f;
    public float detectionRange = 5f;
    public float dashRange = 1f;
    public int damage = 10;
    public float dashCooldown = 3f;

    private SpriteRenderer bodySprite;
    private Rigidbody2D rb;
    private bool isDashing = false;
    private bool hasDealtDamage = false;
    private float dashCooldownTimer;
    private bool isFrozen = false;
    private bool isDead = false; // New flag to track death state
    public GameObject healthItemPrefab;
    public GameObject coinPrefab;

    private void Start()
    {
        bodySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dashCooldownTimer = dashCooldown;
    }

    private void Update()
    {
        if (isDead) return; // Skip all behavior if dead

        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!target)
        {
            GetTarget();
            return;
        }

        SetSpriteFlip();
        dashCooldownTimer -= Time.deltaTime;

        if (Vector2.Distance(target.position, transform.position) <= detectionRange && dashCooldownTimer <= 0f)
        {
            animator.SetBool("inRange", true);
            StartCoroutine(Dash());
        }
        else if (!isDashing)
        {
            animator.SetBool("inRange", false);
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        animator.SetBool("canWalk", true);
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void SetSpriteFlip()
    {
        if (target.position.x - transform.position.x < 0)
        {
            bodySprite.flipX = true;
        }
        else if (target.position.x - transform.position.x > 0)
        {
            bodySprite.flipX = false;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        hasDealtDamage = false;
        animator.SetTrigger("attackTrigger");
        rb.velocity = (target.position - transform.position).normalized * dashSpeed;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        dashCooldownTimer = dashCooldown;

        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // Ignore interactions if dead

        if (isDashing && !hasDealtDamage && collision.CompareTag("Player"))
        {
            NewPlayerMovement player = collision.GetComponent<NewPlayerMovement>();
            if (player != null)
            {
                player.Damage(damage);
                hasDealtDamage = true;
            }
        }
    }

    public void Damage(int damageAmount)
    {
        if (isDead) return; // Don't process damage if already dead

        animator.SetBool("takeDamageState", true);
        health -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining health: " + health);

        if (!isAnimated)
        {
            animator.SetTrigger("takeHitTrigger");
        }

        isAnimated = true;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            isAnimated = false;
            animator.SetTrigger("hitWalkTrigger");
        }
    }

    private void Die()
    {
        if (isDead) return; // Prevent duplicate calls
        isDead = true;

        OnDeath?.Invoke();
        Debug.Log(gameObject.name + " has died!");
        animator.SetTrigger("deathTrigger");
        DropHealthItem();
        DropCoin();
        GetComponent<Collider2D>().enabled = false; // Disable the collider
        rb.velocity = Vector2.zero; // Stop any movement
        rb.isKinematic = true; // Prevent physics interactions
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private void GetTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            target = player.transform;
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
