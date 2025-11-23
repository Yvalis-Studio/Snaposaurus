using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    [Header("Display Settings")]
    public Image photoDisplayImage;
    public GameObject photoDisplayPanel;
    public float photoDisplayDuration = 2f;

    void Start()
    {
        // Hide photo display at start
        if (photoDisplayPanel != null)
        {
            photoDisplayPanel.SetActive(false);
        }
    }

/// <summary>
/// Display and save a photo after QTE
/// </summary>
/// <param name="dinoName">Display name (not used for saving)</param>
/// <param name="photoSprite">The sprite to display</param>
/// <param name="isSuccess">Whether the QTE was successful</param>
/// <param name="isPerfect">Whether the QTE was perfect (3 stars)</param>
/// <param name="dinosaurID">Unique ID for saving (e.g., "archeo")</param>
public void ShowAndSavePhoto(string dinoName, Sprite photoSprite, bool isSuccess, bool isPerfect, string dinosaurID)
{
    // Display photo
    ShowPhoto(photoSprite);

    // Save photo to album
    if (PhotoAlbumData.Instance != null && !string.IsNullOrEmpty(dinosaurID))
    {
        int quality = 0;

        if (isSuccess)
        {
            quality = isPerfect ? 3 : 2; // Perfect = 3, Clear = 2
        }
        else
        {
            quality = 1; // Blurry = 1
        }

        PhotoAlbumData.Instance.SavePhoto(dinosaurID, quality);
        Debug.Log($"[PhotoManager] Saved photo for {dinoName} ({dinosaurID}) with quality {quality}");
    }
    else
    {
        Debug.LogWarning("[PhotoManager] Cannot save photo: PhotoAlbumData.Instance is null or dinosaurID is empty!");
    }
}

    /// <summary>
    /// Display a photo on screen
    /// </summary>
    public void ShowPhoto(Sprite photo)
    {
        if (photoDisplayImage != null && photoDisplayPanel != null)
        {
            photoDisplayImage.sprite = photo;
            photoDisplayPanel.SetActive(true);

            // Auto-hide after duration
            Invoke(nameof(HidePhoto), photoDisplayDuration);
        }
    }

    /// <summary>
    /// Hide the photo display
    /// </summary>
    public void HidePhoto()
    {
        if (photoDisplayPanel != null)
        {
            photoDisplayPanel.SetActive(false);
        }
    }
}
