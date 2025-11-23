using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages saving and loading of photo album data using PlayerPrefs.
/// Singleton pattern with DontDestroyOnLoad for persistence across scenes.
/// </summary>
public class PhotoAlbumData : MonoBehaviour
{
    public static PhotoAlbumData Instance;

    [Header("Dinosaur Configuration")]
    [SerializeField]
    [Tooltip("List of all known dinosaur IDs in the game. Add new dinosaurs here.")]
    private List<string> knownDinosaurs = new List<string> { "archeo", "triceratops", "brachiosaurus", "godzilla" };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PhotoAlbumData] Instance Created and persisted");
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Save a photo to the album. Photos are cumulative (you can unlock all 3 qualities separately).
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier (e.g., "archeo", "triceratops")</param>
    /// <param name="quality">Photo quality: 1=blurry, 2=clear, 3=perfect</param>
    public void SavePhoto(string dinosaurID, int quality)
    {
        if (string.IsNullOrEmpty(dinosaurID))
        {
            Debug.LogError("[PhotoAlbumData] Cannot save photo: dinosaurID is null or empty");
            return;
        }

        if (quality < 1 || quality > 3)
        {
            Debug.LogError($"[PhotoAlbumData] Invalid quality {quality}. Must be 1 (blurry), 2 (clear), or 3 (perfect).");
            return;
        }

        string qualityName = GetQualityName(quality);
        string key = $"photo_{dinosaurID}_{qualityName}";

        // Check if already unlocked
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            Debug.Log($"[PhotoAlbumData] {dinosaurID} {qualityName} photo already unlocked");
            return;
        }

        // Unlock this quality
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        Debug.Log($"[PhotoAlbumData] Unlocked {dinosaurID} {qualityName} photo !");
    }

    /// <summary>
    /// Check if a specific quality photo is unlocked
    /// </summary>
    /// <param name="dinosaurID"> Unique dinosaur identifier</param>
    /// <param name="quality">Photo quality: 1=blurry, 2=clear, 3=perfect</param>
    /// <returns>True if this specific quality is unlocked</returns>
    public bool IsPhotoUnlocked(string dinosaurID, int quality)
    {
        if (string.IsNullOrEmpty(dinosaurID) || quality < 1 || quality > 3)
        {
            return false;
        }

        string qualityName = GetQualityName(quality);
        string key = $"photo_{dinosaurID}_{qualityName}";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    /// <summary>
    /// Get the number of unlocked photos for a specific dinosaur (0-3).
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <returns>Count of unlocked photos (0-3)</returns>
    public int GetUnlockedPhotoCount(string dinosaurID)
    {
        int count = 0;
        for (int quality = 1; quality <= 3; quality++)
        {
            if (IsPhotoUnlocked(dinosaurID, quality))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Check if at least one photo exists for a dinosaur.
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <returns>True if any photo quality is unlocked</returns>
    public bool HasAnyPhoto(string dinosaurID)
    {
        return GetUnlockedPhotoCount(dinosaurID) > 0;
    }

        /// <summary>
    /// Check if all 3 photos are unlocked for a dinosaur (complete collection).
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <returns>True if all 3 qualities are unlocked</returns>
    public bool IsCollectionComplete(string dinosaurID)
    {
        return GetUnlockedPhotoCount(dinosaurID) == 3;
    }

    /// <summary>
    /// Get all dinosaur IDs that have at least one photo saved.
    /// </summary>
    /// <returns>List of dinosaur IDs with at least one photo</returns>
    public List<string> GetAllSavedDinosaurs()
    {
        List<string> savedDinosaurs = new List<string>();

        foreach (string dinoID in knownDinosaurs)
        {
            if (HasAnyPhoto(dinoID))
            {
                savedDinosaurs.Add(dinoID);
            }
        }

        return savedDinosaurs;
    }

    /// <summary>
    /// Clear all saved photos (for testing or reset functionality).
    /// </summary>
    public void ClearAllPhotos()
    {
        foreach (string dinoID in knownDinosaurs)
        {
            for (int quality = 1; quality <= 3; quality++)
            {
                string qualityName = GetQualityName(quality);
                string key = $"photo_{dinoID}_{qualityName}";
                if (PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }
        }

        PlayerPrefs.Save();
        Debug.Log("[PhotoAlbumData] Cleared all saved photos");
    }

    /// <summary>
    /// Get completion percentage based on total possible photos (12 = 4 dinos x 3 qualities).
    /// </summary>
    /// <returns>Percentage of all photos collected (0-100)</returns>
    public float GetCompletionPercentage()
    {
        int totalPossiblePhotos = knownDinosaurs.Count * 3; // 4 dinos x 3 qualities = 12
        int unlockedPhotos = 0;

        foreach (string dinoID in knownDinosaurs)
        {
            unlockedPhotos += GetUnlockedPhotoCount(dinoID);
        }

        return totalPossiblePhotos > 0 ? (float)unlockedPhotos / totalPossiblePhotos * 100f : 0f;
    }

    /// <summary>
    /// Get total count of unlocked photos across all dinosaurs.
    /// </summary>
    /// <returns>Total number of unlocked photos (0-12)</returns>
    public int GetTotalUnlockedPhotos()
    {
        int total = 0;
        foreach (string dinoID in knownDinosaurs)
        {
            total += GetUnlockedPhotoCount(dinoID);
        }
        return total;
    }

    /// <summary>
    /// Get count of dinosaurs with complete collections (all 3 qualities).
    /// </summary>
    /// <returns>Number of dinosaurs with complete collections (0-4)</returns>
    public int GetCompleteCollectionCount()
    {
        int count = 0;
        foreach (string dinoID in knownDinosaurs)
        {
            if (IsCollectionComplete(dinoID))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Helper method to convert quality number to string.
    /// </summary>
    private string GetQualityName(int quality)
    {
        switch (quality)
        {
            case 1: return "blurry";
            case 2: return "clear";
            case 3: return "perfect";
            default: return "unknown";
        }
    }

    /// <summary>
    /// Debug method to print all saved photos.
    /// </summary>
    public void DebugPrintAllPhotos()
    {
        Debug.Log("=== PHOTO ALBUM STATUS ===");
        foreach (string dinoID in knownDinosaurs)
        {
            int count = GetUnlockedPhotoCount(dinoID);
            string status = IsCollectionComplete(dinoID) ? "[COMPLETE]" : $"[{count}/3]";
            Debug.Log($"{dinoID} {status}:");
            Debug.Log($"  Blurry: {(IsPhotoUnlocked(dinoID, 1) ? "✓" : "✗")}");
            Debug.Log($"  Clear: {(IsPhotoUnlocked(dinoID, 2) ? "✓" : "✗")}");
            Debug.Log($"  Perfect: {(IsPhotoUnlocked(dinoID, 3) ? "✓" : "✗")}");
        }
        Debug.Log($"Total: {GetTotalUnlockedPhotos()}/12 photos ({GetCompletionPercentage():F1}%)");
    }
}