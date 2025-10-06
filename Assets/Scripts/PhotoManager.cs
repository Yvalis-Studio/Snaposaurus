using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DinoPhoto
{
    public string dinoName;
    public Sprite photoSprite;
    public bool isHighQuality; // True si photo nette, false si floue
    public string timestamp;
}

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance { get; private set; }

    [Header("Photo Collection")]
    public List<DinoPhoto> savedPhotos = new List<DinoPhoto>();

    [Header("Display Settings")]
    public Image photoDisplayImage;
    public GameObject photoDisplayPanel;
    public float photoDisplayDuration = 3f;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        // Create photo data
        DinoPhoto newPhoto = new DinoPhoto
        {
            dinoName = dinoName,
            photoSprite = photoSprite,
            isHighQuality = isSuccess,
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Save to collection
        savedPhotos.Add(newPhoto);
        Debug.Log($"Photo saved: {dinoName} - Quality: {(isSuccess ? "High" : "Low")} - Total photos: {savedPhotos.Count}");

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
        if (photoDisplayPanel != null)
        {
            photoDisplayPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Get all saved photos
    /// </summary>
    public List<DinoPhoto> GetAllPhotos()
    {
        return savedPhotos;
    }

    /// <summary>
    /// Get photos of a specific dinosaur
    /// </summary>
    public List<DinoPhoto> GetPhotosByDino(string dinoName)
    {
        return savedPhotos.FindAll(p => p.dinoName == dinoName);
    }
}
