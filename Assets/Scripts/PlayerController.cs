using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Input")]
    public InputAction moveAction;
    public InputAction jumpAction;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Enable input actions
        moveAction.Enable();
        jumpAction.Enable();
    }

    void Update()
    {
        // AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        // Get input
        moveInput = moveAction.ReadValue<Vector2>().x;
        animator.SetFloat("Speed", Math.Abs(moveInput));
        if (!Mathf.Approximately(moveInput, 0.0f) && moveInput > 0)
        {
            animator.SetBool("Right", true);
        }
        if (!Mathf.Approximately(moveInput, 0.0f) && moveInput < 0)
        {
            animator.SetBool("Right", false);
        }
        

        // Check if grounded
        CheckGrounded();
        animator.SetBool("isGrounded", isGrounded);

        // Jump
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Better jump physics
        ApplyJumpPhysics();
    }

    void CheckGrounded()
    {
        // Use OverlapCircle at the groundCheck position
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Fallback: check below player
            Vector2 checkPos = (Vector2)transform.position + Vector2.down * 0.5f;
            isGrounded = Physics2D.OverlapCircle(checkPos, groundCheckRadius, groundLayer);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetBool("isGrounded", false);
        animator.SetTrigger("Jump");
    }

    void ApplyJumpPhysics()
    {
        // Fall faster than rise
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Variable jump height - let go of jump to fall sooner
        else if (rb.linearVelocity.y > 0 && !jumpAction.IsPressed())
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize ground check in editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
