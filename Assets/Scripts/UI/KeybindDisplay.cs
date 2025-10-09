using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component that displays a keybind sprite and automatically updates when layout changes
/// Attach this to a UI Image GameObject
/// </summary>
[RequireComponent(typeof(Image))]
public class KeybindDisplay : MonoBehaviour
{
    [Header("Key to Display")]
    [Tooltip("The action this keybind represents (e.g., 'up', 'down', 'jump', 'interact')")]
    public string keyAction;

    [Header("Sprite Sets")]
    [Tooltip("Normal state sprites for QWERTY layout")]
    public KeybindSprites qwertySprites;

    [Tooltip("Normal state sprites for AZERTY layout")]
    public KeybindSprites azertySprites;

    [Header("Visual States")]
    [Tooltip("Should this display show pressed/hover state?")]
    public bool supportsPressedState = false;

    private Image image;
    private bool isPressed = false;

    [System.Serializable]
    public class KeybindSprites
    {
        public Sprite normalSprite;
        public Sprite pressedSprite;
    }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Update the displayed sprite based on current layout and pressed state
    /// </summary>
    public void UpdateDisplay()
    {
        if (image == null)
        {
            Debug.LogWarning($"[KeybindDisplay] Image is NULL on {gameObject.name}");
            return;
        }

        if (InputManager.Instance == null)
        {
            Debug.LogWarning($"[KeybindDisplay] InputManager.Instance is NULL on {gameObject.name}");
            return;
        }

        // Get the appropriate sprite set based on current layout
        KeybindSprites currentSpriteSet = InputManager.Instance.currentLayout == InputManager.KeyboardLayout.QWERTY
            ? qwertySprites
            : azertySprites;

        if (currentSpriteSet == null) return;

        // Choose normal or pressed sprite
        Sprite spriteToDisplay = (supportsPressedState && isPressed)
            ? currentSpriteSet.pressedSprite
            : currentSpriteSet.normalSprite;

        if (spriteToDisplay != null)
        {
            image.sprite = spriteToDisplay;
        }
    }

    /// <summary>
    /// Set the pressed state and update display
    /// </summary>
    public void SetPressed(bool pressed)
    {
        if (!supportsPressedState) return;

        isPressed = pressed;
        UpdateDisplay();
    }

    /// <summary>
    /// Flash the pressed state briefly (useful for feedback)
    /// </summary>
    public void Flash(float duration = 0.2f)
    {
        if (!supportsPressedState) return;

        SetPressed(true);
        Invoke(nameof(ResetPressed), duration);
    }

    private void ResetPressed()
    {
        SetPressed(false);
    }
}
