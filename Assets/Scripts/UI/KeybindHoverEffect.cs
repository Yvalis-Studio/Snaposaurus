using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Add this component alongside KeybindDisplay to enable hover and keyboard press feedback
/// Adapted from BackButtonHoverAnimation to work with the new Input System
/// </summary>
[RequireComponent(typeof(KeybindDisplay))]
public class KeybindHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Keyboard Input")]
    [Tooltip("Which key should trigger the pressed state? (e.g., 'w', 'a', 's', 'd', 'space', 'e', 'escape')")]
    [SerializeField] private string monitoredKey = "";

    [Header("Animation (Optional)")]
    [SerializeField] private bool enableHoverAnimation = false;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationSpeed = 10f;

    private KeybindDisplay keybindDisplay;
    private Vector3 originalScale;
    private bool isHovering = false;
    private bool isKeyPressed = false;

    void Awake()
    {
        keybindDisplay = GetComponent<KeybindDisplay>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Check for keyboard input
        CheckKeyboardInput();

        // Optional hover animation
        if (enableHoverAnimation)
        {
            AnimateHover();
        }
    }

    void CheckKeyboardInput()
    {
        if (string.IsNullOrEmpty(monitoredKey)) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        bool keyCurrentlyPressed = IsKeyPressed(keyboard, monitoredKey.ToLower());

        // Update pressed state
        if (keyCurrentlyPressed != isKeyPressed)
        {
            isKeyPressed = keyCurrentlyPressed;
            UpdatePressedState();
        }
    }

    bool IsKeyPressed(Keyboard keyboard, string key)
    {
        return key switch
        {
            "z" => keyboard.zKey.isPressed,
            "q" => keyboard.qKey.isPressed,
            "w" => keyboard.wKey.isPressed,
            "a" => keyboard.aKey.isPressed,
            "s" => keyboard.sKey.isPressed,
            "d" => keyboard.dKey.isPressed,
            "esc" or "escape" => keyboard.escapeKey.isPressed,
            "space" => keyboard.spaceKey.isPressed,
            "e" => keyboard.eKey.isPressed,
            "c" => keyboard.cKey.isPressed,
            "shift" => keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed,
            _ => false
        };
    }

    void UpdatePressedState()
    {
        if (keybindDisplay != null)
        {
            // Show pressed state if key is pressed OR mouse is hovering
            keybindDisplay.SetPressed(isKeyPressed || isHovering);
        }
    }

    void AnimateHover()
    {
        Vector3 targetScale = (isHovering || isKeyPressed) ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdatePressedState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdatePressedState();
    }
}
