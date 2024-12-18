using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningRatMovement : MonoBehaviour, IDamageable
{
    Transform TargetPos;
    [SerializeField] private int health = 100;
    Vector3 direction;
    RaycastHit2D ray;
    SpriteRenderer bodySprite;
    [SerializeField] float rayDistance = 30;
    Animator animator;
    float Speed = 5f;
    public bool isRunning = true;
    private float lastDamageTime =0f;
    private float damageCooldown = 1f;
    [SerializeField] private int damage = 20;
    public GameObject bulletPrefab;

    public event Action OnDeath;

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

    void GetTargetPos(){
        if(GameObject.FindGameObjectWithTag("Player") !=null){
            TargetPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void GetDirection()
    {
        direction = (TargetPos.position - transform.position).normalized;
    }

    void SetSpriteFlip()
    {
        if(TargetPos.position.x - transform.position.x < 0)
        {
            bodySprite.flipX = false;
        }
        else if (TargetPos.position.x - transform.position.x >0)
        {
            bodySprite.flipX = true;
        }

    }

    void Attack()
    {
        if (ray.collider != null && ray.collider.CompareTag("Player")){
            isRunning = false;
            animator.Play("Base Layer.RunningRatAttack", default);
        }
        else if (ray.collider!= null && ray.collider.CompareTag("Wall"))
        {
            isRunning = false;
            animator.Play("Base Layer.RunningRatAttack", default);
        }
        
    }

    void Run()
    {
        if (isRunning)
        {
            animator.Play("Base Layer.RunningRatRunning", default);
            transform.position += direction * Speed * Time.deltaTime;
        }


    }


    
     public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
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

}