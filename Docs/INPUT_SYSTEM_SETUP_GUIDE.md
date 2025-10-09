# Unity New Input System - Setup Guide

## ✅ Code Changes Complete!

All scripts have been updated to use the new Input System. Here's what you need to do in Unity:

---

## Step 1: Create InputManager GameObject

1. **In your TitleScreen scene:**
   - Right-click in Hierarchy → Create Empty
   - Name it: `InputManager`
   - Add Component → `InputManager` script

2. **Assign the Input Action Asset:**
   - In Inspector, find "Input Actions" field
   - Drag `Assets/InputSystem_Actions.inputactions` into this field

3. **IMPORTANT:** The InputManager is set to DontDestroyOnLoad, so it only needs to exist in the first scene (TitleScreen)

---

## Step 2: Update Player GameObject (in Level scenes)

**Find your Player GameObject and:**

1. **Remove old InputAction fields** (in Inspector):
   - Look at PlayerController component
   - The fields `moveAction`, `jumpAction`, `climbAction`, `interactAction` are now gone
   - ✅ No action needed - just verify they're gone after Unity recompiles

---

## Step 3: Update PlayerQTE GameObject (in QTE scenes)

**Find your PlayerQTE GameObject and:**

1. **Remove old InputAction fields** (in Inspector):
   - Look at PlayerQTE component
   - The fields `up`, `down`, `left`, `right` are now gone
   - ✅ No action needed - just verify they're gone after Unity recompiles

---

## Step 4: Setup QTEManager Sprites

**Find your QTEManager GameObject and:**

1. **You now have TWO sprite sets** (QWERTY and AZERTY):
   - Old fields like `spriteUp`, `spriteDown` are REPLACED with:
   - `spriteUpQwerty` / `spriteUpAzerty`
   - `spriteDownQwerty` / `spriteDownAzerty`
   - `spriteLeftQwerty` / `spriteLeftAzerty`
   - `spriteRightQwerty` / `spriteRightAzerty`
   - Same for pressed variants

2. **Assign sprites based on key:**
   - **QWERTY Up** → W key sprite
   - **QWERTY Down** → S key sprite
   - **QWERTY Left** → A key sprite
   - **QWERTY Right** → D key sprite
   - **AZERTY Up** → Z key sprite
   - **AZERTY Down** → S key sprite
   - **AZERTY Left** → Q key sprite
   - **AZERTY Right** → D key sprite

3. **Your sprite locations:**
   - Find them in: `Assets/Sprites/Keybind/`
   - Available: `w.png`, `a.png`, `s.png`, `d.png`, `z.png`, `q.png`

---

## Step 5: Add Layout Toggle to Controls Menu

**In your Controls Panel:**

1. **Create Toggle Button:**
   - Right-click ControlsPanel → UI → Button
   - Name it: `LayoutToggleButton`
   - Add Component → `KeybindLayoutManager` script

2. **Assign fields in KeybindLayoutManager:**
   - `Toggle Button` → Drag the button itself
   - `Layout Text` → (Optional) Create a TextMeshPro text to show "Current Layout: QWERTY"

3. **Position it nicely in your Controls UI**

---

## Step 6: (Optional) Add Individual Keybind Displays

**To show keybinds in menus with auto-updating sprites:**

1. **Create UI Image:**
   - Right-click → UI → Image
   - Name it descriptively (e.g., "JumpKeyDisplay")

2. **Add KeybindDisplay component:**
   - Add Component → `KeybindDisplay`

3. **Configure it:**
   - `Key Action` → Type the action name (e.g., "jump", "up", "down")
   - `QWERTY Sprites` → Assign normal & pressed sprites for QWERTY
   - `AZERTY Sprites` → Assign normal & pressed sprites for AZERTY
   - `Supports Pressed State` → Check if you want hover/press feedback

4. **Repeat for all keys you want to display visually**

---

## Step 7: Test Everything

### Test Checklist:

1. ✅ **Play TitleScreen** → InputManager should be created
2. ✅ **Press ESC** → Pause menu should work
3. ✅ **Enter Level** → Player movement works (WASD or ZQSD)
4. ✅ **Open Controls Menu** → Click layout toggle button
5. ✅ **Verify sprite changes** → QTE keys should show W/A/S/D or Z/Q/S/D
6. ✅ **Play QTE** → Keys match the displayed sprites
7. ✅ **Toggle layout** → All keybind displays update automatically
8. ✅ **Restart game** → Layout preference persists (saved to PlayerPrefs)

---

## Step 8: Cleanup Old References (After Testing)

**Once everything works, you can clean up:**

1. No old scripts to delete - we only modified existing ones
2. Old InputAction fields in Inspector will automatically disappear after recompile

---

## Troubleshooting

### Error: "InputManager does not exist"
- **Solution:** Go back to Unity and let it recompile
- **Check:** Make sure `InputManager.cs` is in `Assets/Scripts/Input/`

### Movement doesn't work
- **Check:** InputManager GameObject exists in scene
- **Check:** Input Action Asset is assigned in InputManager Inspector
- **Check:** PlayerController is using new code (no more individual InputAction fields)

### QTE shows wrong keys
- **Check:** QTEManager has both QWERTY and AZERTY sprites assigned
- **Check:** Sprite names match the keys (W for up in QWERTY, Z for up in AZERTY)

### Layout toggle doesn't work
- **Check:** KeybindLayoutManager is attached to the button
- **Check:** Toggle Button field is assigned
- **Check:** InputManager exists in scene

### Sprites don't update when toggling
- **Check:** KeybindDisplay components exist
- **Check:** Both sprite sets (QWERTY and AZERTY) are assigned

---

## What Changed (Summary)

### New Files Created:
- `Assets/Scripts/Input/InputManager.cs` - Central input management
- `Assets/Scripts/Input/InputSettings.cs` - Persistent layout storage
- `Assets/Scripts/UI/KeybindDisplay.cs` - Individual keybind sprite component
- `Assets/Scripts/UI/KeybindLayoutManager.cs` - Layout toggle button

### Modified Files:
- `Assets/InputSystem_Actions.inputactions` - Added AZERTY bindings + Pause action
- `Assets/Scripts/MenuNavigation.cs` - Uses InputManager.PauseAction
- `Assets/Scripts/PlayerController.cs` - Uses InputManager actions
- `Assets/Scripts/PlayerQTE.cs` - Uses InputManager directional detection
- `Assets/Scripts/QTEManager.cs` - Supports QWERTY/AZERTY sprite sets

### Inspector Changes You Need to Make:
1. Create InputManager GameObject (TitleScreen)
2. Assign Input Action Asset to InputManager
3. Update QTEManager sprite assignments (QWERTY/AZERTY sets)
4. Create Layout Toggle button with KeybindLayoutManager
5. (Optional) Add KeybindDisplay components to menu UI

---

## Next Steps

1. Open Unity and let it compile
2. Follow Step 1-7 above
3. Test thoroughly
4. Let me know if you encounter any issues!

The system is designed to:
- ✅ Auto-detect keyboard layout preference
- ✅ Persist choice across sessions
- ✅ Update all UI sprites automatically when toggling
- ✅ Work with both menu displays and in-game QTEs
- ✅ Support normal + pressed sprite states
