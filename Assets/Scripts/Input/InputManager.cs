using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralized Input Manager - Singleton that manages all input actions and keyboard layout switching
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Action Asset")]
    public InputActionAsset inputActions;

    // Action Maps
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;

    // Player Actions
    public InputAction MoveAction { get; private set; }
    public InputAction LookAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction AttackAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction CrouchAction { get; private set; }
    public InputAction SprintAction { get; private set; }
    public InputAction PauseAction { get; private set; }
    public InputAction PreviousAction { get; private set; }
    public InputAction NextAction { get; private set; }

    // QTE Actions (directional)
    public InputAction UpAction { get; private set; }
    public InputAction DownAction { get; private set; }
    public InputAction LeftAction { get; private set; }
    public InputAction RightAction { get; private set; }

    // UI Actions
    public InputAction NavigateAction { get; private set; }
    public InputAction SubmitAction { get; private set; }
    public InputAction CancelAction { get; private set; }

    // Current keyboard layout
    public enum KeyboardLayout
    {
        QWERTY,
        AZERTY
    }

    [Header("Current Layout")]
    public KeyboardLayout currentLayout = KeyboardLayout.QWERTY;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInputActions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeInputActions()
    {
        Debug.Log("[InputManager] InitializeInputActions() called");

        if (inputActions == null)
        {
            Debug.LogError("[InputManager] InputActionAsset is not assigned!");
            return;
        }

        Debug.Log($"[InputManager] InputActionAsset assigned: {inputActions.name}");

        // Get action maps
        playerActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("UI");

        if (playerActionMap == null || uiActionMap == null)
        {
            Debug.LogError("[InputManager] Could not find Player or UI action maps!");
            return;
        }

        Debug.Log($"[InputManager] Found action maps - Player: {playerActionMap != null}, UI: {uiActionMap != null}");

        // Cache player actions
        MoveAction = playerActionMap.FindAction("Move");
        LookAction = playerActionMap.FindAction("Look");
        JumpAction = playerActionMap.FindAction("Jump");
        AttackAction = playerActionMap.FindAction("Attack");
        InteractAction = playerActionMap.FindAction("Interact");
        CrouchAction = playerActionMap.FindAction("Crouch");
        SprintAction = playerActionMap.FindAction("Sprint");
        PauseAction = playerActionMap.FindAction("Pause");
        PreviousAction = playerActionMap.FindAction("Previous");
        NextAction = playerActionMap.FindAction("Next");

        // For QTE - we'll use composite parts of Move action or individual bindings
        // These will reference the same bindings as Move but accessed individually
        UpAction = MoveAction; // Will read .y > 0
        DownAction = MoveAction; // Will read .y < 0
        LeftAction = MoveAction; // Will read .x < 0
        RightAction = MoveAction; // Will read .x > 0

        // Cache UI actions
        NavigateAction = uiActionMap.FindAction("Navigate");
        SubmitAction = uiActionMap.FindAction("Submit");
        CancelAction = uiActionMap.FindAction("Cancel");

        // Load saved layout preference
        LoadLayoutPreference();

        Debug.Log($"[InputManager] Loaded layout preference: {currentLayout}");

        // Enable player actions by default
        EnablePlayerInput();

        Debug.Log("[InputManager] Initialization complete!");
    }

    public void EnablePlayerInput()
    {
        playerActionMap?.Enable();
    }

    public void DisablePlayerInput()
    {
        playerActionMap?.Disable();
    }

    public void EnableUIInput()
    {
        uiActionMap?.Enable();
    }

    public void DisableUIInput()
    {
        uiActionMap?.Disable();
    }

    /// <summary>
    /// Switch between QWERTY and AZERTY keyboard layouts
    /// </summary>
    public void SwitchLayout(KeyboardLayout newLayout)
    {
        currentLayout = newLayout;

        // Disable specific composite bindings based on layout
        // WASD composite for QWERTY, ZQSD composite for AZERTY
        if (MoveAction != null)
        {
            // In Unity's new Input System, both composites can be active
            // The system will use whichever keys are pressed
            // No need to manually enable/disable them
        }

        // Save preference
        InputSettings.SaveLayout(newLayout);

        // Notify all keybind displays to update
        NotifyLayoutChanged();

        Debug.Log($"InputManager: Switched to {newLayout} layout");
    }

    /// <summary>
    /// Toggle between QWERTY and AZERTY
    /// </summary>
    public void ToggleLayout()
    {
        KeyboardLayout newLayout = (currentLayout == KeyboardLayout.QWERTY)
            ? KeyboardLayout.AZERTY
            : KeyboardLayout.QWERTY;
        SwitchLayout(newLayout);
    }

    /// <summary>
    /// Get the display text for a specific key binding based on current layout
    /// </summary>
    public string GetKeyDisplayText(string actionName)
    {
        InputAction action = playerActionMap?.FindAction(actionName);
        if (action == null) return "";

        // Get the effective binding for current keyboard layout
        var binding = action.bindings[0]; // Simplified - you might want to filter by control scheme
        return InputControlPath.ToHumanReadableString(
            binding.effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }

    /// <summary>
    /// Check if a specific directional input was pressed this frame (for QTE)
    /// </summary>
    public bool WasDirectionPressedThisFrame(string direction)
    {
        if (MoveAction == null) return false;

        Vector2 moveValue = MoveAction.ReadValue<Vector2>();

        switch (direction.ToLower())
        {
            case "up":
                return MoveAction.WasPressedThisFrame() && moveValue.y > 0.5f;
            case "down":
                return MoveAction.WasPressedThisFrame() && moveValue.y < -0.5f;
            case "left":
                return MoveAction.WasPressedThisFrame() && moveValue.x < -0.5f;
            case "right":
                return MoveAction.WasPressedThisFrame() && moveValue.x > 0.5f;
            default:
                return false;
        }
    }

    /// <summary>
    /// Get the key sprite name for a given direction based on current layout
    /// </summary>
    public string GetDirectionKeyName(string direction)
    {
        switch (direction.ToLower())
        {
            case "up":
                return currentLayout == KeyboardLayout.QWERTY ? "w" : "z";
            case "down":
                return "s"; // Same in both layouts
            case "left":
                return currentLayout == KeyboardLayout.QWERTY ? "a" : "q";
            case "right":
                return "d"; // Same in both layouts
            default:
                return "";
        }
    }

    private void LoadLayoutPreference()
    {
        currentLayout = InputSettings.LoadLayout();
    }

    private void NotifyLayoutChanged()
    {
        // Send message to all KeybindDisplay components
        var displays = FindObjectsByType<KeybindDisplay>(FindObjectsSortMode.None);
        foreach (var display in displays)
        {
            display.UpdateDisplay();
        }

        // Also notify QTE managers
        var qteManagers = FindObjectsByType<QTEManager>(FindObjectsSortMode.None);
        foreach (var qte in qteManagers)
        {
            qte.RefreshKeySprites();
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
