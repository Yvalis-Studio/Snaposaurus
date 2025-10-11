using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Displays text with a typewriter effect. Can auto-start or be triggered manually from Timeline.
/// Best practice: Use manual trigger with Timeline Signals after panel animations complete.
/// </summary>
public class TypewriterEffect : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [TextArea(3, 10)]
    [SerializeField] private string fullText = "Votre texte ici...";
    [SerializeField] private float delayBetweenChars = 0.05f;

    [Header("Start Behavior")]
    [Tooltip("If true, starts automatically on Start(). If false, call StartTyping() manually.")]
    [SerializeField] private bool autoStart = false;
    [Tooltip("Delay in seconds before starting (only used if autoStart is true)")]
    [SerializeField] private float startDelay = 0f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        // Clear text initially
        if (textComponent != null)
            textComponent.text = "";

        // Auto-start if enabled
        if (autoStart)
        {
            if (startDelay > 0)
                Invoke(nameof(StartTyping), startDelay);
            else
                StartTyping();
        }
    }

    /// <summary>
    /// Starts the typewriter effect. Call this from Timeline Signals or Animation Events.
    /// </summary>
    public void StartTyping()
    {
        if (isTyping)
        {
            Debug.LogWarning($"TypewriterEffect on {gameObject.name} is already typing!");
            return;
        }

        if (textComponent == null)
        {
            Debug.LogError($"TypewriterEffect on {gameObject.name}: TextMeshProUGUI component not assigned!");
            return;
        }

        typingCoroutine = StartCoroutine(ShowText());
    }

    /// <summary>
    /// Stops the typewriter effect and displays the full text immediately.
    /// </summary>
    public void SkipToEnd()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        if (textComponent != null)
            textComponent.text = fullText;
    }

    /// <summary>
    /// Resets the typewriter effect, clearing the text.
    /// </summary>
    public void ResetText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        if (textComponent != null)
            textComponent.text = "";
    }

    private IEnumerator ShowText()
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(delayBetweenChars);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    void OnDestroy()
    {
        // Clean up coroutine if still running
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }
}