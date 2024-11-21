using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private float dashTimer;
    private float dashCooldownTimer;
    private bool isDashing;

    public float moveSpeed = 5f;
    private float dashDistance = 10f;

    public Rigidbody2D rigidbody2D;
    public Animator animator;
    private GameInput gameInput;

    private Vector2 lastMoveDir;
    public Vector2 movement;

    private void Awake()
    {
        gameInput = new GameInput();
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

    void Update()
    {
        HandleMovement();
    }


    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (!isDashing)
        {
            rigidbody2D.velocity = new Vector2(movement.x, movement.y).normalized * _speed;
        }
    }

    private void HandleMovement()
    {
        movement = gameInput.Player.Move.ReadValue<Vector2>();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement != Vector2.zero)
        {
            lastMoveDir = movement.normalized;
        }
    }

    private bool TryMove(Vector2 dir, float distance)
    {
        return Physics2D.Raycast(rigidbody2D.position, dir, distance);
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (dashCooldownTimer <= 0f && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
        else
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void StartDash()
    {
        Debug.Log("dashing");
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        rigidbody2D.velocity = transform.up * dashSpeed;
    }

    private void EndDash()
    {
        isDashing = false;
        rigidbody2D.velocity = Vector2.zero;
    }
}
