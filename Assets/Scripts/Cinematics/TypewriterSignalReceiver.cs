using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Receives Timeline Signals to control TypewriterEffect.
/// Attach this to the same GameObject as TypewriterEffect, then use Signal Emitters in Timeline.
/// </summary>
[RequireComponent(typeof(TypewriterEffect))]
public class TypewriterSignalReceiver : MonoBehaviour, INotificationReceiver
{
    private TypewriterEffect typewriterEffect;

    void Awake()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();

        if (typewriterEffect == null)
        {
            Debug.LogError($"TypewriterSignalReceiver on {gameObject.name}: No TypewriterEffect component found!");
        }
    }

    /// <summary>
    /// Called by Timeline when a Signal is emitted. Triggers the typewriter effect.
    /// </summary>
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        // You can create custom Signal assets, but for simplicity we respond to any signal
        if (typewriterEffect != null)
        {
            typewriterEffect.StartTyping();
        }
    }
}
