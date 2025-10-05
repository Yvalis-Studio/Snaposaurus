using UnityEditor;
using UnityEngine;

public class QTETrigger : MonoBehaviour
{
    public SceneAsset targetScene;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.Dino1.isActive)
        {
            // Save current position before leaving overworld
            GameManager.Instance.SavePlayerPosition(other.transform.position);
            GameManager.Instance.Dino1.isActive = false;

            // Begin transition
            SceneTransition.Instance.TransitionToScene(targetScene.name);
        }
    }
}
