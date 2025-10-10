using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DinosaurQTE Dino1;
    public Vector3 playerPosition;

    [Header("Difficulty Settings")]
    [SerializeField]
    private DifficultySettings.DifficultyLevel gameDifficulty = DifficultySettings.DifficultyLevel.Easy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Dino1.isActive = true;

            // Set difficulty in DifficultySettings if it exists
            if (DifficultySettings.Instance != null)
            {
                DifficultySettings.Instance.SetDifficulty(gameDifficulty);
            }
        }
        else
        {
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
}
