using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

/// <summary>
/// Core controller for cinematic playback. Handles Timeline playback, skip functionality, and scene transitions.
/// Attach to a GameObject with a PlayableDirector component.
/// </summary>
public class CinematicManager : MonoBehaviour
{
    [Header("Timeline Settings")]
    [SerializeField] private PlayableDirector timelineDirector;

    [Header("UI References")]
    [SerializeField] private GameObject skipPrompt;
    [SerializeField] private CanvasGroup fadePanel;

    [Header("Skip Settings")]
    [SerializeField] private bool allowSkip = true;
    [SerializeField] private float skipPromptDelay = 2f;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName = "Level 1";
    [SerializeField] private bool autoTransitionOnComplete = true;

    private bool hasSkipped = false;
    private float elapsedTime = 0f;

    void Awake()
    {
        // Get PlayableDirector if not assigned
        if (timelineDirector == null)
            timelineDirector = GetComponent<PlayableDirector>();

        // Hide skip prompt initially
        if (skipPrompt != null)
            skipPrompt.SetActive(false);

        // Hide menu canvas if it persists from TitleScreen
        if (MenuNavigation.Instance != null)
            MenuNavigation.Instance.HideCanvasUI();

        // Ensure cursor is visible during cinematics
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Start()
    {
        // Subscribe to Timeline events
        if (timelineDirector != null)
        {
            timelineDirector.stopped += OnTimelineComplete;
            timelineDirector.Play();
        }

        // Fade in from black
        if (fadePanel != null)
            StartCoroutine(FadeIn());
    }

    void Update()
    {
        // Show skip prompt after delay
        if (allowSkip && skipPrompt != null && !skipPrompt.activeSelf)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= skipPromptDelay)
                skipPrompt.SetActive(true);
        }

        // Handle skip input
        if (allowSkip && !hasSkipped)
        {
            bool skipPressed = false;

            #if UNITY_EDITOR
            // In editor, use keyboard directly
            if (Keyboard.current != null)
                skipPressed = Keyboard.current.escapeKey.wasPressedThisFrame ||
                             Keyboard.current.spaceKey.wasPressedThisFrame;
            #else
            // In build, use InputManager if available
            if (InputManager.Instance != null && InputManager.Instance.PauseAction != null)
                skipPressed = InputManager.Instance.PauseAction.WasPressedThisFrame();

            // Fallback to keyboard if InputManager not available
            if (!skipPressed && Keyboard.current != null)
                skipPressed = Keyboard.current.escapeKey.wasPressedThisFrame ||
                             Keyboard.current.spaceKey.wasPressedThisFrame;
            #endif

            if (skipPressed)
            {
                SkipCinematic();
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (timelineDirector != null)
            timelineDirector.stopped -= OnTimelineComplete;
    }

    /// <summary>
    /// Skips the cinematic and proceeds to the next scene
    /// </summary>
    public void SkipCinematic()
    {
        if (hasSkipped) return;

        hasSkipped = true;

        // Stop timeline
        if (timelineDirector != null && timelineDirector.state == PlayState.Playing)
            timelineDirector.Stop();

        // Transition to next scene
        TransitionToNextScene();
    }

    /// <summary>
    /// Called when Timeline playback completes naturally
    /// </summary>
    private void OnTimelineComplete(PlayableDirector director)
    {
        if (autoTransitionOnComplete && !hasSkipped)
        {
            TransitionToNextScene();
        }
    }

    /// <summary>
    /// Transitions to the next scene using SceneTransition
    /// </summary>
    private void TransitionToNextScene()
    {
        if (SceneTransition.Instance != null && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneTransition.Instance.TransitionToScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("CinematicManager: SceneTransition.Instance not found or nextSceneName not set!");
        }
    }

    /// <summary>
    /// Fades in from black at the start of the cinematic
    /// </summary>
    private System.Collections.IEnumerator FadeIn()
    {
        if (fadePanel == null) yield break;

        float duration = 1f;
        float timer = 0f;

        fadePanel.alpha = 1f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }

        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Public method to set the next scene from Timeline Signal or other sources
    /// </summary>
    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
    }
}
