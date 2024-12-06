using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    public int health = 100;
    public Transform target;
    public float speed = 3f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;

    public float stoppingDistance = 0f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    public Transform player;
    public GameObject aoeMarkerPrefab; 
    public float jumpHeight = 5f; 
    public float jumpDuration = 1f; 
    public float slamDelay = 1f; 
    public float slamRadius = 2f;
    public int damage = 20;
    private Vector3 originalPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetTarget(); // Initialize target
        originalPosition = transform.position; // Save starting position
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(target.position, transform.position);

            if (distanceToTarget > stoppingDistance)
            {
                // Calculate direction to the player
                Vector2 direction = (target.position - transform.position).normalized;

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
        if (!target)
        {
            GetTarget(); // Continuously look for the player
        }

        if (target != null && Vector2.Distance(target.position, transform.position) <= stoppingDistance)
        {
            StartJumpAttack();
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

    private IEnumerator JumpAttack()
    {
        //Jump
        Vector3 peakPosition = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, peakPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // AoE Marker
        GameObject aoeMarker = Instantiate(aoeMarkerPrefab, player.position, Quaternion.identity);
        Destroy(aoeMarker, slamDelay); // Auto-destroy marker 

        yield return new WaitForSeconds(slamDelay);

        //Slam
        transform.position = originalPosition;

        // Check players in AoE
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, slamRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Deal damage to the player
                Debug.Log("Player hit by slam!");
                // Add player damage logic here
            }
        }
    }   

    private void OnDrawGizmosSelected()
    {
        // Visualize the slam radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }

private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject); //killing the player
            target = null;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Damage(10); // Take damage
            Destroy(other.gameObject);
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
}
