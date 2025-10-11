# Camera-Based Background Solution

Since your Background uses **SpriteRenderers** (not UI Images), you should use a **Camera-based approach** instead of Canvas!

## The Setup:

### Step 1: Move Background Out of Canvas

1. In **Hierarchy**, drag **Background** to the **root** (outside any Canvas)
2. Delete the Background's Canvas if it's empty

### Step 2: Set Background Transform

1. Select **Background** GameObject
2. Set **Transform** position:
   - X: **0**
   - Y: **0** (or **1** to center it with camera)
   - Z: **0** or **5** (in front of camera)

### Step 3: Scale Background to Fit Camera

Your camera has **Orthographic Size = 12**, which means it shows 24 units vertically.

For a 16:9 aspect ratio:
- Height visible = 24 units (orthographic size × 2)
- Width visible = 24 × (16/9) = 42.67 units

**Option A - Manual scaling:**
1. Check your background sprite's size in pixels
2. Calculate scale needed to fill 24 units height
3. Example: If sprite is 1080px tall, scale = 24 / (1080 / 100) = ~2.22

**Option B - Script to auto-scale:**
I can create a script that automatically scales the background to always fill the camera view, regardless of resolution!

### Step 4: Set Sprite Renderer Sort Order

1. Select **Background** GameObject
2. In **SpriteRenderer** component:
   - **Sorting Layer:** Default
   - **Order in Layer:** **-100** (renders behind everything)

### Step 5: Make Sure Camera Sees It

1. Select **Main Camera**
2. Check settings:
   - **Projection:** Orthographic ✓ (you have this)
   - **Size:** 12 ✓ (you have this)
   - **Position:** (0, 1, -10) ✓ (you have this)
   - **Clear Flags:** Solid Color
   - **Culling Mask:** Everything is checked

## Auto-Scaling Script (Recommended!)

This script will automatically scale your background to fill the camera at any resolution:

```csharp
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool maintainAspect = true;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            ScaleToFitCamera();
    }

    void ScaleToFitCamera()
    {
        if (spriteRenderer.sprite == null) return;

        // Get camera dimensions
        float cameraHeight = targetCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        // Get sprite dimensions
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        // Calculate scale needed
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        if (maintainAspect)
        {
            // Use the larger scale to ensure coverage (may crop)
            float scale = Mathf.Max(scaleX, scaleY);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
        else
        {
            // Stretch to fit (may distort)
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }

    // Call this if you change camera size or aspect ratio
    public void UpdateScale()
    {
        ScaleToFitCamera();
    }
}
```

**To use this script:**
1. Create new C# script called `BackgroundScaler.cs` in Assets/Scripts
2. Copy the code above
3. Add the script to your **Background** GameObject
4. In Inspector:
   - **Target Camera:** Drag Main Camera here
   - **Maintain Aspect:** Check this (prevents distortion)

The background will now automatically scale to fill the screen at any resolution!

## Why This Works Better:

✅ SpriteRenderers work in world space (no Canvas needed)
✅ MenuParallax script will work perfectly
✅ Better performance for animated backgrounds
✅ No UI anchor issues
✅ Works at all resolutions automatically

## Hierarchy Should Look Like:

```
Scene
├─ Main Camera
├─ Background              ← World space, SpriteRenderer
│  └─ (child sprites)
├─ Canvas                  ← UI Canvas
│  ├─ TitlePanel
│  ├─ OptionsPanel
│  └─ ...
└─ EventSystem
```

## Canvas Settings:

Your main menu Canvas should be:
- **Render Mode:** Screen Space - Overlay
- **Canvas Scaler → UI Scale Mode:** Scale With Screen Size
- **Reference Resolution:** 1920 x 1080
- **Match:** 0

This way:
- Background is in world space (SpriteRenderer)
- UI is in screen space (Canvas overlay)
- Both scale properly to any resolution!

---

## Quick Test:

1. Move Background out of Canvas
2. Add the BackgroundScaler script
3. Enter Play mode
4. Change Game view resolution
5. Background should always fill screen!
