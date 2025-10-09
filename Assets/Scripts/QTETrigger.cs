using UnityEngine;

public class QTETrigger : MonoBehaviour
{
    public string targetScene;

    bool playerInRange = false;
    PlayerController player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerController>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            bool playerInteracting = InputManager.Instance != null &&
                                     InputManager.Instance.InteractAction.WasPressedThisFrame();

            if (playerInteracting)
            {
                // Save current position before leaving overworld
                GameManager.Instance.SavePlayerPosition(player.transform.position);

                // Begin transition
                SceneTransition.Instance.TransitionToScene(targetScene);
            }
        }
    }
}
