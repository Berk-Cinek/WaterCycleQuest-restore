using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] public int maxHealth = 100;

    private PlayerHealthBar playerHealthBar;
    public int health = 100;

    private int currentHealth;
    private float dashTimer;
    private float dashCooldownTimer;
    private bool isDashing;

    public Rigidbody2D rigidbody2D;
    public Animator animator;
    private GameInput gameInput;

    private Vector2 lastMoveDir;
    private Vector2 movement;

    [SerializeField] private Image coinIcon;
    [SerializeField] private TMP_Text coinCounterText;
    private int currentCoins = 0;

    private int playerDamage = 10;

    private void Awake()
    {
        playerHealthBar = GetComponentInChildren<PlayerHealthBar>();
        gameInput = new GameInput();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        gameInput.Enable();
        gameInput.Player.Dash.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        gameInput.Player.Dash.performed -= OnDashPerformed;
        gameInput.Disable();
    }

    private void Update()
    {
        if (dialogueUI.IsOpen)
        {
            rigidbody2D.velocity = Vector2.zero;
            movement = Vector2.zero;
            UpdateAnimator(Vector2.zero);
            return;
        }

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.E) && !dialogueUI.IsOpen)
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }

        HandleDashCooldown();
    }

    private void FixedUpdate()
    {
        if (isDashing || dialogueUI.IsOpen) return;

        rigidbody2D.MovePosition(rigidbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleMovement()
    {
        if (dialogueUI.IsOpen) return;

        movement = gameInput.Player.Move.ReadValue<Vector2>();
        UpdateAnimator(movement);

        if (movement != Vector2.zero)
        {
            lastMoveDir = movement.normalized;
        }
    }

    private void UpdateAnimator(Vector2 currentMovement)
    {
        animator.SetFloat("Horizontal", currentMovement.x);
        animator.SetFloat("Vertical", currentMovement.y);
        animator.SetFloat("Speed", currentMovement.sqrMagnitude);
        animator.SetBool("IsDashing", isDashing);
    }

    private void HandleDashCooldown()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
        else if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (dashCooldownTimer <= 0f && !isDashing && !dialogueUI.IsOpen)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        if (dialogueUI.IsOpen) return;

        Debug.Log("Dashing");
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            Vector2 dashStep = lastMoveDir * dashSpeed * Time.fixedDeltaTime;

            RaycastHit2D[] hitResults = new RaycastHit2D[1];
            int hitCount = rigidbody2D.Cast(dashStep.normalized, new ContactFilter2D(), hitResults, dashStep.magnitude);

            if (hitCount > 0)
            {
                Debug.Log("Dash hit: " + hitResults[0].collider.name);
                break;
            }

            rigidbody2D.MovePosition(rigidbody2D.position + dashStep);

            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        EndDash();
    }

    private void EndDash()
    {
        isDashing = false;
        rigidbody2D.velocity = Vector2.zero;
    }

    public void Damage(int damageAmount)
    {
     
        InvincibilitySkill invincibilitySkill = GetComponent<InvincibilitySkill>();
        if (invincibilitySkill != null && invincibilitySkill.IsInvincible())
        {
            Debug.Log("Player is invincible and took no damage!");
            return;
        }

        
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
       // Destroy(gameObject);
    }

    public void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        Debug.Log(gameObject.name + " restored " + amount + " health. Current health: " + health);
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = currentCoins.ToString();
        }
    }

    public void UpgradeDamage(int damageIncrease)
    {
        playerDamage += damageIncrease;
        Debug.Log("Damage upgraded to: " + playerDamage);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;  
        Debug.Log("Max health increased. Current max health: " + maxHealth);
        RestoreHealth(amount);  

        if (playerHealthBar != null)
        {
            playerHealthBar.UpdateHealthSlider();
        }
    }

    public void UpgradeSpeed(float speedIncrease)
    {
        moveSpeed += speedIncrease;
        Debug.Log($"Speed upgraded! Current move speed: {moveSpeed}");
    }

    public void UpgradeDashSpeed(float dashSpeedIncrease)
    {
        dashSpeed += dashSpeedIncrease;
        Debug.Log($"Dash speed upgraded! Current dash speed: {dashSpeed}");
    }

    public int GetDamage()
    {
        return playerDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            AddCoins(0);  
            Destroy(other.gameObject);  
        }
    }

    public int GetCoins()
    {
        return currentCoins;
    }

    public void SetInteractable(IInteractable interactable)
    {
        Interactable = interactable;
    }

    public void SetInvincibility(bool value)
    {
        
        Debug.Log($"Invincibility set to: {value}");
    }




}
