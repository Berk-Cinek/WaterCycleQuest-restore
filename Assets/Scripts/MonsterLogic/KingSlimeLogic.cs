using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private int jumpCoolDown = 10;

    public Transform player;
    public GameObject aoeMarkerPrefab; 
    public float jumpHeight = 15f; 
    public float jumpDuration = 10f;
    public float slamDelay = 5f; 
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

        //if (isJumping)
        //{
           
        //}else
        //{
           
        //}
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
        Vector3 startPosition = transform.position; // Starting position of the slime
        Vector3 offScreenPosition = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z); // Off-screen target position
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, offScreenPosition, elapsedTime / jumpDuration); // Smoothly move upwards
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Place AoE Marker
        GameObject aoeMarker = Instantiate(aoeMarkerPrefab, player.position, Quaternion.identity);
        Vector3 slamPosition = player.position; // Store the player's last position before slam

        float markerFollowTime = slamDelay;
        while (markerFollowTime > 0f)
        {
            if (aoeMarker != null)
            {
                aoeMarker.transform.position = player.position; // Update marker
                slamPosition = player.position; // update slam
            }
            markerFollowTime -= Time.deltaTime;
            yield return null;
        }

        Destroy(aoeMarker);

        // Slam
        transform.position = player.position;

        //// check in AoE
        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, slamRadius);
        //foreach (var hit in hits)
        //{
        //    if (hit.CompareTag("Player"))
        //    {
        //        Debug.Log("Player hit by slam!");
                   
        //    }
        //}
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
    public bool isJumping()
    {
        if (jumpCoolDown == 0)
        {
           jumpCoolDown = 10;
           return true;
        }
        else
        {
            jumpCoolDown -= 1;
            return false;
        }
    }
}
