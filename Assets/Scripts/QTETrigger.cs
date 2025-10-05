using UnityEditor;
using UnityEngine;

public class QTETrigger : MonoBehaviour
{
    public SceneAsset targetScene;

    bool playerInRange = false;
    PlayerController player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.Dino1.isActive)
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
            bool playerInteracting = player.interactAction.WasPressedThisFrame();
            if (playerInteracting)
            {
                // Save current position before leaving overworld
                GameManager.Instance.SavePlayerPosition(player.transform.position);
                GameManager.Instance.Dino1.isActive = false;

                // Begin transition
                SceneTransition.Instance.TransitionToScene(targetScene.name);
            }
        }
    }
}
