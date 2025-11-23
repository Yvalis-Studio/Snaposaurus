using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Initializes the unified QTE scene with the correct dinosaur data.
/// Place this script on a GameObject in your QTE scene.
/// </summary>
public class QTESceneInitializer : MonoBehaviour
{
    [Header("References to Update")]
    [Tooltip("The DinosaurQTE component to configure")]
    public DinosaurQTE dinosaurQTE;

    [Tooltip("The Image/SpriteRenderer that displays the dinosaur sprite")]
    public Image dinosaurImage; // If using UI Image
    public SpriteRenderer dinosaurSpriteRenderer; // If using world space sprite

    [Header("Optional: Player Sprite")]
    [Tooltip("The Image/SpriteRenderer for the player (if you want to override it)")]
    public Image playerImage;
    public SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        InitializeQTE();
    }

    /// <summary>
    /// Initialize the QTE scene with dinosaur data from GameManager
    /// </summary>
    public void InitializeQTE()
    {
        // Get the dinosaur data from GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("[QTESceneInitializer] GameManager.Instance is NULL!");
            return;
        }

        DinosaurData dinosaurData = GameManager.Instance.GetCurrentDinosaurEncounter();

        if (dinosaurData == null)
        {
            Debug.LogError("[QTESceneInitializer] No dinosaur data found in GameManager!");
            return;
        }

        Debug.Log($"[QTESceneInitializer] Initializing QTE for: {dinosaurData.dinosaurName}");

        // Configure DinosaurQTE component with the data
        if (dinosaurQTE != null)
        {
            dinosaurQTE.dinoName = dinosaurData.dinosaurName;
            dinosaurQTE.baseTimeLimit = dinosaurData.baseTimeLimit;
            dinosaurQTE.baseKeyCount = dinosaurData.baseKeyCount;
            dinosaurQTE.perfectPhoto = dinosaurData.perfectPhoto;
            dinosaurQTE.clearPhoto = dinosaurData.clearPhoto;
            dinosaurQTE.blurryPhoto = dinosaurData.blurryPhoto;
            dinosaurQTE.dinosaurData = dinosaurData; // NEW: Assign the full DinosaurData reference for photo album

            Debug.Log($"[QTESceneInitializer] Configured DinosaurQTE: {dinosaurData.dinosaurName}, Keys: {dinosaurData.baseKeyCount}, Time: {dinosaurData.baseTimeLimit}s");
        }
        else
        {
            Debug.LogWarning("[QTESceneInitializer] DinosaurQTE reference is NULL!");
        }

        // Update dinosaur visual sprite
        if (dinosaurData.dinosaurSprite != null)
        {
            Debug.Log($"[QTESceneInitializer] Trying to set dinosaur sprite: {dinosaurData.dinosaurSprite.name}");

            if (dinosaurImage != null)
            {
                dinosaurImage.sprite = dinosaurData.dinosaurSprite;
                Debug.Log($"[QTESceneInitializer] ✓ Successfully set dinosaur sprite '{dinosaurData.dinosaurSprite.name}' on UI Image '{dinosaurImage.gameObject.name}'");
            }
            else if (dinosaurSpriteRenderer != null)
            {
                dinosaurSpriteRenderer.sprite = dinosaurData.dinosaurSprite;
                Debug.Log($"[QTESceneInitializer] ✓ Successfully set dinosaur sprite '{dinosaurData.dinosaurSprite.name}' on SpriteRenderer '{dinosaurSpriteRenderer.gameObject.name}'");
            }
            else
            {
                Debug.LogError("[QTESceneInitializer] ✗ No dinosaur Image or SpriteRenderer assigned! Cannot change dinosaur sprite.");
            }
        }
        else
        {
            Debug.LogWarning($"[QTESceneInitializer] DinosaurData '{dinosaurData.dinosaurName}' has no dinosaurSprite assigned!");
        }

        // Optional: Update player sprite if custom one is provided
        if (dinosaurData.customPlayerSprite != null)
        {
            if (playerImage != null)
            {
                playerImage.sprite = dinosaurData.customPlayerSprite;
            }
            else if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.sprite = dinosaurData.customPlayerSprite;
            }
        }

        // Optional: Instantiate dinosaur prefab if provided instead of using sprite
        if (dinosaurData.dinosaurPrefab != null)
        {
            // You could instantiate the prefab at a specific position
            // GameObject dinoInstance = Instantiate(dinosaurData.dinosaurPrefab, dinoSpawnPoint.position, Quaternion.identity);
            Debug.LogWarning("[QTESceneInitializer] Dinosaur prefab instantiation not implemented yet. Using sprite instead.");
        }
    }
}
