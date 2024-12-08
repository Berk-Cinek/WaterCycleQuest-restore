using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target2 : MonoBehaviour, IDamageable, IFreezeable
{
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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            RotateTowardsTarget();

            if (Vector2.Distance(target.position, transform.position) <= attackRange)
            {
                AttemptAttack();
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
            rb.velocity = (target.position - transform.position).normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void AttemptAttack()
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            Debug.Log("Melee attack on player!");
            target.GetComponent<NewPlayerMovement>()?.Damage(damage);
            timeSinceLastAttack = 0f;
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
