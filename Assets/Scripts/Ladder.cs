using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ladder : MonoBehaviour
{
    private bool isInRange;
    private bool isClimbingLadder;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow climbing if player is in range and pressed E
        if (isInRange && InputManager.Instance != null)
        {
            // Start climbing when E is pressed
            if (InputManager.Instance.InteractAction.WasPressedThisFrame())
            {
                isClimbingLadder = !isClimbingLadder; // Toggle climbing on/off
            }

            // Set climbing state
            playerController.isClimbing = isClimbingLadder;
        }
        else
        {
            // Player left ladder range
            isClimbingLadder = false;
            playerController.isClimbing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
