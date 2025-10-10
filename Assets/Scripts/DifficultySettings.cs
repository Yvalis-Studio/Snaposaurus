using UnityEngine;

/// <summary>
/// Manages game difficulty settings that affect QTE parameters
/// </summary>
[DefaultExecutionOrder(-100)] // Ensure this runs before other scripts
public class DifficultySettings : MonoBehaviour
{
    public static DifficultySettings Instance { get; private set; }

    [System.Serializable]
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    [Header("Current Difficulty")]
    [SerializeField]
    private DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    [Header("Easy Settings")]
    [Tooltip("Time multiplier for QTE timer (e.g., 1.5 = 50% more time)")]
    public float easyTimeMultiplier = 1.5f;
    [Tooltip("Key count modifier (e.g., 0 = no change to base keys)")]
    public int easyKeyCountModifier = 0;
    [Tooltip("Countdown duration before QTE starts")]
    public float easyCountdownDuration = 4f;

    [Header("Normal Settings")]
    public float normalTimeMultiplier = 1.0f;
    public int normalKeyCountModifier = 2;
    public float normalCountdownDuration = 3f;

    [Header("Hard Settings")]
    [Tooltip("Time multiplier for QTE timer (e.g., 0.75 = 25% less time)")]
    public float hardTimeMultiplier = 0.75f;
    [Tooltip("Key count modifier (e.g., +4 = 4 more keys to press)")]
    public int hardKeyCountModifier = 4;
    [Tooltip("Countdown duration before QTE starts")]
    public float hardCountdownDuration = 2f;

    [Header("Advanced Settings")]
    [Tooltip("Prevent the same key from appearing consecutively")]
    public bool preventConsecutiveDuplicates = true;

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

    /// <summary>
    /// Get the current difficulty level
    /// </summary>
    public DifficultyLevel GetDifficulty()
    {
        return currentDifficulty;
    }

    /// <summary>
    /// Set the difficulty level
    /// </summary>
    public void SetDifficulty(DifficultyLevel difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log($"Difficulty set to: {difficulty}");
    }

    /// <summary>
    /// Get the time multiplier for the current difficulty
    /// </summary>
    public float GetTimeMultiplier()
    {
        return currentDifficulty switch
        {
            DifficultyLevel.Easy => easyTimeMultiplier,
            DifficultyLevel.Normal => normalTimeMultiplier,
            DifficultyLevel.Hard => hardTimeMultiplier,
            _ => normalTimeMultiplier
        };
    }

    /// <summary>
    /// Get the key count modifier for the current difficulty
    /// </summary>
    public int GetKeyCountModifier()
    {
        return currentDifficulty switch
        {
            DifficultyLevel.Easy => easyKeyCountModifier,
            DifficultyLevel.Normal => normalKeyCountModifier,
            DifficultyLevel.Hard => hardKeyCountModifier,
            _ => normalKeyCountModifier
        };
    }

    /// <summary>
    /// Get the countdown duration for the current difficulty
    /// </summary>
    public float GetCountdownDuration()
    {
        return currentDifficulty switch
        {
            DifficultyLevel.Easy => easyCountdownDuration,
            DifficultyLevel.Normal => normalCountdownDuration,
            DifficultyLevel.Hard => hardCountdownDuration,
            _ => normalCountdownDuration
        };
    }

    /// <summary>
    /// Calculate the adjusted QTE timer based on base time and difficulty
    /// </summary>
    public float CalculateQTETimer(float baseTimer)
    {
        return baseTimer * GetTimeMultiplier();
    }

    /// <summary>
    /// Calculate the adjusted key count based on base length and difficulty
    /// </summary>
    public int CalculateKeyCount(int baseLength)
    {
        int adjustedLength = baseLength + GetKeyCountModifier();
        return Mathf.Max(1, adjustedLength); // Ensure at least 1 key
    }

    /// <summary>
    /// Check if consecutive duplicate keys should be prevented
    /// </summary>
    public bool ShouldPreventConsecutiveDuplicates()
    {
        return preventConsecutiveDuplicates;
    }
}
