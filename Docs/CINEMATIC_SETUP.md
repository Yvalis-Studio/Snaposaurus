# Cinematic System Setup Guide

## Overview
This guide will help you set up your intro cinematic using the modular cinematic system.

---

## Part 1: Scene Setup

### 1. Create the Cinematic Scene

1. Open Unity Editor
2. Create a new scene: `File > New Scene` (Basic/Empty template)
3. Save as: `Assets/Scenes/Cinematics/IntroCinematic.unity`

---

## Part 2: Create Timeline Asset

### 1. Create the Timeline

1. In Project window, navigate to `Assets/Timelines/`
2. Right-click > `Create > Timeline`
3. Name it: `IntroCinematic`

---

## Part 3: Build the GameObject Hierarchy

### 1. Create CinematicManager GameObject

In your `IntroCinematic` scene Hierarchy:

```
IntroCinematic (Scene)
├── CinematicManager          (Empty GameObject)
│   ├── Main Camera           (Camera)
│   └── CinematicCanvas       (UI Canvas)
│       ├── FadePanel         (UI Image - black)
│       └── SkipPrompt        (UI Text/TextMeshPro)
```

### 2. Setup CinematicManager GameObject

1. Create empty GameObject: `GameObject > Create Empty`
2. Name it: `CinematicManager`
3. Add components:
   - `Playable Director` component
   - `Cinemachine Brain` component (optional, for camera effects)
   - `CinematicManager` script
   - `TimelineSignalReceiver` script
   - `CinematicTrigger` script

### 3. Setup Main Camera

1. Create Camera: `GameObject > Camera`
2. Make it child of `CinematicManager`
3. Set position: (0, 0, -10) for 2D
4. Background: Solid Color, Black

---

## Part 4: Create UI Canvas

### 1. Create Canvas

1. Right-click CinematicManager > `UI > Canvas`
2. Name it: `CinematicCanvas`
3. Canvas settings:
   - Render Mode: `Screen Space - Overlay`
   - Canvas Scaler:
     - UI Scale Mode: `Scale With Screen Size`
     - Reference Resolution: `1920 x 1080`
     - Match: `0.5` (middle)

### 2. Create FadePanel (for fade-in effect)

1. Right-click CinematicCanvas > `UI > Image`
2. Name it: `FadePanel`
3. Settings:
   - Anchor: Stretch both (full screen)
   - Offsets: All 0
   - Color: Black (RGB: 0,0,0, Alpha: 255)
   - Raycast Target: OFF

4. Add `CanvasGroup` component:
   - Alpha: 1
   - Interactable: OFF
   - Blocks Raycasts: OFF

### 3. Create SkipPrompt

1. Right-click CinematicCanvas > `UI > Text - TextMeshPro`
2. Name it: `SkipPrompt`
3. Settings:
   - Text: "Press [ESC] or [SPACE] to skip"
   - Font Size: 24
   - Alignment: Bottom-Center
   - Position: Bottom of screen (Anchor: Bottom-Center)
   - Color: White with slight transparency (Alpha: 180)

---

## Part 5: Connect Components in Inspector

### CinematicManager Component Settings:

1. Select `CinematicManager` GameObject
2. In `CinematicManager` script:
   - **Timeline Director**: Drag the `Playable Director` component
   - **Skip Prompt**: Drag `SkipPrompt` GameObject
   - **Fade Panel**: Drag `FadePanel` CanvasGroup component
   - **Allow Skip**: ✓ Checked
   - **Skip Prompt Delay**: `2` seconds
   - **Next Scene Name**: `"Level 1"` (or your level scene name)
   - **Auto Transition On Complete**: ✓ Checked

### Playable Director Settings:

1. **Playable**: Drag `IntroCinematic.playable` from Assets/Timelines/
2. **Update Method**: `Unscaled Game Time` (important!)
3. **Wrap Mode**: `None`

---

## Part 6: Build the Timeline

### 1. Open Timeline Window

1. Select `CinematicManager` GameObject
2. Open Timeline: `Window > Sequencing > Timeline`
3. The Timeline window should show your `IntroCinematic` timeline

---

### 2. Prepare Your Images

Before creating the timeline, prepare your image files:

1. **Import Images**: Place your images (mom1.jpg, mom2.jpg, mom3.jpg, mom4.jpg) in `Assets/Images/Cinematics/`
2. **Configure Import Settings**:
   - Select each image in Project window
   - In Inspector, set **Texture Type** to `Sprite (2D and UI)`
   - Click **Apply**
3. Your images are now ready to use as sprites!

---

### 3. Create ImageDisplay GameObject

This GameObject will display your cinematic images:

1. In Hierarchy, right-click `CinematicCanvas` > `UI > Image`
2. Name it: `ImageDisplay`
3. **Configure the Image component**:
   - **Anchor**: Click the anchor preset, hold ALT+SHIFT, click bottom-right (stretch/stretch)
   - **Left, Right, Top, Bottom**: All set to `0` (full screen)
   - **Source Image**: Leave empty for now
   - **Color**: White (255, 255, 255, 255)
   - **Raycast Target**: OFF
4. **Add CanvasGroup component** (for fade effects):
   - Click `Add Component` > `Canvas Group`
   - **Alpha**: `1`
   - **Interactable**: OFF
   - **Blocks Raycasts**: OFF

Your ImageDisplay is now ready to be animated!

---

### 4. Create Animation Tracks in Timeline

Now we'll create the timeline structure. You'll need **TWO animation tracks**:
- One for the FadePanel (fade in/out transitions)
- One for the ImageDisplay (showing your images)

#### Track 1: FadePanel Animation (Fade Effects)

1. In Timeline window, click `+ (Add)` button
2. Select `Animation Track`
3. **Drag `FadePanel` GameObject** from Hierarchy onto the **empty slot** on the left side of the track (where it says "None (Animator)")
4. The track should now show "FadePanel" and be bound

#### Track 2: ImageDisplay Animation (Your Images)

1. Click `+ (Add)` button again
2. Select `Animation Track`
3. **Drag `ImageDisplay` GameObject** from Hierarchy onto the empty slot
4. The track should now show "ImageDisplay"

Your Timeline should now have 2 Animation Tracks ready to animate!

---

### 5. Animate the FadePanel (Fade In/Out)

This creates smooth black fade transitions at start and end.

#### Step A: Fade IN at Start (0s → 2s)

1. Click the **red Record button** at the top of Timeline (it turns red when active)
2. Move the **playhead** (white vertical line) to **0 seconds**
3. Select `FadePanel` in Hierarchy
4. In Inspector, find the **CanvasGroup** component
5. Set **Alpha = 1** (screen is fully black)
6. A keyframe (diamond shape) appears on the FadePanel track

7. Move the playhead to **2 seconds**
8. In CanvasGroup, set **Alpha = 0** (screen is clear/transparent)
9. Another keyframe appears

10. Click the **Record button** to stop recording

**Result**: The cinematic now fades in from black over 2 seconds!

#### Step B: Fade OUT at End (16s → 18s)

1. Click **Record button** again
2. Move playhead to **16 seconds**
3. Make sure `FadePanel` is selected
4. In CanvasGroup, set **Alpha = 0** (screen is clear)

5. Move playhead to **18 seconds**
6. Set **Alpha = 1** (screen fades to black)

7. Stop recording

**Result**: The cinematic fades to black at the end before loading the next scene!

---

### 6. Animate the ImageDisplay (Show Your Images)

Now we'll make your images appear at different times.

#### Configure Timeline Duration First:

1. Look at the top-right of Timeline window
2. Extend the timeline duration to **20 seconds** by dragging the end marker

#### Add Image Keyframes:

1. Click the **Record button**
2. Select the **ImageDisplay track** (click on it)
3. Move playhead to **0 seconds**

4. Select `ImageDisplay` in Hierarchy
5. In Inspector, find **Image** component
6. Click the **Source Image** circle/dropdown
7. Select **mom1** sprite
8. A keyframe appears on the ImageDisplay track

9. Move playhead to **4 seconds**
10. Change **Source Image** to **mom2**

11. Move playhead to **8 seconds**
12. Change **Source Image** to **mom3**

13. Move playhead to **12 seconds**
14. Change **Source Image** to **mom4**

15. Move playhead to **16 seconds**
16. Keep **Source Image** as **mom4** (or set to None if you want it to disappear)

17. Stop recording

**Result**: Your images now change every 4 seconds throughout the cinematic!

---

### 7. Optional: Add Fade Between Images

If you want each image to smoothly fade in/out instead of popping:

#### Method 1: Single Image with Fades (Simpler)

1. Make sure `ImageDisplay` has a **CanvasGroup** component (you added this in Step 3)
2. Click **Record** button
3. On the **ImageDisplay Animation Track**, add Alpha keyframes:

**Example for first image:**
- 0s: ImageDisplay CanvasGroup Alpha = 0 (invisible)
- 0.5s: Alpha = 1 (mom1 fades in)
- 3.5s: Alpha = 1 (mom1 stays visible)
- 4s: Alpha = 0 (mom1 fades out)

**Then for second image:**
- 4s: Alpha = 0, change sprite to mom2
- 4.5s: Alpha = 1 (mom2 fades in)
- 7.5s: Alpha = 1 (stays visible)
- 8s: Alpha = 0 (fades out)

Repeat this pattern for mom3 and mom4.

#### Method 2: Crossfade Between Two Images (Advanced)

For smoother crossfade transitions, you need **two overlapping Image components**:

1. Create two Image GameObjects under CinematicCanvas:
   - `ImageDisplay1` (bottom layer)
   - `ImageDisplay2` (top layer)
2. Both need CanvasGroup components and identical RectTransform settings
3. In Timeline, alternate between them:
   - While Image1 is visible, set Image2's sprite (invisible)
   - Crossfade: Image1 alpha 1→0 while Image2 alpha 0→1
   - While Image2 is visible, set Image1's sprite (invisible)
   - Crossfade back: Image2 alpha 1→0 while Image1 alpha 0→1

**Tip**: Use the Timeline curve editor to adjust easing for smoother transitions (right-click keyframes → "Edit Ease In/Out")

---

### 8. Add Signal Track (Load Next Scene)

This tells the system to load the next level when the cinematic ends.

1. In Timeline, click `+ > Signal Track`
2. Right-click on the timeline at **18 seconds** (when screen is black)
3. Select `Add Signal Emitter`
4. In the Signal Emitter that appears:
   - You can leave it as default
   - The CinematicManager will handle the scene transition automatically

**Important**: The Signal should fire when the screen is fully black (after the fade-out), so the scene loads seamlessly!

---

### 9. Verify Timeline Structure

Your Timeline should now look like this:

```
IntroCinematic Timeline (0s ─────────────────────── 20s)
│
├── Animation Track: FadePanel
│   ├── 0s:  Alpha = 1 (black)      ◆────────
│   ├── 2s:  Alpha = 0 (clear)              ◆─────────────
│   ├── 16s: Alpha = 0 (clear)                          ◆────
│   └── 18s: Alpha = 1 (black)                              ◆
│
├── Animation Track: ImageDisplay
│   ├── 0s:  mom1 sprite    ◆─────────────
│   ├── 4s:  mom2 sprite                   ◆─────────────
│   ├── 8s:  mom3 sprite                                ◆─────────────
│   ├── 12s: mom4 sprite                                             ◆──────
│   └── (Optional: CanvasGroup Alpha keyframes for fades)
│
└── Signal Track
    └── 18s: Signal Emitter ▶ (triggers scene load)
```

---

### 10. Create Cinematic Signal Asset (Optional)

If you want custom signals for different actions:

1. Right-click in Project > `Create > ScriptableObject > CinematicSignal`
2. Name: `LoadLevel1Signal`
3. Settings:
   - Signal Type: `Load Scene`
   - Scene Name: `"Level 1"`

---

## Part 7: Understanding All Components

Here's what each component does on the CinematicManager:

### Components on CinematicManager GameObject:

1. **Playable Director** (Unity built-in)
   - **Purpose**: Plays the Timeline animation
   - **Settings**:
     - **Playable**: Assign your `IntroCinematic.playable` timeline asset
     - **Update Method**: `Unscaled Game Time` (so pause doesn't affect it)
     - **Play On Awake**: ✓ Checked (starts automatically)
   - This is the "engine" that runs your timeline!

2. **Cinemachine Brain** (optional)
   - **Purpose**: Handles camera animations and transitions
   - **Usage**: Only needed if you add Cinemachine Virtual Cameras later
   - You can ignore this for now if you're just showing static images

3. **CinematicManager** (Your custom script)
   - **Purpose**: Handles skip functionality, fade effects, and scene transitions
   - **Settings you configured in Part 5**:
     - Timeline Director: Links to Playable Director
     - Skip Prompt: Shows "Press ESC to skip" text
     - Fade Panel: The black overlay for fades
     - Next Scene Name: Where to go after cinematic
   - This script listens for ESC/Space key and manages the flow

4. **TimelineSignalReceiver** (Your custom script)
   - **Purpose**: Receives signals from the Timeline's Signal Track
   - **How it works**: When the Signal Emitter fires at 18s, this script tells CinematicManager to load the next scene
   - You don't need to configure anything - it works automatically!

5. **CinematicTrigger** (Your custom script)
   - **Purpose**: Can trigger cinematics from other scenes/events
   - **Usage**: Useful for mid-game cutscenes triggered by player actions
   - For intro cinematic, this isn't needed (but doesn't hurt to have)

### Why You Need Each Component:

```
User presses Play
        ↓
Playable Director starts playing IntroCinematic.playable
        ↓
Timeline animates FadePanel and ImageDisplay
        ↓
At 18s, Signal Track fires
        ↓
TimelineSignalReceiver catches the signal
        ↓
Tells CinematicManager to load next scene
        ↓
CinematicManager calls scene transition
        ↓
Game loads Level 1!

(At any time, if user presses ESC/Space)
        ↓
CinematicManager skips to scene transition immediately
```

---

## Part 8: Complete Timeline Structure Summary

Your final Timeline should look like this:

```
IntroCinematic Timeline (20 seconds total)
│
├── Animation Track: FadePanel
│   ├── 0s:  Alpha = 1 (black)      ◆────────
│   ├── 2s:  Alpha = 0 (clear)              ◆───────────────
│   ├── 16s: Alpha = 0 (clear)                          ◆────
│   └── 18s: Alpha = 1 (black)                              ◆
│   (Creates fade-in at start, fade-out at end)
│
├── Animation Track: ImageDisplay
│   ├── 0s:  mom1.jpg sprite    ◆────────────
│   ├── 4s:  mom2.jpg sprite                ◆────────────
│   ├── 8s:  mom3.jpg sprite                            ◆────────────
│   └── 12s: mom4.jpg sprite                                        ◆──────
│   (Shows your 4 images, 4 seconds each)
│
└── Signal Track
    └── 18s: Signal Emitter ▶ (triggers scene load when screen is black)
```

---

## Part 9: Create Prefabs (For Reusability)

### 1. Create CinematicCanvas Prefab

1. Drag `CinematicCanvas` from Hierarchy to `Assets/Prefabs/Cinematics/`
2. This canvas can be reused in all future cinematics!

### 2. Create CinematicManager Prefab (optional)

1. Drag `CinematicManager` GameObject to `Assets/Prefabs/Cinematics/`
2. For future cinematics: Instantiate this prefab and swap Timeline

---

## Part 10: Add Scene to Build Settings

**Important**: The scene must be added to Build Settings or you'll get a "scene not found" error!

### Manual Method (Unity Editor):
1. `File > Build Settings`
2. Drag `IntroCinematic.unity` into "Scenes In Build"
3. Ensure `Level 1.unity` is also in build settings

### Direct File Edit (Advanced):
The scene has been added to `ProjectSettings/EditorBuildSettings.asset`:
```yaml
- enabled: 1
  path: Assets/Scenes/Cinematics/IntroCinematic.unity
  guid: 2ceaa1f9731eb6e6fa38a7d028a4054c
```

---

## Part 11: Integration with Title Screen

### MenuNavigation Integration

The game uses a **DontDestroyOnLoad** menu system that persists across scenes. To properly integrate cinematics:

#### 1. Load Cinematic from Menu

In `MenuNavigation.cs`, the `ShowLevelSelect()` method has been updated:

```csharp
public void ShowLevelSelect()
{
    HideAll();
    HideCanvasUI(); // Hides the menu canvas during cinematic
    if (levelSelectPanel != null)
    {
        SceneTransition.Instance.TransitionToScene("IntroCinematic");
    }
}
```

**What this does**:
- Hides all menu panels
- Sets the menu canvas alpha to 0 (invisible, no interaction)
- Loads the `IntroCinematic` scene
- The menu's EventSystem persists but doesn't interfere

#### 2. CinematicManager Menu Handling

The `CinematicManager.Awake()` includes safety code to hide the menu:

```csharp
void Awake()
{
    // ... other code ...

    // Hide menu canvas if it persists from TitleScreen
    if (MenuNavigation.Instance != null)
        MenuNavigation.Instance.HideCanvasUI();

    // Ensure cursor is visible during cinematics
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
}
```

**Why this is needed**:
- The TitleScreen menu uses `DontDestroyOnLoad` and persists into the cinematic scene
- Without hiding it, the menu canvas could block interactions or appear visually
- This ensures a clean cinematic experience

#### 3. Returning to Menu

The menu automatically re-appears when returning to TitleScreen via `OnSceneLoaded()` in MenuNavigation.

---

## Part 12: Test the Cinematic

### Testing Checklist:

1. **Start from Title Screen**: Press Play in Unity Editor
2. **Click "New Game"** (or equivalent button that calls `ShowLevelSelect()`)
3. **Verify**:
   - ✓ Menu fades out and disappears
   - ✓ Cinematic scene loads
   - ✓ Screen fades in from black
   - ✓ Images display and animate according to Timeline
   - ✓ Skip prompt appears after 2 seconds
   - ✓ ESC/Space skips to Level 1 immediately
   - ✓ OR cinematic completes and auto-loads Level 1
   - ✓ No menu UI visible during cinematic

### Troubleshooting Integration Issues:

**Menu UI visible during cinematic:**
- Check that `HideCanvasUI()` is public in MenuNavigation
- Verify `ShowLevelSelect()` calls `HideCanvasUI()`
- Ensure CinematicManager.Awake() hides menu as backup

**Multiple EventSystems warning:**
- This is normal - the TitleScreen EventSystem persists
- As long as the menu canvas is hidden, this won't cause issues

**Cursor not visible:**
- Check CinematicManager.Awake() sets `Cursor.visible = true`

---

## Best Practices for Future Cinematics

### Creating New Cinematics:

1. **Duplicate Scene**: Copy `IntroCinematic.unity` → rename
2. **Create New Timeline**: Duplicate `IntroCinematic.playable` → customize
3. **Reuse Canvas Prefab**: Drag `CinematicCanvas.prefab` into new scene
4. **Update Scene Name**: Set next scene in CinematicManager

### Adding Audio:

1. In Timeline: `+ > Audio Track`
2. Drag audio clip onto track
3. Sync with visuals

### Adding Camera Effects:

1. Add Cinemachine Virtual Camera
2. In Timeline: `+ > Cinemachine Track`
3. Add camera animations (zoom, pan, etc.)

---

## Troubleshooting

**Cinematic doesn't play:**
- Check Playable Director has Timeline asset assigned
- Verify "Play On Awake" is checked on Playable Director

**Skip doesn't work:**
- Check "Allow Skip" is enabled in CinematicManager
- Verify Input System is set up (InputManager exists)

**Scene doesn't transition:**
- Verify SceneTransition prefab exists in scene (DontDestroyOnLoad)
- Check scene name spelling in "Next Scene Name"
- Ensure target scene is in Build Settings

**Images don't show:**
- Verify sprites are imported as Sprite/2D
- Check Image component has sprite assigned
- Ensure Canvas is rendering (check Camera settings)

---

## File Structure Summary

```
Project Root/
├── Assets/
│   ├── Scenes/
│   │   └── Cinematics/
│   │       └── IntroCinematic.unity
│   ├── Scripts/
│   │   ├── Cinematics/
│   │   │   ├── CinematicManager.cs
│   │   │   ├── TimelineSignalReceiver.cs
│   │   │   └── CinematicTrigger.cs
│   │   └── MenuNavigation.cs (modified for integration)
│   ├── Prefabs/
│   │   └── Cinematics/
│   │       └── CinematicCanvas.prefab
│   ├── Timelines/
│   │   └── IntroCinematic.playable
│   ├── Sprites/
│   │   └── Cinematics/
│   │       └── Intro/
│   │           ├── mom1.jpg
│   │           ├── mom2.jpg
│   │           ├── mom3.jpg
│   │           └── mom4.jpg
│   └── Signals/
│       └── (Optional cinematic signal assets)
├── Docs/
│   └── CINEMATIC_SETUP.md (this file - documentation)
└── ProjectSettings/
    └── EditorBuildSettings.asset (includes IntroCinematic scene)
```

---

## Next Steps

1. Follow this guide to set up your intro cinematic
2. Add your 4 mother images (mom1-4.jpg) to Timeline
3. Test skip functionality
4. Add audio/music if desired
5. Build additional cinematics by reusing the system!

**Questions?** All scripts are well-commented for easy customization.
