using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

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
    public InputAction climbAction;

    [Header("Ladder")]
    public bool isClimbing;

    [Header("SoundEffects")]
    public AudioSource footstepsSound;

    Rigidbody2D rb;
    bool isGrounded;
    float moveInput;
    float climbInput;

    Animator animator;

    bool facingRight = true;

    void Start()
    {
        GameManager.Instance.RestorePlayerPosition(gameObject);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Enable input actions
        moveAction.Enable();
        jumpAction.Enable();
        climbAction.Enable();
    }

    void Update()
    {
        // Get input
        moveInput = moveAction.ReadValue<Vector2>().x;
        if (isClimbing)
        {
            climbInput = climbAction.ReadValue<Vector2>().y;
            animator.SetBool("IsClimbing", true);
        }
        else
        {
            animator.SetBool("IsClimbing", false);
        }

        // Set Animation
            animator.SetFloat("Speed", Math.Abs(moveInput));
        if (!Mathf.Approximately(moveInput, 0.0f) && moveInput > 0 && !facingRight)
        {
            Flip();
        }
        if (!Mathf.Approximately(moveInput, 0.0f) && moveInput < 0 && facingRight)
        {
            Flip();
        }


        // Check if grounded
            CheckGrounded();
        animator.SetBool("isGrounded", isGrounded);

        // Jump
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }

        if (!Mathf.Approximately(moveInput, 0.0f) && isGrounded)
        {
            footstepsSound.enabled = true;
        }
        else
        {
            footstepsSound.enabled = false;
        }
    }

    void FixedUpdate()
    {
        // Movement
        if (!isClimbing)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, climbInput * moveSpeed);
        }


        // Better jump physics
        ApplyJumpPhysics();
    }
    
    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;  // invert the X scale
        transform.localScale = scale;
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
