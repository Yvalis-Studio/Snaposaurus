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
    public void ShowAndSavePhoto(string dinoName, Sprite photoSprite, bool isSuccess)
    {
        // Display photo
        ShowPhoto(photoSprite);
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
        Debug.Log("hiding photo panel");
        if (photoDisplayPanel != null)
        {
            photoDisplayPanel.SetActive(false);
        }
    }
}
