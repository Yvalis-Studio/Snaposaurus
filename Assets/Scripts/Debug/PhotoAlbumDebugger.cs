using UnityEngine;

/// <summary>
/// Debug utility for testing photo album functionality.
/// Attach to any GameObject and use hotkeys in Play mode.
/// </summary>
public class PhotoAlbumDebugger : MonoBehaviour
{
    [Header("Debug Keys")]
    [Tooltip("Press to unlock a random photo")]
    public KeyCode unlockRandomPhotoKey = KeyCode.U;

    [Tooltip("Press to clear all photos")]
    public KeyCode clearAllPhotosKey = KeyCode.C;

    [Tooltip("Press to unlock all photos")]
    public KeyCode unlockAllPhotosKey = KeyCode.P;

    [Tooltip("Press to print album status")]
    public KeyCode printStatusKey = KeyCode.I;

    private string[] dinosaurIDs = { "archeo", "triceratops", "brachiosaurus", "godzilla" };

    void Update()
    {
        if (PhotoAlbumData.Instance == null) return;

        // Unlock random photo
        if (Input.GetKeyDown(unlockRandomPhotoKey))
        {
            string randomDino = dinosaurIDs[Random.Range(0, dinosaurIDs.Length)];
            int randomQuality = Random.Range(1, 4); // 1-3
            PhotoAlbumData.Instance.SavePhoto(randomDino, randomQuality);
            Debug.Log($"[DEBUG] Unlocked {randomDino} with quality {randomQuality}");
        }

        // Clear all photos
        if (Input.GetKeyDown(clearAllPhotosKey))
        {
            PhotoAlbumData.Instance.ClearAllPhotos();
            Debug.Log("[DEBUG] Cleared all photos");
        }

        // Unlock all photos
        if (Input.GetKeyDown(unlockAllPhotosKey))
        {
            foreach (string dinoID in dinosaurIDs)
            {
                for (int quality = 1; quality <= 3; quality++)
                {
                    PhotoAlbumData.Instance.SavePhoto(dinoID, quality);
                }
            }
            Debug.Log("[DEBUG] Unlocked all photos (12 total)");
        }

        // Print status
        if (Input.GetKeyDown(printStatusKey))
        {
            PhotoAlbumData.Instance.DebugPrintAllPhotos();
        }
    }

    void OnGUI()
    {
        if (PhotoAlbumData.Instance == null) return;

        GUILayout.BeginArea(new Rect(10, 10, 350, 300));
        GUILayout.Label("=== Photo Album Debugger ===");
        GUILayout.Label($"Completion: {PhotoAlbumData.Instance.GetCompletionPercentage():F0}%");
        GUILayout.Label($"Total Photos: {PhotoAlbumData.Instance.GetTotalUnlockedPhotos()}/12");
        GUILayout.Label($"Complete Collections: {PhotoAlbumData.Instance.GetCompleteCollectionCount()}/4");

        GUILayout.Space(10);
        GUILayout.Label("Controls:");
        GUILayout.Label($"  {unlockRandomPhotoKey}: Unlock random photo");
        GUILayout.Label($"  {clearAllPhotosKey}: Clear all photos");
        GUILayout.Label($"  {unlockAllPhotosKey}: Unlock all photos");
        GUILayout.Label($"  {printStatusKey}: Print detailed status");

        GUILayout.Space(10);
        GUILayout.Label("Status by Dinosaur:");
        foreach (string dinoID in dinosaurIDs)
        {
            int count = PhotoAlbumData.Instance.GetUnlockedPhotoCount(dinoID);
            string status = PhotoAlbumData.Instance.IsCollectionComplete(dinoID) ? "[COMPLETE]" : $"[{count}/3]";
            GUILayout.Label($"  {dinoID}: {status}");
        }

        GUILayout.EndArea();
    }
}