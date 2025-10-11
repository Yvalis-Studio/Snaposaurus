# Unity New Input System - Setup Guide

## Overview

This guide explains how to set up and use the QWERTY/AZERTY keyboard layout switching system in Snaposaurus.

---

## ✅ System Status

The input system is **fully implemented and functional**. This guide covers:
- Initial Unity setup requirements
- How the system works
- Configuration in Unity Editor
- Testing procedures

---

## Quick Setup Checklist

### 1. Create InputManager GameObject (First Scene Only)

In your **TitleScreen** scene:

1. Right-click in Hierarchy → Create Empty
2. Name it: `InputManager`
3. Add Component → `InputManager` script
4. In Inspector, assign `Assets/InputSystem_Actions.inputactions` to the **Input Actions** field

**Note:** The InputManager persists across scenes (DontDestroyOnLoad), so it only needs to exist in the first scene.

---

### 2. Configure QTEManager Sprites

The QTE system requires separate sprite sets for QWERTY and AZERTY layouts.

**Find your QTEManager GameObject and assign:**

#### QWERTY Sprites:
- Sprite Up Qwerty → `w.png`
- Sprite Down Qwerty → `s.png`
- Sprite Left Qwerty → `a.png`
- Sprite Right Qwerty → `d.png`

#### AZERTY Sprites:
- Sprite Up Azerty → `z.png`
- Sprite Down Azerty → `s.png`
- Sprite Left Azerty → `q.png`
- Sprite Right Azerty → `d.png`

**Note:** Also assign pressed variants if you have them. If not, you can reuse the normal sprites.

**Sprite Location:** `Assets/Sprites/Keybind/`

---

### 3. Add Layout Toggle Button (Optional)

To let players switch between QWERTY and AZERTY:

1. In your Controls/Settings menu, create a UI Button
2. Name it: `LayoutToggleButton`
3. Add Component → `KeybindLayoutManager` script
4. Assign fields:
   - **Toggle Button** → The button itself
   - **Layout Text** → (Optional) TextMeshPro to display current layout

---

### 4. Add Keybind Display Components (Optional)

To show individual keybinds with auto-updating sprites:

1. Create UI Image for each keybind you want to display
2. Add Component → `KeybindDisplay`
3. Configure:
   - **Key Action** → Action name (e.g., "jump", "up", "down")
   - **QWERTY Sprites** → Normal & pressed sprites
   - **AZERTY Sprites** → Normal & pressed sprites
   - **Supports Pressed State** → Enable for hover/press feedback

---

## How It Works

### System Architecture

**InputManager (Singleton)**
- Lives in TitleScreen, persists across all scenes
- Loads and manages all input actions from `InputSystem_Actions.inputactions`
- Handles QWERTY ↔ AZERTY layout switching
- Saves layout preference to PlayerPrefs
- Notifies UI components when layout changes

**Layout Switching Flow:**
1. User clicks toggle button → `KeybindLayoutManager.OnToggleClicked()`
2. Calls `InputManager.ToggleLayout()`
3. InputManager saves preference and notifies all UI components
4. All `KeybindDisplay` components and QTE sprites update automatically

**Key Mappings:**

| Direction | QWERTY | AZERTY |
|-----------|--------|--------|
| Up        | W      | Z      |
| Down      | S      | S      |
| Left      | A      | Q      |
| Right     | D      | D      |

---

## Testing Checklist

After setup, test the following:

- [ ] **Scene Load** → InputManager created in TitleScreen
- [ ] **Pause Menu** → ESC key opens/closes pause menu
- [ ] **Player Movement** → WASD/ZQSD works in gameplay
- [ ] **Layout Toggle** → Button in Controls menu switches layout
- [ ] **QTE Sprites** → Keys display correctly (W/A/S/D or Z/Q/S/D)
- [ ] **QTE Gameplay** → Correct keys are detected during QTE
- [ ] **Persistence** → Layout preference saved after restart

---

## File Structure

```
Assets/
├── InputSystem_Actions.inputactions
│   └── Contains QWERTY (WASD) and AZERTY (ZQSD) bindings
│
├── Scripts/
│   ├── Input/
│   │   ├── InputManager.cs         - Central input management
│   │   ├── InputSettings.cs        - PlayerPrefs storage
│   │   └── InputDebugger.cs        - Debug helper (optional)
│   │
│   ├── UI/
│   │   ├── KeybindDisplay.cs       - Individual keybind UI component
│   │   └── KeybindLayoutManager.cs - Layout toggle button
│   │
│   ├── MenuNavigation.cs           - Uses InputManager.PauseAction
│   ├── PlayerController.cs         - Uses InputManager for movement
│   ├── PlayerQTE.cs                - Uses InputManager for QTE input
│   └── QTEManager.cs               - Dynamic sprite display
```

---

## API Reference

### Accessing InputManager

```csharp
// Check if InputManager exists
if (InputManager.Instance != null)
{
    // Read movement input
    Vector2 move = InputManager.Instance.MoveAction.ReadValue<Vector2>();

    // Check if jump was pressed
    if (InputManager.Instance.JumpAction.WasPressedThisFrame())
    {
        Jump();
    }

    // Get current layout
    var layout = InputManager.Instance.currentLayout; // QWERTY or AZERTY

    // Toggle layout programmatically
    InputManager.Instance.ToggleLayout();
}
```

### QTE Directional Input

```csharp
// Check specific direction (QTE system)
if (InputManager.Instance.WasDirectionPressedThisFrame("up"))
{
    HandleUpPress();
}

// Get key name for current layout
string keyName = InputManager.Instance.GetDirectionKeyName("up"); // "w" or "z"
```

---

## Troubleshooting

### InputManager not found
- Verify InputManager GameObject exists in TitleScreen scene
- Check that InputManager script is attached
- Verify Input Action Asset is assigned in Inspector

### Movement doesn't work
- Check that InputManager exists in scene
- Ensure PlayerController has no null references
- Check Console for errors

### QTE shows wrong keys
- Verify both QWERTY and AZERTY sprites are assigned in QTEManager
- Check that sprite names match keys (W→w.png, Z→z.png, etc.)

### Layout doesn't persist
- PlayerPrefs are saved automatically
- Check that InputSettings.cs is in the project
- Try deleting PlayerPrefs: `PlayerPrefs.DeleteAll()` then restart

---

## Additional Features

### Input Debugging

Attach `InputDebugger.cs` to any GameObject to:
- Log input events to Console
- Right-click component → "Toggle Layout" to test
- Right-click component → "Print Current Bindings" to see mappings

### Future Enhancements

The system supports future additions:
- **Full rebinding UI** - Let users customize any key
- **More layouts** - Add DVORAK, COLEMAK, etc.
- **Gamepad support** - Already functional in Input Action Asset
- **In-game hints** - Use KeybindDisplay for contextual prompts

---

## Questions?

The system is production-ready. If you encounter any issues, check:
1. Console for error messages
2. InputManager Inspector for missing references
3. This guide's Troubleshooting section
