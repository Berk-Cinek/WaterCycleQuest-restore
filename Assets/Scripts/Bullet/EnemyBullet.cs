using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private int damage = 10;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        NewPlayerMovement target = collider.GetComponent<NewPlayerMovement>();
        if (target != null)
        {
            target.Damage(damage);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Bullet hit something else: " + collider.name);
        }
    }

}
