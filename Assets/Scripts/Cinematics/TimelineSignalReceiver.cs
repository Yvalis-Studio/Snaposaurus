using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Receives signals from Timeline and triggers scene transitions or other events.
/// Use with Timeline Signal Emitters to create event-driven cinematics.
/// Attach to the same GameObject as CinematicManager.
/// </summary>
public class TimelineSignalReceiver : MonoBehaviour, INotificationReceiver
{
    [Header("References")]
    [SerializeField] private CinematicManager cinematicManager;

    void Awake()
    {
        // Auto-find CinematicManager if not assigned
        if (cinematicManager == null)
            cinematicManager = GetComponent<CinematicManager>();
    }

    /// <summary>
    /// Called when a Timeline Signal is received
    /// </summary>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        // Handle different signal types
        if (notification is CinematicSignal signal)
        {
            HandleCinematicSignal(signal);
        }
    }

    /// <summary>
    /// Processes cinematic-specific signals
    /// </summary>
    private void HandleCinematicSignal(CinematicSignal signal)
    {
        switch (signal.signalType)
        {
            case CinematicSignalType.LoadScene:
                LoadScene(signal.sceneName);
                break;

            case CinematicSignalType.FadeToBlack:
                // Could trigger fade effect here
                Debug.Log("TimelineSignalReceiver: FadeToBlack signal received");
                break;

            case CinematicSignalType.CustomEvent:
                Debug.Log($"TimelineSignalReceiver: Custom event '{signal.eventName}' triggered");
                break;
        }
    }

    /// <summary>
    /// Loads a scene via SceneTransition
    /// </summary>
    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("TimelineSignalReceiver: Scene name is empty!");
            return;
        }

        Debug.Log($"TimelineSignalReceiver: Loading scene '{sceneName}'");

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.TransitionToScene(sceneName);
        }
        else
        {
            Debug.LogError("TimelineSignalReceiver: SceneTransition.Instance not found!");
        }
    }
}

/// <summary>
/// Signal types for cinematics
/// </summary>
public enum CinematicSignalType
{
    LoadScene,
    FadeToBlack,
    CustomEvent
}

/// <summary>
/// Custom signal for Timeline. Create via Assets > Create > Signals > Cinematic Signal
/// </summary>
[System.Serializable]
public class CinematicSignal : SignalAsset
{
    [Header("Signal Settings")]
    public CinematicSignalType signalType = CinematicSignalType.LoadScene;

    [Header("Scene Loading")]
    [Tooltip("Name of the scene to load (for LoadScene signal type)")]
    public string sceneName = "Level 1";

    [Header("Custom Event")]
    [Tooltip("Custom event name (for CustomEvent signal type)")]
    public string eventName = "";
}
