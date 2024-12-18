using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class SlimeDevilLogic : MonoBehaviour , IDamageable
{
    private Transform TargetPos;
    [SerializeField] private int health = 100;
    private Vector3 direction;
    private RaycastHit2D ray;
    private SpriteRenderer bodySprite;
    [SerializeField] float rayDistance = 30;
    private Animator animator;
    float Speed = 5f;
    public bool isRunning = true;
    private float lastDamageTime = 0f;
    private float damageCooldown = 1f;
    [SerializeField] private int damage = 20;

    public GameObject bulletPrefab;
    [SerializeField] private GameObject instructorPrefab;
    [SerializeField] private GameObject healthItemPrefab;
    [SerializeField] private GameObject coinPrefab;

    public event Action OnDeath;

    event Action IDamageable.OnDeath
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    void Awake()
    {
        bodySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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

    void GetDirection()
    {
        direction = (TargetPos.position - transform.position).normalized;
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
            animator.SetBool("inRange", true);
        }
        else if (ray.collider != null && ray.collider.CompareTag("Wall"))
        {
            isRunning = false;
            animator.SetBool("inRange", true);
        }

    }

    void Run()
    {
        if (isRunning)
        {
            animator.SetBool("canWalk", true);
            transform.position += direction * Speed * Time.deltaTime;
        }
        else
        {
            animator.SetBool("canWalk", false);
        }


    }



    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("takeHitTrigger");
            animator.SetBool("hitWalk", true);
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        animator.SetTrigger("deathTrigger");
        DropCoin();
        DropHealthItem();
        DropInstructor();
    }

    private void destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // only apply damage if enough time has passed since the last damage 
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time; // Update
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

            Instantiate(healthItemPrefab, spawnPosition , Quaternion.identity);
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
