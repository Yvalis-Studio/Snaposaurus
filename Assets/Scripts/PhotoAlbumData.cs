using System.Collections.Generic;
using UnityEngine;

// <summary> 
// Manages saving and loading of photo album dta using PlayerPrefs.
// Singleton pattern with DontDestroyOnload for persistence accross scenes.
// </summary>
public class PhotoAlbumData : MonoBehaviour
{
    public static PhotoAlbumData Instance;

    // Known dinosaur IDs in the game
    private List<string> knownDinosaurs = new List<string> { "archeo", "triceratops", "diplo", "godzilla" };

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

    // <summary>
    // Save a photo to the album. Photos are cumulative (you can unlock all 3 qualities separately).
    // </summary>
    // <param name="idnosaurID">Unique dinosaur identifier (e.g., "archeo", "triceratops")</param>
    // <param name="quality">Photo quality: 1= blurry, 2=clear, 3=perfect</param>
    public void SavePhoto(string dinosaurID, int quality) 
    {
        if (string.IsNullOrEmpty(dinosaurID))
        {
            Debug.LogError("[PhotoAlbumData] Connot save photo: dinosaurID is null or empty");
            return;
        }

        if (quality < 1 || quality > 3)
        {
            Debug.LogError($"[PhotoAlbumData] Invalid quality {quality}. Must be 1 (blurry), 2 (clear), or 3 (perfect).");
        }
    }
}