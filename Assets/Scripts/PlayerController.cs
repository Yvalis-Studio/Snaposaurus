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

    [Header("SoundEffects")]
    public AudioSource footstepsSound;

    [Header("Ladder")]
    private bool _isClimbing;
    public bool isClimbing
    {
        get { return _isClimbing; }
        set
        {
            if (_isClimbing == value) return; // Ne fait rien si l'état ne change pas
            _isClimbing = value;

            if (_isClimbing)
            {
                // Passe en mode "fantôme" pour ignorer toute la physique
                rb.bodyType = RigidbodyType2D.Kinematic; 
                rb.linearVelocity = Vector2.zero; // Stoppe tout mouvement en cours
                animator.SetBool("IsClimbing", true);
            }
            else
            {
                // Revient au mode physique normal
                rb.bodyType = RigidbodyType2D.Dynamic; 
                animator.SetBool("IsClimbing", false);
            }
        }
    }
    
    // Variables internes
    private bool isOnLadder;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private float climbInput;
    private Animator animator;
    private bool facingRight = true;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestorePlayerPosition(gameObject);
        }
        else
        {
            Debug.LogWarning("[PlayerController] GameManager.Instance is null - cannot restore player position");
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (InputManager.Instance == null) return;

        // Active/désactive le mode grimpe si on est sur une échelle et qu'on appuie sur 'E'
        if (isOnLadder && InputManager.Instance.InteractAction.WasPressedThisFrame())
        {
            isClimbing = !isClimbing;
        }

        // On lit les inputs à chaque frame
        moveInput = InputManager.Instance.MoveAction.ReadValue<Vector2>().x;
        climbInput = InputManager.Instance.MoveAction.ReadValue<Vector2>().y;

        // L'animation de marche ne s'active que si on ne grimpe pas
        animator.SetFloat("Speed", isClimbing ? 0 : Math.Abs(moveInput));
        
        // On ne retourne le personnage que s'il ne grimpe pas
        if (!isClimbing)
        {
            if (!Mathf.Approximately(moveInput, 0.0f) && moveInput > 0 && !facingRight) Flip();
            if (!Mathf.Approximately(moveInput, 0.0f) && moveInput < 0 && facingRight) Flip();
        }

        CheckGrounded();
        animator.SetBool("isGrounded", isGrounded);

        if (InputManager.Instance.JumpAction.WasPressedThisFrame() && isGrounded && !isClimbing)
        {
            Jump();
        }

        if (!Mathf.Approximately(moveInput, 0.0f) && isGrounded && !isClimbing) 
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
        // La physique est gérée ici
        if (isClimbing)
        {
            // En mode Kinematic, on modifie directement la vélocité
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, climbInput * moveSpeed);
        }
        else
        {
            // En mode Dynamic, on applique les forces normalement
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            ApplyJumpPhysics();
        }
    }

    // Méthode appelée par le script Ladder pour dire au joueur s'il est à portée
    public void SetIsOnLadder(bool onLadder)
    {
        isOnLadder = onLadder;
        // Si le joueur quitte la zone de l'échelle, on force l'arrêt de la grimpe
        if (!isOnLadder)
        {
            isClimbing = false;
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void CheckGrounded()
    {
        if (isClimbing) 
        {
            isGrounded = false; // On ne peut pas être "grounded" en grimpant
            return;
        }

        if (groundCheck != null) 
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
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
        if (InputManager.Instance == null) return;
        if (rb.linearVelocity.y < 0) 
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !InputManager.Instance.JumpAction.IsPressed()) 
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}