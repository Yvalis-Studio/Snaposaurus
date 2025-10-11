using UnityEngine;

public class QTETrigger : MonoBehaviour
{
    [Header("Dinosaur Configuration")]
    [Tooltip("The dinosaur data for this encounter")]
    public DinosaurData dinosaurData;

    [Header("Scene to Load")]
    [Tooltip("The QTE scene to load (usually 'QTE')")]
    public string qteSceneName = "QTE";

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
                // Validate dinosaur data is assigned
                if (dinosaurData == null)
                {
                    Debug.LogError($"QTETrigger on {gameObject.name} has no DinosaurData assigned!");
                    return;
                }

                // Save current position before leaving overworld
                GameManager.Instance.SavePlayerPosition(player.transform.position);

                // Store the dinosaur data in GameManager so QTE scene can access it
                GameManager.Instance.SetCurrentDinosaurEncounter(dinosaurData);

                // Begin transition to unified QTE scene
                SceneTransition.Instance.TransitionToScene(qteSceneName);
            }
        }
    }
}
