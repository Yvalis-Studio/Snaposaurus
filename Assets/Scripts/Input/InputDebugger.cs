using UnityEngine;

/// <summary>
/// Optional debug helper to test input system in play mode
/// Attach to any GameObject to see debug info in console
/// </summary>
public class InputDebugger : MonoBehaviour
{
    [Header("Settings")]
    public bool logInputEvents = true;
    public bool logLayoutChanges = true;

    private InputManager.KeyboardLayout lastLayout;

    void Start()
    {
        if (InputManager.Instance != null)
        {
            lastLayout = InputManager.Instance.currentLayout;
            Debug.Log($"[InputDebugger] Started with layout: {lastLayout}");
        }
    }

    void Update()
    {
        if (InputManager.Instance == null) return;

        // Check for layout changes
        if (logLayoutChanges && InputManager.Instance.currentLayout != lastLayout)
        {
            lastLayout = InputManager.Instance.currentLayout;
            Debug.Log($"[InputDebugger] Layout changed to: {lastLayout}");
        }

        // Log input events
        if (logInputEvents)
        {
            if (InputManager.Instance.MoveAction.WasPressedThisFrame())
            {
                Vector2 move = InputManager.Instance.MoveAction.ReadValue<Vector2>();
                Debug.Log($"[InputDebugger] Move pressed: {move}");
            }

            if (InputManager.Instance.JumpAction.WasPressedThisFrame())
            {
                Debug.Log($"[InputDebugger] Jump pressed");
            }

            if (InputManager.Instance.PauseAction.WasPressedThisFrame())
            {
                Debug.Log($"[InputDebugger] Pause pressed");
            }
        }
    }

    // Test function you can call from Inspector or console
    [ContextMenu("Toggle Layout")]
    public void TestToggleLayout()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ToggleLayout();
            Debug.Log($"[InputDebugger] Toggled to: {InputManager.Instance.currentLayout}");
        }
    }

    [ContextMenu("Print Current Bindings")]
    public void PrintCurrentBindings()
    {
        if (InputManager.Instance == null) return;

        Debug.Log($"=== Current Input Bindings ({InputManager.Instance.currentLayout}) ===");
        Debug.Log($"Up: {InputManager.Instance.GetDirectionKeyName("up")}");
        Debug.Log($"Down: {InputManager.Instance.GetDirectionKeyName("down")}");
        Debug.Log($"Left: {InputManager.Instance.GetDirectionKeyName("left")}");
        Debug.Log($"Right: {InputManager.Instance.GetDirectionKeyName("right")}");
    }
}
