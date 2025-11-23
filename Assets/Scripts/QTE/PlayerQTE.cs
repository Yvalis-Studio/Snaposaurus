using UnityEngine;

public class PlayerQTE : MonoBehaviour
{
    public QTEManager qteManager;

    // ANIM
    Animator animator;
    bool successTriggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Input is now managed by InputManager - no manual enabling needed
        successTriggered = false;
    }

    void Update()
    {
        if (InputManager.Instance == null || !qteManager.isActive)
        {
            // Check if QTE just became inactive with success
            if (qteManager.isSuccess && !successTriggered)
            {
                animator.SetTrigger("Success");
                successTriggered = true;
            }
            return;
        }

        // Check directional inputs using InputManager
        if (InputManager.Instance.WasDirectionPressedThisFrame("up"))
        {
            qteManager.DoQTE("up");
        }
        else if (InputManager.Instance.WasDirectionPressedThisFrame("down"))
        {
            qteManager.DoQTE("down");
        }
        else if (InputManager.Instance.WasDirectionPressedThisFrame("left"))
        {
            qteManager.DoQTE("left");
        }
        else if (InputManager.Instance.WasDirectionPressedThisFrame("right"))
        {
            qteManager.DoQTE("right");
        }
    }

    public void ResetSuccess()
    {
        successTriggered = false;
    }
}
