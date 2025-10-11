# Position Sprite at Bottom of Screen - QTE Scene

## Your Situation:
- Camera: Orthographic Size = **5**
- Player sprite: Cut at hips (designed that way)
- Goal: Position sprite so **bottom edge aligns with bottom of screen**

---

## Camera Visible Area

With orthographic size = 5:
- **Visible height:** 10 units (5 × 2)
- **Bottom edge:** Y = **-5**
- **Top edge:** Y = **+5**
- **Center:** Y = **0**

For 16:9 aspect ratio:
- **Visible width:** 17.78 units
- **Left edge:** X = **-8.89**
- **Right edge:** X = **+8.89**

---

## Formula to Position Sprite at Bottom

To align a sprite's **bottom edge** with the screen bottom:

```
Y Position = Bottom Edge + (Sprite Height / 2)
Y Position = -5 + (Sprite Height / 2)
```

### How to Find Your Sprite Height:

1. Select your player sprite in the scene
2. Look at **SpriteRenderer** component
3. Check the **Bounds → Size → Y** value

OR

1. Select the sprite asset in Project
2. Look at the dimensions (e.g., 512x512 pixels)
3. Calculate: Height in units = (Pixels / Pixels Per Unit)
4. Default Pixels Per Unit = 100

---

## Examples:

### Example 1: Sprite is 300 pixels tall
- Pixels Per Unit: 100 (default)
- **Height in units:** 300 / 100 = **3 units**
- **Y Position:** -5 + (3 / 2) = **-3.5**

### Example 2: Sprite is 400 pixels tall
- Pixels Per Unit: 100
- **Height in units:** 400 / 100 = **4 units**
- **Y Position:** -5 + (4 / 2) = **-3**

### Example 3: Sprite is 500 pixels tall
- Pixels Per Unit: 100
- **Height in units:** 500 / 100 = **5 units**
- **Y Position:** -5 + (5 / 2) = **-2.5**

---

## Step-by-Step: Position Your Player

### Method 1: Manual (Quick)

1. **Find sprite height:**
   - Select player sprite in scene
   - Inspector → SpriteRenderer → look at sprite preview size
   - Or check the sprite asset (e.g., "512x300 px")

2. **Calculate position:**
   - Sprite height in pixels: **[YOUR_VALUE]**
   - Divide by 100 (or your Pixels Per Unit): **[HEIGHT_IN_UNITS]**
   - Y Position = -5 + (height / 2)

3. **Set Transform:**
   - X: **-5** (left side, or adjust as needed)
   - Y: **[CALCULATED_VALUE]**
   - Z: **0**

### Method 2: Script (Automatic)

Use this script to automatically align sprite to bottom:

```csharp
using UnityEngine;

public class AlignToBottomOfCamera : MonoBehaviour
{
    public Camera targetCamera;
    public float bottomPadding = 0f; // Extra space from bottom (in units)

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        AlignToBottom();
    }

    void AlignToBottom()
    {
        if (targetCamera == null || !targetCamera.orthographic) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        // Get sprite height in world units
        float spriteHeight = sr.bounds.size.y;

        // Calculate bottom edge of camera
        float cameraBottom = -targetCamera.orthographicSize;

        // Position sprite so its bottom edge aligns with camera bottom
        float newY = cameraBottom + (spriteHeight / 2f) + bottomPadding;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    [ContextMenu("Realign to Bottom")]
    public void Realign()
    {
        AlignToBottom();
    }
}
```

**To use:**
1. Create script `AlignToBottomOfCamera.cs` in Assets/Scripts
2. Add to your **Player sprite** GameObject
3. Assign **Main Camera** in Inspector
4. It automatically positions the sprite!
5. Adjust **Bottom Padding** if you want a small gap

---

## Positioning Both Player and Dino

### Player (left, bottom-aligned):
```
X: -5 (or -6 for more left)
Y: Calculate using formula above
Z: 0
```

### Dino (right, bottom-aligned):
```
X: 5 (or 6 for more right)
Y: Same as player (if same height) or calculate separately
Z: 0
```

---

## Handling Different Sprite Scales

If you **scale** your sprite, the position changes!

**Formula with scale:**
```
Y Position = -5 + (Sprite Height × Scale / 2)
```

**Example:**
- Sprite height: 4 units
- Scale: 1.5x
- **Y Position:** -5 + (4 × 1.5 / 2) = -5 + 3 = **-2**

---

## Quick Reference Table (for Orthographic Size = 5)

| Sprite Height | Scale | Y Position |
|---------------|-------|------------|
| 2 units       | 1.0   | -4.0       |
| 3 units       | 1.0   | -3.5       |
| 4 units       | 1.0   | -3.0       |
| 5 units       | 1.0   | -2.5       |
| 6 units       | 1.0   | -2.0       |
| 4 units       | 1.5   | -2.0       |
| 4 units       | 2.0   | -1.0       |

---

## Testing Different Resolutions

After positioning, test in Game view:
1. **16:9 (1920x1080)** - Standard
2. **16:10 (1920x1200)** - Slightly taller (shows more top/bottom)
3. **21:9 (2560x1080)** - Ultrawide (shows more sides)

**Expected behavior:**
- Sprite stays at bottom on 16:9 ✓
- On 16:10 (taller), sprite is **slightly higher from bottom edge** (more space shown below)
- On 21:9 (ultrawide), sprite stays at same height ✓

**If you want EXACT bottom alignment on ALL aspect ratios:**
- You need a script that recalculates position based on camera aspect
- OR accept that taller screens show slightly more at top/bottom (standard approach)

---

## Example: Complete Setup

```csharp
// AlignToBottomOfCamera.cs
using UnityEngine;

[ExecuteInEditMode] // Updates in editor too!
public class AlignToBottomOfCamera : MonoBehaviour
{
    public Camera targetCamera;

    [Tooltip("Extra space from bottom edge (in world units)")]
    public float bottomPadding = 0f;

    [Tooltip("Horizontal position (-5 = left, 0 = center, 5 = right)")]
    public float horizontalPosition = -5f;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        PositionSprite();
    }

    void Update()
    {
        // Only update in editor, not during play
        if (!Application.isPlaying)
            PositionSprite();
    }

    void PositionSprite()
    {
        if (targetCamera == null || !targetCamera.orthographic) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        // Get actual sprite bounds (accounts for scale automatically)
        float spriteHeight = sr.bounds.size.y;

        // Calculate bottom edge of camera view
        float cameraBottom = -targetCamera.orthographicSize;

        // Position sprite
        float yPos = cameraBottom + (spriteHeight / 2f) + bottomPadding;

        transform.position = new Vector3(horizontalPosition, yPos, transform.position.z);
    }
}
```

**Benefits:**
- Auto-positions in editor AND runtime
- Accounts for sprite scale automatically
- Easy to adjust with sliders
- Works with any camera size

---

## Quick Fix Right Now:

Without creating a script, here's what to do:

1. **Select your player sprite** in hierarchy
2. **Look at the sprite's bounds:**
   - Play the scene
   - Select sprite
   - Inspector → SpriteRenderer → check "Bounds" section
   - Note the "Size Y" value (let's say it's 4.0)
3. **Calculate Y position:**
   - Y = -5 + (4.0 / 2) = **-3.0**
4. **Set Transform position:**
   - X: -5
   - Y: -3.0
   - Z: 0
5. **Test in Game view!**

If sprite is still not at exact bottom, adjust the Y value up or down by 0.5 units until it looks right!
