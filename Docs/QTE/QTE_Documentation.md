# QTE System - Complete Documentation

## Overview

The QTE (Quick Time Event) system lets players photograph dinosaurs by pressing arrow key sequences within a time limit. Success quality affects photo clarity.

---

## Core Components

### 1. QTEManager.cs
Main QTE logic controller.

**Features:**
- Countdown (3, 2, 1, GO!)
- Random key sequence generation
- Timer with progress bar
- Sliding window display (3 keys visible at a time)
- Input detection (New Input System)
- Success/fail/perfect scoring

**Key Settings:**
- `baseDbmPull`: Base countdown duration (default: 3s)
- `keyDisplaySlots`: 3 Image UI slots for key display
- `activeKeyScale`: Current key size multiplier (default: 1.2)
- `pressedDisplayDuration`: Pressed sprite duration (default: 0.2s)

### 2. DinosaurQTE.cs
Per-dinosaur QTE configuration.

**Settings:**
- `baseTimeLimit`: BASE seconds to complete (modified by difficulty)
- `baseKeyCount`: BASE keys to press (modified by difficulty)
- `dinoName`: Dinosaur identifier
- `perfectPhoto/clearPhoto/blurryPhoto`: Result sprites
- `autoRetry`: Retry on failure (default: true)
- `retryDelay`: Delay before retry (default: 4s)

### 3. DifficultySettings.cs
Global difficulty manager (Singleton, DontDestroyOnLoad).

**Difficulty Levels:**

| Level | Time Multiplier | Key Modifier | Countdown |
|-------|----------------|--------------|-----------|
| Easy | 1.5× | +0 | 4s |
| Normal | 1.0× | +2 | 3s |
| Hard | 0.75× | +4 | 2s |

**Example (baseTimeLimit=5s, baseKeyCount=3):**
- Easy: 3 keys, 7.5s
- Normal: 5 keys, 5s
- Hard: 7 keys, 3.75s

**API:**
```csharp
DifficultySettings.Instance.SetDifficulty(DifficultyLevel.Easy);
DifficultySettings.Instance.GetDifficulty();
```

### 4. PhotoManager.cs
Photo collection manager (Singleton).

**Features:**
- Display photos temporarily
- Store photos in memory (session-only)
- Retrieve by dinosaur name

**API:**
```csharp
PhotoManager.Instance.ShowAndSavePhoto(dinoName, sprite, isSuccess);
List<DinoPhoto> photos = PhotoManager.Instance.GetAllPhotos();
```

---

## Visual Effects

### QTESuccessHalo.cs
Green halo effect on successful key press.

### CountdownAnimation.cs
Animates countdown with pulse and color changes (3=red, 2=orange, 1=yellow, GO=green).

### ScrollingTexture.cs
Scrolls timer bar texture horizontally.

### CameraFlashEffect.cs
Camera flash effect (reusable).

---

## Setting Up Difficulty

### 1. Create DifficultySettings GameObject
- Open **TitleScreen.unity** (or first scene)
- Create Empty GameObject → Name: "DifficultySettings"
- Add Component → DifficultySettings
- Set difficulty to Easy/Normal/Hard
- Save scene

### 2. Configure Per-Dinosaur Difficulty
Adjust `baseTimeLimit` and `baseKeyCount` in each DinosaurQTE:

**Common dinosaurs:**
```
baseTimeLimit = 6.0
baseKeyCount = 2
→ Easy: 2 keys, 9s | Normal: 4 keys, 6s | Hard: 6 keys, 4.5s
```

**Standard dinosaurs:**
```
baseTimeLimit = 5.0
baseKeyCount = 3
→ Easy: 3 keys, 7.5s | Normal: 5 keys, 5s | Hard: 7 keys, 3.75s
```

**Boss dinosaurs:**
```
baseTimeLimit = 4.0
baseKeyCount = 5
→ Easy: 5 keys, 6s | Normal: 7 keys, 4s | Hard: 9 keys, 3s
```

---

## Unity Setup

### Canvas Hierarchy
```
Canvas QTE (World Space)
├── Background_Image
├── MaskContainer (Mask component)
│   └── Background (scrolling texture)
├── QTE_TimerBar (Slider)
│   ├── Fill Area
│   │   └── Fill (Image Type: Filled, Horizontal)
│   └── Border
├── CountdownText (TextMeshPro + CountdownAnimation)
├── KeysContainer
│   ├── KeySlot_0 (current key - larger)
│   ├── KeySlot_1 (next key)
│   └── KeySlot_2 (next key)
├── SuccessImage (hidden by default)
├── FailImage (hidden by default)
├── PerfectImage (hidden by default)
└── ExitButton (Button + QTEExitButton)
```

### Scene GameObjects
```
Scene
├── DifficultySettings (in TitleScreen scene)
├── QTEManager (+ QTESuccessHalo component)
├── PhotoManager
└── Dinosaur
    └── DinosaurQTE component
```

---

## Input System

Uses **New Input System**:
- Arrow keys: up/down/left/right
- Maps to WASD (QWERTY) or ZQSD (AZERTY)
- Supports both keyboard layouts via InputManager

**Key mapping:**
- "up" → W/Z sprites
- "down" → S sprites
- "left" → A/Q sprites
- "right" → D sprites

---

## Gameplay Flow

### Success Path
1. Countdown: 3, 2, 1, GO! (configurable)
2. QTE Active: Press keys shown (3 visible at a time)
3. All keys pressed: Show Perfect/Success image
4. Display photo (3s)
5. Save to PhotoManager
6. Exit QTE

### Failure Path
1. Countdown: 3, 2, 1, GO!
2. QTE Active: Press keys
3. Timer runs out: Show Fail image
4. Display blurry photo (3s)
5. Save to PhotoManager
6. Retry after delay (if autoRetry enabled)

### Perfect vs Success
- **Perfect:** All keys correct, no mistakes
- **Success:** All keys pressed, but had wrong inputs
- **Fail:** Timer expired

---

## Advanced Features

### Sliding Window Display
Keys display 3 at a time:
```
Sequence: [UP, DOWN, LEFT, RIGHT, UP, DOWN, LEFT]
Display:   ^    ^     ^                              (shows 3)
After 1:        ^     ^      ^                       (slides forward)
After 2:              ^      ^      ^                (slides forward)
```

### Prevent Consecutive Duplicates
Enable `preventConsecutiveDuplicates` in DifficultySettings to avoid patterns like UP-UP-DOWN.

### Keyboard Layout Support
Automatically detects QWERTY/AZERTY and displays correct sprites via InputManager.

---

## Debugging

### Console Messages
```
[QTE] Difficulty: Easy | Base: 3 keys, 5s | Adjusted: 3 keys, 7.5s
```

If you see:
```
[QTE] DifficultySettings.Instance is NULL!
```
→ DifficultySettings GameObject missing from scene.

---

## Troubleshooting

**Sprites not showing:**
- Verify KeyDisplaySlots assigned in QTEManager
- Check sprites imported as "Sprite (2D and UI)"
- Image color should be white

**Timer bar not emptying:**
- Fill Image Type = "Filled"
- Fill Method = "Horizontal"
- Fill Origin = "Left"

**Difficulty not applying:**
- DifficultySettings GameObject must exist in TitleScreen or Level 1
- Check DifficultySettings has `[DefaultExecutionOrder(-100)]` to load first
- Verify GameManager syncs difficulty in Awake()

**Wrong key count in Inspector:**
- DinosaurQTE Inspector values override code defaults
- Manually set `baseKeyCount = 3` in Inspector for each scene

---

## Technical Notes

- **Singleton pattern:** GameManager, DifficultySettings, PhotoManager use DontDestroyOnLoad
- **Execution order:** DifficultySettings loads first (`-100` priority)
- **Input System:** Requires Unity Input System package
- **Minimum keys:** System enforces at least 1 key (never 0)
- **Photo storage:** Memory-only, not persisted to disk

---

## Assets Required

**Sprites:**
- Key sprites: WASD (QWERTY) and ZQSD (AZERTY)
- Pressed variants for each key
- Success/Fail/Perfect result images
- Dinosaur photos (perfect, clear, blurry per dino)

**UI:**
- Timer bar texture
- Exit button (cross icon)
- Background images

---

## API Reference

### Change Difficulty at Runtime
```csharp
GameManager.Instance.SetDifficulty(DifficultySettings.DifficultyLevel.Hard);
// or
DifficultySettings.Instance.SetDifficulty(DifficultySettings.DifficultyLevel.Easy);
```

### Access Photos
```csharp
List<DinoPhoto> all = PhotoManager.Instance.GetAllPhotos();
List<DinoPhoto> trex = PhotoManager.Instance.GetPhotosByDino("T-Rex");

foreach (var photo in all)
{
    Debug.Log($"{photo.dinoName}: {(photo.isHighQuality ? "Perfect" : "Clear")}");
}
```

### Restart/Stop QTE
```csharp
qteManager.RestartQTE(); // Restart countdown
qteManager.StopQTE();    // Cancel QTE
```

---

## Credits

System developed with assistance from Claude Code (Anthropic).
