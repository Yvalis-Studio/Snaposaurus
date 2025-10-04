using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    //Variables for Movement
    Vector2 moveDirection = new Vector2(1, 0);
    public InputAction MoveAction;
    public InputAction JumpAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    //Variables for Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;
    private bool isGrounded;
    private bool jumpPressed;
    private CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        JumpAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        // Check for jump input
        if (JumpAction.WasPressedThisFrame() && isGrounded)
        {
            jumpPressed = true;
        }

        // Ground check
        CheckGround();
    }

    void FixedUpdate()
    {
        // Apply jump
        if (jumpPressed)
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }

        // Better jump physics
        if (rigidbody2d.linearVelocity.y < 0)
        {
            // Falling - apply extra gravity
            rigidbody2d.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rigidbody2d.linearVelocity.y > 0 && !JumpAction.IsPressed())
        {
            // Released jump button early - apply extra gravity for variable jump height
            rigidbody2d.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Apply horizontal movement
        rigidbody2d.linearVelocity = new Vector2(move.x * speed, rigidbody2d.linearVelocity.y);
    }

    void CheckGround()
    {
        // Cast from bottom of collider
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - circleCollider.radius);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        // Debug visualization
        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
}
