# QTE Scene Setup Guide - Best Practices

## Your Current QTE Setup:

- **Camera:** Orthographic, Size = 5
- **Canvas:** You have 2 canvases (UI elements)
- **Scene Type:** Fixed screen QTE (typing prompts)
- **Content:** Player sprite + Dino sprite + UI prompts

---

## Best Practice for Fixed QTE Scenes

For a **fixed screen** with **UI prompts** and **character sprites**, use a **hybrid approach**:

### **1. Background & Characters: World Space (SpriteRenderers)**
### **2. UI Elements: Canvas Overlay**

This gives you:
- ✅ Easy sprite positioning in world coordinates
- ✅ Consistent UI scaling across resolutions
- ✅ Clear separation between gameplay and UI

---

## Step-by-Step Setup

### Part 1: Camera Setup (Already Good!)

Your camera settings are fine:
- **Projection:** Orthographic ✓
- **Size:** 5 ✓ (shows 10 units vertically)
- **Position:** (0, 0, -10)

**Visible area at 16:9:**
- Height: 10 units (5 × 2)
- Width: 17.78 units (10 × 16/9)

### Part 2: Position Your Sprites (Player & Dino)

Use world coordinates for consistent positioning:

#### **Safe Zone (Always Visible):**
```
         Top: Y = 4.5

Left: X = -8    Center: X = 0    Right: X = 8

        Bottom: Y = -4.5
```

#### **Recommended Positions:**

**Player (left side):**
- Position: X = **-5**, Y = **0**, Z = **0**
- Scale: Adjust to fit (try 1.5 to 2)
- Sorting Layer: Default
- Order in Layer: 10

**Triceratops (right side):**
- Position: X = **5**, Y = **0**, Z = **0**
- Scale: Adjust to fit
- Sorting Layer: Default
- Order in Layer: 10

**Background (if any):**
- Position: X = **0**, Y = **0**, Z = **1** (behind characters)
- Use BackgroundScaler script (or scale to ~20 units wide)
- Sorting Layer: Default
- Order in Layer: -10

### Part 3: Canvas Setup for UI

You have 2 canvases - configure both the same way:

#### **Canvas Settings:**
```
Render Mode: Screen Space - Overlay
```

#### **Canvas Scaler:**
```
UI Scale Mode: Scale With Screen Size
Reference Resolution: 1920 x 1080
Screen Match Mode: Match Width Or Height
Match: 0.5 (balanced for 16:9)
```

#### **UI Elements Positioning:**

Use **anchors** for consistent positioning across resolutions:

**Countdown Text (center-top):**
- Anchors: Center-Top (0.5, 1)
- Position: Y = -100 (pixels from top)

**Timer Bar (bottom):**
- Anchors: Stretch-Bottom (0, 0) to (1, 0)
- Position: Y = 50 (pixels from bottom)
- Width: Stretch

**Key Display Slots (center):**
- Anchors: Center-Middle (0.5, 0.5)
- Position: Y = 200 (above center)

**Result Images (center):**
- Anchors: Center-Middle (0.5, 0.5)
- Position: (0, 0)

---

## Quick Script: Auto Position Characters

Create a script to automatically position your characters based on camera:

```csharp
using UnityEngine;

public class QTECharacterPositioner : MonoBehaviour
{
    [Header("References")]
    public Camera qteCamera;
    public Transform playerSprite;
    public Transform dinoSprite;

    [Header("Positioning")]
    [Range(0.2f, 0.9f)]
    public float horizontalPosition = 0.3f; // How far from edges (0.5 = center)

    [Range(-1f, 1f)]
    public float verticalOffset = -0.2f; // Offset from center (-1 = bottom, 1 = top)

    void Start()
    {
        if (qteCamera == null)
            qteCamera = Camera.main;

        PositionCharacters();
    }

    void PositionCharacters()
    {
        if (qteCamera == null || !qteCamera.orthographic) return;

        // Calculate visible area
        float cameraHeight = qteCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * qteCamera.aspect;

        // Calculate positions
        float yPos = verticalOffset * qteCamera.orthographicSize;
        float leftX = -cameraWidth * horizontalPosition;
        float rightX = cameraWidth * horizontalPosition;

        // Position player (left)
        if (playerSprite != null)
        {
            playerSprite.position = new Vector3(leftX, yPos, 0);
        }

        // Position dino (right)
        if (dinoSprite != null)
        {
            dinoSprite.position = new Vector3(rightX, yPos, 0);
        }
    }

    // Call this if you change camera size or aspect ratio
    [ContextMenu("Reposition Characters")]
    public void RepositionCharacters()
    {
        PositionCharacters();
    }
}
```

**To use:**
1. Create `QTECharacterPositioner.cs` in Assets/Scripts
2. Add to any GameObject in QTE scene
3. Assign camera, player sprite, and dino sprite
4. Adjust `horizontalPosition` and `verticalOffset` sliders
5. Characters will auto-position for any resolution!

---

## Testing Across Resolutions

### Test these in Game view:

1. **16:9 (1920x1080)** - Standard
2. **16:10 (1920x1200)** - Taller
3. **21:9 (2560x1080)** - Ultrawide
4. **Free Aspect** - Resize window

### What Should Happen:

✅ **World sprites (characters):**
- Stay in the same world positions
- May show more/less on sides (ultrawide vs narrow)
- This is expected and normal

✅ **UI elements:**
- Scale consistently
- Timer bar always stretches full width
- Text always readable
- Prompts always centered

❌ **Avoid:**
- Characters positioned with UI anchors (they won't scale right)
- Hard-coded pixel positions for sprites
- Different Canvas settings on the two canvases

---

## Common QTE Layout Patterns

### Pattern 1: Side-by-Side (Your Current Setup)
```
[ Player ]    [ UI Prompts ]    [ Dino ]
    -5              0               +5
```

### Pattern 2: Face-off
```
         [ UI Prompts ]
              (top)

[ Player ]              [ Dino ]
   -6                      +6

          [ Timer Bar ]
            (bottom)
```

### Pattern 3: Close-up
```
           [ Dino Close-up ]
                 (0, 2)

          [ UI Prompts ]
               (0, 0)

           [ Player Hands ]
               (0, -2)
```

---

## Troubleshooting

### "Sprites are too big/small at different resolutions"

**Solution:** This is normal for world space sprites. Either:
1. Accept that ultra-wide shows more scene (standard approach)
2. Use the QTECharacterPositioner script to scale sprites based on camera size
3. Adjust camera orthographic size for different aspect ratios (advanced)

### "UI elements are in wrong positions"

**Check:**
- Canvas Scaler is set to "Scale With Screen Size"
- Reference Resolution is 1920x1080
- UI elements use **anchors**, not absolute positions

### "Characters are off-screen on some resolutions"

**Fix:**
- Keep sprites within **safe zone** (±8 units horizontally for 16:9)
- Test on narrower aspect ratios (4:3, 5:4)
- Use QTECharacterPositioner to automatically adjust

### "Everything works in editor but not in build"

**Check:**
- Canvas Render Mode is "Screen Space - Overlay"
- Camera settings are saved in the scene
- Build resolution settings in Player Settings

---

## Recommended Final Setup

**Hierarchy:**
```
QTE_triceratops Scene
├─ Main Camera (Ortho, Size 5)
├─ Background (SpriteRenderer, Z=1, Order=-10)
├─ PlayerSprite (SpriteRenderer, X=-5, Order=10)
├─ DinoSprite (SpriteRenderer, X=5, Order=10)
├─ Canvas (Overlay, Scale With Screen Size)
│  ├─ CountdownText (Center-Top)
│  ├─ KeyDisplaySlots (Center-Middle)
│  ├─ TimerBar (Stretch-Bottom)
│  └─ ResultImages (Center-Middle)
├─ QTEManager
└─ EventSystem
```

**Canvas Scaler Settings:**
- UI Scale Mode: **Scale With Screen Size**
- Reference Resolution: **1920 x 1080**
- Match: **0.5**

**Character Positions:**
- Player: **(-5, 0, 0)** or **(-6, -1, 0)** if you want them lower
- Dino: **(5, 0, 0)** or **(6, -1, 0)**

This setup will work consistently across all resolutions while keeping your characters properly positioned!
