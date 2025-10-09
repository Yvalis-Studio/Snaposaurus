using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages the keyboard layout toggle button in the Controls menu
/// </summary>
public class KeybindLayoutManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Button to toggle between QWERTY and AZERTY")]
    public Button toggleButton;

    [Tooltip("Text showing current layout (optional)")]
    public TextMeshProUGUI layoutText;

    [Header("Button Text")]
    public string qwertyButtonText = "Switch to AZERTY";
    public string azertyButtonText = "Switch to QWERTY";

    void Start()
    {
        // Setup button listener
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(OnToggleLayout);
        }

        // Update UI to reflect current layout
        UpdateLayoutDisplay();
    }

    /// <summary>
    /// Called when toggle button is clicked
    /// </summary>
    public void OnToggleLayout()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ToggleLayout();
            UpdateLayoutDisplay();
            Debug.Log($"[KeybindLayoutManager] Layout toggled to: {InputManager.Instance.currentLayout}");
        }
        else
        {
            Debug.LogWarning("[KeybindLayoutManager] InputManager.Instance is NULL! Cannot toggle layout.");
        }
    }

    /// <summary>
    /// Update the button text and layout display to show current state
    /// </summary>
    public void UpdateLayoutDisplay()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogWarning("[KeybindLayoutManager] InputManager.Instance is NULL! Cannot update display.");
            return;
        }

        InputManager.KeyboardLayout currentLayout = InputManager.Instance.currentLayout;

        // Update button text to show what will happen when clicked
        if (toggleButton != null)
        {
            var buttonText = toggleButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = currentLayout == InputManager.KeyboardLayout.QWERTY
                    ? qwertyButtonText
                    : azertyButtonText;
            }
        }

        // Update layout display text
        if (layoutText != null)
        {
            layoutText.text = $"Current Layout: {currentLayout}";
        }
    }

    void OnDestroy()
    {
        // Cleanup listener
        if (toggleButton != null)
        {
            toggleButton.onClick.RemoveListener(OnToggleLayout);
        }
    }
}
