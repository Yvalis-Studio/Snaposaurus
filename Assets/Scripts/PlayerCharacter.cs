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
    public float groundCheckDistance = 0.1f;
    public float groundCheckOffset = 0.1f;
    private bool isGrounded;
    private bool jumpPressed;
    private Collider2D playerCollider;
    private float lastJumpTime = -1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        JumpAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ground check first
        CheckGround();

        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        // Check for jump input - only if grounded
        if (JumpAction.WasPressedThisFrame())
        {
            Debug.Log($"Space pressed! isGrounded: {isGrounded}");
            if (isGrounded)
            {
                jumpPressed = true;
                Debug.Log("JUMPING!");
            }
        }
    }

    void FixedUpdate()
    {
        // Apply jump
        if (jumpPressed)
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
            jumpPressed = false;
            lastJumpTime = Time.time;
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
        // Only check if we're falling or standing (not jumping up)
        if (rigidbody2d.linearVelocity.y > 0.1f)
        {
            isGrounded = false;
            return;
        }

        // Get the bottom of the player's collider
        Bounds bounds = playerCollider.bounds;
        Vector2 checkPosition = new Vector2(bounds.center.x, bounds.min.y - 0.05f);

        // Use OverlapCircle to check for ground - more reliable with Composite Collider
        Collider2D hit = Physics2D.OverlapCircle(checkPosition, 0.1f, groundLayer);

        // Only grounded if we hit something
        isGrounded = hit != null;

        Debug.Log($"CheckPos: {checkPosition}, Hit: {hit != null}, HitObject: {hit?.name}, GroundLayer: {groundLayer.value}");

        // Debug visualization
        Debug.DrawRay(checkPosition, Vector2.down * 0.1f, isGrounded ? Color.green : Color.red);
    }
}
