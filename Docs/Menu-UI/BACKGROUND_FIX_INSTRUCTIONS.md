# Background Display Issues - Solutions

## Problems Identified

I've found several issues preventing your background from displaying properly in full screen:

### 1. Canvas Scaler Settings (CRITICAL)
**Location:** TitleScreen scene - Canvas GameObject
- **Current:** `m_UiScaleMode: 0` (Constant Pixel Size)
- **Current Resolution:** `{x: 800, y: 600}`
- **Problem:** This mode doesn't scale UI to fill the screen properly

### 2. Background RectTransform Anchors (CRITICAL)
**Location:** Background prefab
- **Current Anchors:** Min `{x: 0, y: 0}`, Max `{x: 0, y: 0}`
- **Current Size Delta:** `{x: 0, y: 0}`
- **Current Pivot:** `{x: 0, y: 0}`
- **Problem:** These settings make the background have zero size!

### 3. Project Resolution Settings
**Location:** ProjectSettings
- **Default Resolution:** 1920x1080 (good)
- **Resizable Window:** Disabled
- **Problem:** `resizableWindow: 0` might limit display flexibility

## How to Fix

### Fix 1: Canvas Scaler (Do This First!)

1. Open the **TitleScreen** scene
2. Select the **Canvas** GameObject in the hierarchy
3. Find the **Canvas Scaler** component in the Inspector
4. Change settings to:
   - **UI Scale Mode:** Scale With Screen Size
   - **Reference Resolution:** X: 1920, Y: 1080 (or your target resolution)
   - **Screen Match Mode:** Match Width Or Height
   - **Match:** 0.5 (balanced) or adjust to preference
     - 0 = prioritize width matching
     - 1 = prioritize height matching
     - 0.5 = balanced

### Fix 2: Background Anchors (Critical!)

1. In the **TitleScreen** scene hierarchy, find the **Background** GameObject
2. Select it and look at the **Rect Transform** component
3. Click the **Anchor Presets** button (top-left of Rect Transform)
4. Hold **ALT + SHIFT** and click **Bottom-Right** (stretch/stretch preset)
   - This will set anchors to fill the entire canvas
5. Verify the following values:
   - **Anchor Min:** X: 0, Y: 0
   - **Anchor Max:** X: 1, Y: 1
   - **Left, Top, Right, Bottom:** all set to 0
   - **Pivot:** X: 0.5, Y: 0.5

OR manually set:
   - Anchor Min: {x: 0, y: 0}
   - Anchor Max: {x: 1, y: 1}
   - Pivot: {x: 0.5, y: 0.5}
   - Anchored Position: {x: 0, y: 0}
   - Size Delta: {x: 0, y: 0}

### Fix 3: Update Background Prefab

After fixing the Background in the scene:
1. Select the Background GameObject
2. In the Inspector, look for the Prefab header
3. Click **Overrides** dropdown
4. Click **Apply All** to save changes to the prefab

### Fix 4: Camera Settings (Optional - Check)

The camera is currently orthographic with size 12. Make sure:
- **Clear Flags:** Solid Color or Skybox
- **Orthographic Size:** Adjust if UI seems wrong (current: 12)

### Fix 5: Enable Resizable Window (Optional)

In **Edit > Project Settings > Player:**
- Find **Resolution and Presentation**
- Enable **Resizable Window** for better user experience

## Testing

After making these changes:
1. Save the scene (Ctrl+S)
2. Test in Play Mode
3. Test in a build (File > Build and Run)
4. Try different resolutions to ensure proper scaling

## Additional Tips

- Make sure your background image asset has proper import settings:
  - **Texture Type:** Sprite (2D and UI)
  - **Max Size:** Large enough for your resolution
- Consider using **Preserve Aspect** on the Image component if you want to maintain aspect ratio
- If you want the background to extend beyond screen edges (no letterboxing), use Match = 0 or 1 depending on your needs
