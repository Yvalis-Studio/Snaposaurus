using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DinosaurQTE Dino1;
    public Vector3 playerPosition;

    [Header("Current Encounter")]
    private DinosaurData currentDinosaurEncounter;
    [Header("Photo Album")]
    public PhotoAlbumData photoAlbumData;

    [Header("Difficulty Settings")]
    [SerializeField]
    private DifficultySettings.DifficultyLevel gameDifficulty = DifficultySettings.DifficultyLevel.Easy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] Instance created and will persist across scenes");

            // Legacy field - only set if assigned
            if (Dino1 != null)
            {
                Dino1.isActive = true;
            }

            // Initialize PhotoAlbumData if not already present
            if (photoAlbumData == null)
            {
                photoAlbumData = gameObject.AddComponent<PhotoAlbumData>();
            }

            // Set difficulty in DifficultySettings if it exists
            if (DifficultySettings.Instance != null)
            {
                DifficultySettings.Instance.SetDifficulty(gameDifficulty);
            }
        }
        else
        {
            Debug.LogWarning("[GameManager] Duplicate GameManager detected and destroyed. Instance remains valid.");
            Destroy(gameObject);
        }
    }

    public void SavePlayerPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public void RestorePlayerPosition(GameObject player)
    {
        player.transform.position = playerPosition;
    }

    public bool IsGamePaused()
    {
        return MenuNavigation.Instance != null && Time.timeScale == 0f;
    }

    /// <summary>
    /// Set the game difficulty level
    /// </summary>
    public void SetDifficulty(DifficultySettings.DifficultyLevel difficulty)
    {
        gameDifficulty = difficulty;
        if (DifficultySettings.Instance != null)
        {
            DifficultySettings.Instance.SetDifficulty(difficulty);
        }
    }

    /// <summary>
    /// Get the current game difficulty level
    /// </summary>
    public DifficultySettings.DifficultyLevel GetDifficulty()
    {
        return gameDifficulty;
    }

    /// <summary>
    /// Store the dinosaur data for the current QTE encounter.
    /// Called by QTETrigger before transitioning to QTE scene.
    /// </summary>
    public void SetCurrentDinosaurEncounter(DinosaurData dinosaurData)
    {
        currentDinosaurEncounter = dinosaurData;
        Debug.Log($"[GameManager] Set current encounter: {dinosaurData.dinosaurName}");
    }

    /// <summary>
    /// Get the dinosaur data for the current QTE encounter.
    /// Called by QTE scene during initialization.
    /// </summary>
    public DinosaurData GetCurrentDinosaurEncounter()
    {
        Debug.Log($"[GameManager] GetCurrentDinosaurEncounter called. Instance is {(Instance != null ? "valid" : "NULL")}");
        if (currentDinosaurEncounter == null)
        {
            Debug.LogWarning("[GameManager] No dinosaur encounter data found! Did you enter QTE scene directly?");
        }
        return currentDinosaurEncounter;
    }

    /// <summary>
    /// Clear the current encounter data (optional cleanup after QTE ends)
    /// </summary>
    public void ClearCurrentEncounter()
    {
        currentDinosaurEncounter = null;
    }
}
