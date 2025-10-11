# Typewriter Effect Setup Guide

## Overview
The TypewriterEffect now supports Timeline integration to perfectly synchronize with panel animations.

## Components

### 1. TypewriterEffect
Main component that displays text character-by-character.

**Inspector Settings:**
- `Text Component`: Reference to the TextMeshProUGUI component
- `Full Text`: The complete text to display
- `Delay Between Chars`: Speed of typing (0.05 = fast, 0.1 = medium, 0.2 = slow)
- `Auto Start`: Set to **FALSE** for Timeline control
- `Start Delay`: Only used if Auto Start is enabled

### 2. TypewriterSignalReceiver
Receives signals from Timeline to start the typewriter effect.

## Setup Instructions

### Option A: Timeline Control (RECOMMENDED for cinematics)

1. **Add Components to Your Panel:**
   - Add `TypewriterEffect` component
   - Add `TypewriterSignalReceiver` component (automatically requires TypewriterEffect)
   - Assign the TextMeshProUGUI reference
   - Set `Auto Start` to **FALSE**

2. **In Your Timeline:**
   - Create a **Signal Track**
   - Right-click in the track → Add Signal Emitter
   - Position the signal emitter AFTER your panel's entrance animation completes
   - In the signal emitter inspector, set the **Receiver** to your panel GameObject

3. **Timeline Example:**
   ```
   Timeline:
   [0s] ---------- [1s] ---------- [2s] ---------- [3s]
        Panel Fade In Animation
                    ↑
                    Signal: Start Typewriter
                    (placed at 1.0s, after fade completes)
   ```

### Option B: Animation Event (For single panel animations)

1. **Add TypewriterEffect:**
   - Set `Auto Start` to **FALSE**

2. **In Your Panel's Animation:**
   - Open the Animation window
   - At the END of your entrance animation, add an **Animation Event**
   - Set the event function to: `StartTyping()`
   - Set the receiver to the GameObject with TypewriterEffect

### Option C: Auto-Start with Delay (Quick & dirty)

1. **Add TypewriterEffect:**
   - Set `Auto Start` to **TRUE**
   - Set `Start Delay` to match your panel animation duration (e.g., 1.0 seconds)

**Note:** This is less reliable as delays can desync. Use Option A or B for production.

## Advanced Usage

### Skip Typewriter Effect
Call `SkipToEnd()` to instantly show all text:
```csharp
typewriterEffect.SkipToEnd();
```

### Reset Text
Call `ResetText()` to clear the text:
```csharp
typewriterEffect.ResetText();
```

### Check if Typing
```csharp
// Access via reflection or add a public IsTyping property
```

## Best Practices

1. **Always use Timeline Signals** for multi-panel cinematics - it's the most reliable method
2. **Set Auto Start to FALSE** when using Timeline control
3. **Position signals carefully** - place them exactly when animations finish
4. **Test timing** - adjust signal position or delay to feel natural
5. **Clear text initially** - TypewriterEffect automatically clears text on Start()

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Text appears immediately | Check that `Auto Start` is FALSE and signal is firing |
| Text appears during animation | Move Timeline signal later, after animation completes |
| No text appears | Verify TextMeshProUGUI is assigned and Full Text is set |
| Signal not firing | Check Signal Track has correct Receiver GameObject set |

## Example Timeline Structure

```
Cinematic Timeline:
├── Activation Track (Panel GameObject)
│   └── [0s-3s] Active
├── Animation Track (Panel Animator)
│   └── [0s-1s] FadeIn animation
└── Signal Track
    └── [1.0s] Signal → Triggers TypewriterEffect.StartTyping()
```

## Migration from Old System

If you have existing TypewriterEffect components:
1. Open the panel prefab/scene
2. Keep your existing settings (text, delay, etc.)
3. **Set Auto Start to FALSE**
4. Add TypewriterSignalReceiver component
5. Update your Timeline with Signal Track
6. Test!

Old behavior (auto-start on enable) is still available by setting `Auto Start = TRUE`.
