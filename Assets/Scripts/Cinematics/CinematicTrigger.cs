using UnityEngine;

/// <summary>
/// Automatically triggers cinematic playback when the scene loads.
/// Optional component - use if you want auto-play on scene start.
/// Attach to the CinematicManager GameObject.
/// </summary>
public class CinematicTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("Automatically start cinematic on scene load")]
    [SerializeField] private bool playOnStart = true;

    [Tooltip("Delay before starting playback (in seconds)")]
    [SerializeField] private float startDelay = 0.5f;

    [Header("References")]
    [SerializeField] private CinematicManager cinematicManager;

    void Awake()
    {
        // Auto-find CinematicManager if not assigned
        if (cinematicManager == null)
            cinematicManager = GetComponent<CinematicManager>();

        if (cinematicManager == null)
        {
            Debug.LogError("CinematicTrigger: CinematicManager not found on this GameObject!");
            enabled = false;
        }
    }

    void Start()
    {
        if (playOnStart)
        {
            if (startDelay > 0f)
                Invoke(nameof(TriggerCinematic), startDelay);
            else
                TriggerCinematic();
        }
    }

    /// <summary>
    /// Triggers the cinematic playback
    /// </summary>
    public void TriggerCinematic()
    {
        // CinematicManager handles its own Start() playback
        // This is here for manual triggering if needed
        Debug.Log("CinematicTrigger: Cinematic triggered");
    }

    /// <summary>
    /// Public method to skip cinematic (can be called from UI buttons)
    /// </summary>
    public void SkipCinematic()
    {
        if (cinematicManager != null)
            cinematicManager.SkipCinematic();
    }
}
