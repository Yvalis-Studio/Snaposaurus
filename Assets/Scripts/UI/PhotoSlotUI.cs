using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Represents a single photo slot in the album.
/// Shows either the unlocked photo or a blurred locked version.
/// Designed for fan/spread layout with rotation.
/// Brings photo to front on hover if unlocked.
/// </summary>
public class PhotoSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Elements")]
    public Image photoImage; // The main image component

    [Header("Sprites")]
    public Sprite unlockedSprite; // The actual photo (assign in Inspector or via code)
    public Sprite lockedSprite;   // Blurred version (assign in Inspector or via code)

    [Header("Hover Effect")]
    public float hoverScale = 1.1f; // Scale when hovering (default 10% bigger)
    public int hoverSiblingIndex = 100; // High number to bring to front

    private bool isUnlocked = false;
    private int originalSiblingIndex;
    private Vector3 originalScale;

    void Awake()
    {
        // Store original transform values
        originalSiblingIndex = transform.GetSiblingIndex();
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Setup the photo slot with unlock status.
    /// </summary>
    /// <param name="unlocked">Whether this photo is unlocked</param>
    public void Setup(bool unlocked)
    {
        if (photoImage == null)
        {
            Debug.LogError("[PhotoSlotUI] photoImage is null! Assign it in Inspector.");
            return;
        }

        isUnlocked = unlocked;

        if (unlocked && unlockedSprite != null)
        {
            // Show unlocked photo
            photoImage.sprite = unlockedSprite;
            photoImage.color = Color.white; // Full color
        }
        else if (lockedSprite != null)
        {
            // Show locked/blurred photo
            photoImage.sprite = lockedSprite;
            photoImage.color = new Color(1f, 1f, 1f, 0.7f); // Slightly transparent
        }
        else
        {
            // Fallback: show nothing or error sprite
            photoImage.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark placeholder
            Debug.LogWarning($"[PhotoSlotUI] Missing sprites for slot {gameObject.name}");
        }
    }

    /// <summary>
    /// Called when mouse enters the photo slot.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only show hover effect if photo is unlocked
        if (!isUnlocked) return;

        // Bring to front by setting as last sibling
        transform.SetAsLastSibling();

        // Scale up slightly
        transform.localScale = originalScale * hoverScale;
    }

    /// <summary>
    /// Called when mouse exits the photo slot.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Only reset if photo is unlocked
        if (!isUnlocked) return;

        // Return to original position in hierarchy
        transform.SetSiblingIndex(originalSiblingIndex);

        // Reset scale
        transform.localScale = originalScale;
    }

    /// <summary>
    /// Setup with explicit sprites (alternative to Inspector assignment).
    /// </summary>
    public void Setup(bool isUnlocked, Sprite unlocked, Sprite locked)
    {
        unlockedSprite = unlocked;
        lockedSprite = locked;
        Setup(isUnlocked);
    }
}