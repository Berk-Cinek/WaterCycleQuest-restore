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

    private void Start()
    {
        bodySprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GetTarget();
    }

    private void Update()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

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

    void SetSpriteFlip()
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

    private void Shoot()
    {
        if (isFrozen) return;

        if (timeToFire <= 0f)
        {
            animator.SetBool("inRange", true);
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            Debug.Log("Shoot");
            timeToFire = fireRate;
            animator.SetBool("inRange", false);
        }
        else
        {
            animator.SetBool("inRange", false);
            timeToFire -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isFrozen) return; 

        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
            {
                animator.SetBool("canWalk", true );
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

    

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            target = null;
            animator.SetTrigger("takeHitTrigger");
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
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
        else
        {
            animator.SetTrigger("hitWalkTrigger");
        }
    }

    private void Die()
    {
        animator.SetTrigger("deathTrigger");
        OnDeath?.Invoke();
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
        DropHealthItem();
        DropCoin();
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
