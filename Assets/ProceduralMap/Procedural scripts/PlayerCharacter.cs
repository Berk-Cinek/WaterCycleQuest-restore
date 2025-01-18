using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Animator animator;
    private Rigidbody2D rb;
    private GameInput gameInput;
    private Vector2 inputVector;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        gameInput.Enable();
        gameInput.Player.Move.performed += OnMovePerformed;
        gameInput.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        gameInput.Player.Move.performed -= OnMovePerformed;
        gameInput.Player.Move.canceled -= OnMoveCanceled;
        gameInput.Disable();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", inputVector.sqrMagnitude);

        

    }

    private void FixedUpdate()
    {
        Vector2 moveDir = inputVector.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        inputVector = Vector2.zero;
    }
}
