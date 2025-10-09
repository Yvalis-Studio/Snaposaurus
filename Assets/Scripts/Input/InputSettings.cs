using UnityEngine;

/// <summary>
/// Persistent storage for input settings (keyboard layout preference)
/// </summary>
public static class InputSettings
{
    private const string LAYOUT_PREF_KEY = "KeyboardLayout";

    /// <summary>
    /// Save the current keyboard layout preference
    /// </summary>
    public static void SaveLayout(InputManager.KeyboardLayout layout)
    {
        PlayerPrefs.SetInt(LAYOUT_PREF_KEY, (int)layout);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load the saved keyboard layout preference (defaults to QWERTY)
    /// </summary>
    public static InputManager.KeyboardLayout LoadLayout()
    {
        if (PlayerPrefs.HasKey(LAYOUT_PREF_KEY))
        {
            return (InputManager.KeyboardLayout)PlayerPrefs.GetInt(LAYOUT_PREF_KEY);
        }
        return InputManager.KeyboardLayout.QWERTY; // Default
    }

    /// <summary>
    /// Reset to default layout (QWERTY)
    /// </summary>
    public static void ResetLayout()
    {
        SaveLayout(InputManager.KeyboardLayout.QWERTY);
    }
}
