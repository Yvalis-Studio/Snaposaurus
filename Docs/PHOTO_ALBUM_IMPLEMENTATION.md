# 📸 Photo Album System - Implementation Guide

## 🎯 Project Overview

Implementation of a photo album collection system for Snaposaurus. Players can unlock photos of dinosaurs after completing QTE minigames, with 3 quality levels per dinosaur that accumulate in a scrapbook-style album.

### Design Specifications
- **4 Dinosaurs**: Archeopteryx, Triceratops, Diplodocus, Godzilla
- **3 Quality Levels per Dino**: Blurry (fail), Clear (success), Perfect (perfect QTE)
- **Cumulative Collection**: All 3 qualities can be unlocked independently (12 photos total)
- **Photo Specs**: 1126x1080px imported at 100 PPU in Unity
- **UI Layout**: Fan/spread style with overlapping photos, rotated slightly
- **Background**: `concept_02_vide.png` (scrapbook style)
- **Locked State**: Blurred version of the actual photo

---

## 📋 Implementation Status

### ✅ Completed
- [x] Project structure analysis
- [x] Architecture planning
- [x] PhotoAlbumData.cs (in progress - see snippet below to complete)

### 🔄 In Progress
- [ ] PhotoAlbumData.cs - finish copying the full script

### ⏳ To Do
- [ ] Modify PhotoManager.cs
- [ ] Modify DinosaurQTE.cs
- [ ] Modify GameManager.cs
- [ ] Create PhotoSlotUI.cs
- [ ] Create PhotoAlbumUI.cs
- [ ] Modify MenuNavigation.cs
- [ ] Create PhotoAlbumDebugger.cs
- [ ] Create Unity UI hierarchy
- [ ] Create blurred placeholder sprites
- [ ] Configure Inspector references
- [ ] Test complete flow

---

## 🗂️ File Structure

```
Assets/
├── Scripts/
│   ├── PhotoAlbumData.cs          [NEW - IN PROGRESS]
│   ├── PhotoManager.cs             [MODIFY]
│   ├── QTE/
│   │   └── DinosaurQTE.cs          [MODIFY]
│   ├── GameManager.cs              [MODIFY]
│   └── UI/
│       ├── PhotoSlotUI.cs          [NEW]
│       ├── PhotoAlbumUI.cs         [NEW]
│       └── MenuNavigation.cs       [MODIFY]
├── Sprites/
│   └── PhotoAlbum/
│       ├── concept_02_vide.png     [USE AS BACKGROUND]
│       └── Locked/                 [CREATE FOLDER]
│           ├── archeo_locked.png       [CREATE - blurred sprite]
│           ├── triceratops_locked.png  [CREATE - blurred sprite]
│           ├── diplo_locked.png        [CREATE - blurred sprite]
│           └── godzilla_locked.png     [CREATE - blurred sprite]
└── Docs/
    └── PHOTO_ALBUM_IMPLEMENTATION.md [THIS FILE]
```

---

## 💾 Key Architecture Decisions

### Persistence System
- **Technology**: PlayerPrefs (simple, sufficient for 12 photos)
- **Key Format**: `photo_{dinosaurID}_{quality}`
  - Example: `photo_archeo_blurry = 1` (unlocked)
  - Example: `photo_triceratops_perfect = 0` (locked)

### Collection Logic
- **Cumulative**: Each quality unlocks independently
- **No Overwrite**: Once unlocked, stays unlocked
- **Example**: Taking a clear photo doesn't unlock blurry/perfect automatically

### UI Design
- **Fan Layout**: 3 photos per dinosaur in horizontal spread
  - Blurry: rotation -5°, position X: -200
  - Clear: rotation 0°, position X: 0
  - Perfect: rotation +5°, position X: +200
- **Z-Order**: Blurry (back) → Clear (middle) → Perfect (front)
- **Scale**: ~0.2 on canvas (photos display at ~225x216 pixels)

---

## 📝 Code Snippets - Ready to Copy/Paste

### 1️⃣ PhotoAlbumData.cs (COMPLETE THIS FIRST)

**File**: `/Assets/Scripts/PhotoAlbumData.cs`

**Status**: You started copying this - here's the COMPLETE version to finish:

```csharp
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages saving and loading of photo album data using PlayerPrefs.
/// Tracks each photo quality independently (cumulative collection system).
/// Singleton pattern with DontDestroyOnLoad for persistence across scenes.
/// </summary>
public class PhotoAlbumData : MonoBehaviour
{
    public static PhotoAlbumData Instance;

    // Known dinosaur IDs in the game
    private List<string> knownDinosaurs = new List<string> { "archeo", "triceratops", "diplo", "godzilla" };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PhotoAlbumData] Instance created and persisted");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Save a photo to the album. Photos are cumulative (you can unlock all 3 qualities separately).
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier (e.g., "archeo", "triceratops")</param>
    /// <param name="quality">Photo quality: 1=blurry, 2=clear, 3=perfect</param>
    public void SavePhoto(string dinosaurID, int quality)
    {
        if (string.IsNullOrEmpty(dinosaurID))
        {
            Debug.LogError("[PhotoAlbumData] Cannot save photo: dinosaurID is null or empty!");
            return;
        }

        if (quality < 1 || quality > 3)
        {
            Debug.LogError($"[PhotoAlbumData] Invalid quality {quality}. Must be 1 (blurry), 2 (clear), or 3 (perfect).");
            return;
        }

        string qualityName = GetQualityName(quality);
        string key = $"photo_{dinosaurID}_{qualityName}";

        // Check if already unlocked
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            Debug.Log($"[PhotoAlbumData] {dinosaurID} {qualityName} photo already unlocked");
            return;
        }

        // Unlock this quality
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        Debug.Log($"[PhotoAlbumData] ✓ Unlocked {dinosaurID} {qualityName} photo!");
    }

    /// <summary>
    /// Check if a specific quality photo is unlocked.
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <param name="quality">Photo quality: 1=blurry, 2=clear, 3=perfect</param>
    /// <returns>True if this specific quality is unlocked</returns>
    public bool IsPhotoUnlocked(string dinosaurID, int quality)
    {
        if (string.IsNullOrEmpty(dinosaurID) || quality < 1 || quality > 3)
        {
            return false;
        }

        string qualityName = GetQualityName(quality);
        string key = $"photo_{dinosaurID}_{qualityName}";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    /// <summary>
    /// Get the number of unlocked photos for a specific dinosaur (0-3).
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <returns>Count of unlocked photos (0-3)</returns>
    public int GetUnlockedPhotoCount(string dinosaurID)
    {
        int count = 0;
        for (int quality = 1; quality <= 3; quality++)
        {
            if (IsPhotoUnlocked(dinosaurID, quality))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Check if at least one photo exists for a dinosaur.
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <returns>True if any photo quality is unlocked</returns>
    public bool HasAnyPhoto(string dinosaurID)
    {
        return GetUnlockedPhotoCount(dinosaurID) > 0;
    }

    /// <summary>
    /// Check if all 3 photos are unlocked for a dinosaur (complete collection).
    /// </summary>
    /// <param name="dinosaurID">Unique dinosaur identifier</param>
    /// <returns>True if all 3 qualities are unlocked</returns>
    public bool IsCollectionComplete(string dinosaurID)
    {
        return GetUnlockedPhotoCount(dinosaurID) == 3;
    }

    /// <summary>
    /// Get all dinosaur IDs that have at least one photo saved.
    /// </summary>
    /// <returns>List of dinosaur IDs with at least one photo</returns>
    public List<string> GetAllSavedDinosaurs()
    {
        List<string> savedDinosaurs = new List<string>();

        foreach (string dinoID in knownDinosaurs)
        {
            if (HasAnyPhoto(dinoID))
            {
                savedDinosaurs.Add(dinoID);
            }
        }

        return savedDinosaurs;
    }

    /// <summary>
    /// Clear all saved photos (for testing or reset functionality).
    /// </summary>
    public void ClearAllPhotos()
    {
        foreach (string dinoID in knownDinosaurs)
        {
            for (int quality = 1; quality <= 3; quality++)
            {
                string qualityName = GetQualityName(quality);
                string key = $"photo_{dinoID}_{qualityName}";
                if (PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }
        }

        PlayerPrefs.Save();
        Debug.Log("[PhotoAlbumData] Cleared all saved photos");
    }

    /// <summary>
    /// Get completion percentage based on total possible photos (12 = 4 dinos x 3 qualities).
    /// </summary>
    /// <returns>Percentage of all photos collected (0-100)</returns>
    public float GetCompletionPercentage()
    {
        int totalPossiblePhotos = knownDinosaurs.Count * 3; // 4 dinos x 3 qualities = 12
        int unlockedPhotos = 0;

        foreach (string dinoID in knownDinosaurs)
        {
            unlockedPhotos += GetUnlockedPhotoCount(dinoID);
        }

        return totalPossiblePhotos > 0 ? (float)unlockedPhotos / totalPossiblePhotos * 100f : 0f;
    }

    /// <summary>
    /// Get total count of unlocked photos across all dinosaurs.
    /// </summary>
    /// <returns>Total number of unlocked photos (0-12)</returns>
    public int GetTotalUnlockedPhotos()
    {
        int total = 0;
        foreach (string dinoID in knownDinosaurs)
        {
            total += GetUnlockedPhotoCount(dinoID);
        }
        return total;
    }

    /// <summary>
    /// Get count of dinosaurs with complete collections (all 3 qualities).
    /// </summary>
    /// <returns>Number of dinosaurs with complete collections (0-4)</returns>
    public int GetCompleteCollectionCount()
    {
        int count = 0;
        foreach (string dinoID in knownDinosaurs)
        {
            if (IsCollectionComplete(dinoID))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Helper method to convert quality number to string.
    /// </summary>
    private string GetQualityName(int quality)
    {
        switch (quality)
        {
            case 1: return "blurry";
            case 2: return "clear";
            case 3: return "perfect";
            default: return "unknown";
        }
    }

    /// <summary>
    /// Debug method to print all saved photos.
    /// </summary>
    public void DebugPrintAllPhotos()
    {
        Debug.Log("=== PHOTO ALBUM STATUS ===");
        foreach (string dinoID in knownDinosaurs)
        {
            int count = GetUnlockedPhotoCount(dinoID);
            string status = IsCollectionComplete(dinoID) ? "[COMPLETE]" : $"[{count}/3]";
            Debug.Log($"{dinoID} {status}:");
            Debug.Log($"  Blurry: {(IsPhotoUnlocked(dinoID, 1) ? "✓" : "✗")}");
            Debug.Log($"  Clear: {(IsPhotoUnlocked(dinoID, 2) ? "✓" : "✗")}");
            Debug.Log($"  Perfect: {(IsPhotoUnlocked(dinoID, 3) ? "✓" : "✗")}");
        }
        Debug.Log($"Total: {GetTotalUnlockedPhotos()}/12 photos ({GetCompletionPercentage():F1}%)");
    }
}
```

---

### 2️⃣ PhotoManager.cs Modifications

**File**: `/Assets/Scripts/PhotoManager.cs`

**Current method** (lines 24-28):
```csharp
public void ShowAndSavePhoto(string dinoName, Sprite photoSprite, bool isSuccess)
{
    // Display photo
    ShowPhoto(photoSprite);
}
```

**REPLACE WITH**:
```csharp
/// <summary>
/// Display and save a photo after QTE
/// </summary>
/// <param name="dinoName">Display name (not used for saving)</param>
/// <param name="photoSprite">The sprite to display</param>
/// <param name="isSuccess">Whether the QTE was successful</param>
/// <param name="isPerfect">Whether the QTE was perfect (3 stars)</param>
/// <param name="dinosaurID">Unique ID for saving (e.g., "archeo")</param>
public void ShowAndSavePhoto(string dinoName, Sprite photoSprite, bool isSuccess, bool isPerfect, string dinosaurID)
{
    // Display photo
    ShowPhoto(photoSprite);

    // Save photo to album
    if (PhotoAlbumData.Instance != null && !string.IsNullOrEmpty(dinosaurID))
    {
        int quality = 0;

        if (isSuccess)
        {
            quality = isPerfect ? 3 : 2; // Perfect = 3, Clear = 2
        }
        else
        {
            quality = 1; // Blurry = 1
        }

        PhotoAlbumData.Instance.SavePhoto(dinosaurID, quality);
        Debug.Log($"[PhotoManager] Saved photo for {dinoName} ({dinosaurID}) with quality {quality}");
    }
    else
    {
        Debug.LogWarning("[PhotoManager] Cannot save photo: PhotoAlbumData.Instance is null or dinosaurID is empty!");
    }
}
```

---

### 3️⃣ DinosaurQTE.cs Modifications

**File**: `/Assets/Scripts/QTE/DinosaurQTE.cs`

**ADD** at the top of the class (after line 32):
```csharp
[Header("Dinosaur Data")]
public DinosaurData dinosaurData; // Reference to the full DinosaurData ScriptableObject
```

**MODIFY** in `ShowPhoto()` method (around line 96):

**Current**:
```csharp
photoManager.ShowAndSavePhoto(dinoName, photoToShow, qteManager.isSuccess);
```

**REPLACE WITH**:
```csharp
// Get dinosaurID from DinosaurData, fallback to dinoName if not available
string dinosaurID = (dinosaurData != null) ? dinosaurData.dinosaurID : dinoName.ToLower();
bool isPerfect = qteManager.perfect;

photoManager.ShowAndSavePhoto(dinoName, photoToShow, qteManager.isSuccess, isPerfect, dinosaurID);
```

**Unity Inspector**: After this change, assign the DinosaurData ScriptableObject to the `dinosaurData` field in the Inspector for each QTE scene.

---

### 4️⃣ GameManager.cs Modifications

**File**: `/Assets/Scripts/GameManager.cs`

**ADD** after line 11 (after `currentDinosaurEncounter`):
```csharp
[Header("Photo Album")]
public PhotoAlbumData photoAlbumData;
```

**ADD** in `Awake()` method (after line 23):
```csharp
// Initialize PhotoAlbumData if not already present
if (photoAlbumData == null)
{
    photoAlbumData = gameObject.AddComponent<PhotoAlbumData>();
}
```

---

### 5️⃣ PhotoSlotUI.cs (NEW FILE)

**File**: `/Assets/Scripts/UI/PhotoSlotUI.cs`

**Create new file and paste**:
```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a single photo slot in the album.
/// Shows either the unlocked photo or a blurred locked version.
/// Designed for fan/spread layout with rotation.
/// </summary>
public class PhotoSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image photoImage; // The main image component

    [Header("Sprites")]
    public Sprite unlockedSprite; // The actual photo (assign in Inspector or via code)
    public Sprite lockedSprite;   // Blurred version (assign in Inspector or via code)

    [Header("Fan Layout Settings")]
    public float rotationAngle = 0f;  // Rotation for fan effect (e.g., -5°, 0°, +5°)
    public Vector2 positionOffset;    // Position offset in fan layout

    /// <summary>
    /// Setup the photo slot with unlock status.
    /// </summary>
    /// <param name="isUnlocked">Whether this photo is unlocked</param>
    public void Setup(bool isUnlocked)
    {
        if (photoImage == null)
        {
            Debug.LogError("[PhotoSlotUI] photoImage is null! Assign it in Inspector.");
            return;
        }

        if (isUnlocked && unlockedSprite != null)
        {
            // Show unlocked photo
            photoImage.sprite = unlockedSprite;
            photoImage.color = Color.white; // Full color
        }
        else if (lockedSprite != null)
        {
            // Show locked/blurred photo
            photoImage.sprite = lockedSprite;
            photoImage.color = new Color(1f, 1f, 1f, 0.7f); // Slightly transparent
        }
        else
        {
            // Fallback: show nothing or error sprite
            photoImage.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark placeholder
            Debug.LogWarning($"[PhotoSlotUI] Missing sprites for slot {gameObject.name}");
        }

        // Apply rotation for fan effect
        transform.localRotation = Quaternion.Euler(0, 0, rotationAngle);
    }

    /// <summary>
    /// Setup with explicit sprites (alternative to Inspector assignment).
    /// </summary>
    public void Setup(bool isUnlocked, Sprite unlocked, Sprite locked)
    {
        unlockedSprite = unlocked;
        lockedSprite = locked;
        Setup(isUnlocked);
    }
}
```

---

### 6️⃣ PhotoAlbumUI.cs (NEW FILE)

**File**: `/Assets/Scripts/UI/PhotoAlbumUI.cs`

**Create new file and paste**:
```csharp
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the Photo Album UI display.
/// Shows 4 dinosaurs with 3 quality slots each (12 slots total) in fan layout.
/// </summary>
public class PhotoAlbumUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject albumPanel; // Main album panel
    public TextMeshProUGUI completionText; // "X/12 Photos Collected (XX%)"

    [Header("Archeopteryx Slots")]
    public PhotoSlotUI archeoBlurry;
    public PhotoSlotUI archeoClear;
    public PhotoSlotUI archeoPerfect;

    [Header("Triceratops Slots")]
    public PhotoSlotUI triceratopsBlurry;
    public PhotoSlotUI triceratopsClear;
    public PhotoSlotUI triceratopsPerfect;

    [Header("Diplodocus Slots")]
    public PhotoSlotUI diploBlurry;
    public PhotoSlotUI diploClear;
    public PhotoSlotUI diploPerfect;

    [Header("Godzilla Slots")]
    public PhotoSlotUI godzillaBlurry;
    public PhotoSlotUI godzillaClear;
    public PhotoSlotUI godzillaPerfect;

    [Header("Dinosaur Data")]
    public DinosaurData archeoData;
    public DinosaurData triceratopsData;
    public DinosaurData diploData;
    public DinosaurData godzillaData;

    [Header("Locked Sprites (Blurred)")]
    public Sprite archeoLockedSprite;
    public Sprite triceratopsLockedSprite;
    public Sprite diploLockedSprite;
    public Sprite godzillaLockedSprite;

    void Start()
    {
        // Hide album on start
        if (albumPanel != null)
        {
            albumPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Open the photo album and populate it with current save data.
    /// </summary>
    public void OpenAlbum()
    {
        if (PhotoAlbumData.Instance == null)
        {
            Debug.LogError("[PhotoAlbumUI] Cannot open album: PhotoAlbumData.Instance is null!");
            return;
        }

        // Show panel
        if (albumPanel != null)
        {
            albumPanel.SetActive(true);
        }

        // Update header
        UpdateCompletionText();

        // Populate all slots
        PopulateAlbum();

        // Pause game and show cursor
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Close the photo album.
    /// </summary>
    public void CloseAlbum()
    {
        if (albumPanel != null)
        {
            albumPanel.SetActive(false);
        }

        // Resume game and hide cursor
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Update the completion counter text.
    /// </summary>
    private void UpdateCompletionText()
    {
        if (completionText != null)
        {
            int totalPhotos = PhotoAlbumData.Instance.GetTotalUnlockedPhotos();
            float percentage = PhotoAlbumData.Instance.GetCompletionPercentage();
            completionText.text = $"{totalPhotos}/12 Photos Collected ({percentage:F0}%)";
        }
    }

    /// <summary>
    /// Populate all 12 photo slots based on save data.
    /// </summary>
    private void PopulateAlbum()
    {
        // Archeopteryx
        SetupDinosaurSlots(
            "archeo",
            archeoData,
            archeoLockedSprite,
            archeoBlurry,
            archeoClear,
            archeoPerfect
        );

        // Triceratops
        SetupDinosaurSlots(
            "triceratops",
            triceratopsData,
            triceratopsLockedSprite,
            triceratopsBlurry,
            triceratopsClear,
            triceratopsPerfect
        );

        // Diplodocus
        SetupDinosaurSlots(
            "diplo",
            diploData,
            diploLockedSprite,
            diploBlurry,
            diploClear,
            diploPerfect
        );

        // Godzilla
        SetupDinosaurSlots(
            "godzilla",
            godzillaData,
            godzillaLockedSprite,
            godzillaBlurry,
            godzillaClear,
            godzillaPerfect
        );
    }

    /// <summary>
    /// Helper method to setup all 3 slots for one dinosaur.
    /// </summary>
    private void SetupDinosaurSlots(
        string dinosaurID,
        DinosaurData dinoData,
        Sprite lockedSprite,
        PhotoSlotUI blurrySlot,
        PhotoSlotUI clearSlot,
        PhotoSlotUI perfectSlot)
    {
        if (dinoData == null)
        {
            Debug.LogWarning($"[PhotoAlbumUI] DinosaurData is null for {dinosaurID}");
            return;
        }

        // Blurry slot (quality 1)
        if (blurrySlot != null)
        {
            bool isUnlocked = PhotoAlbumData.Instance.IsPhotoUnlocked(dinosaurID, 1);
            blurrySlot.Setup(isUnlocked, dinoData.blurryPhoto, lockedSprite);
        }

        // Clear slot (quality 2)
        if (clearSlot != null)
        {
            bool isUnlocked = PhotoAlbumData.Instance.IsPhotoUnlocked(dinosaurID, 2);
            clearSlot.Setup(isUnlocked, dinoData.clearPhoto, lockedSprite);
        }

        // Perfect slot (quality 3)
        if (perfectSlot != null)
        {
            bool isUnlocked = PhotoAlbumData.Instance.IsPhotoUnlocked(dinosaurID, 3);
            perfectSlot.Setup(isUnlocked, dinoData.perfectPhoto, lockedSprite);
        }
    }
}
```

---

### 7️⃣ MenuNavigation.cs Modifications

**File**: `/Assets/Scripts/MenuNavigation.cs`

**ADD** in `[Header("Menu Panels")]` section (after line 16):
```csharp
public GameObject photoAlbumPanel; // Photo album panel
```

**ADD** in `HideAll()` method (before the closing brace around line 298):
```csharp
if (photoAlbumPanel != null) photoAlbumPanel.SetActive(false);
```

**ADD** at the end of the class (after `QuitGame()` around line 308):
```csharp
/// <summary>
/// Show the photo album panel.
/// </summary>
public void ShowPhotoAlbum()
{
    HideAll();
    if (photoAlbumPanel != null)
    {
        photoAlbumPanel.SetActive(true);

        // Get PhotoAlbumUI component and open it
        PhotoAlbumUI albumUI = photoAlbumPanel.GetComponent<PhotoAlbumUI>();
        if (albumUI != null)
        {
            albumUI.OpenAlbum();
        }
    }
}
```

---

### 8️⃣ PhotoAlbumDebugger.cs (NEW FILE)

**File**: `/Assets/Scripts/Debug/PhotoAlbumDebugger.cs`

**Create new file and paste** (useful for testing):
```csharp
using UnityEngine;

/// <summary>
/// Debug utility for testing photo album functionality.
/// Attach to any GameObject and use hotkeys in Play mode.
/// </summary>
public class PhotoAlbumDebugger : MonoBehaviour
{
    [Header("Debug Keys")]
    [Tooltip("Press to unlock a random photo")]
    public KeyCode unlockRandomPhotoKey = KeyCode.U;

    [Tooltip("Press to clear all photos")]
    public KeyCode clearAllPhotosKey = KeyCode.C;

    [Tooltip("Press to unlock all photos")]
    public KeyCode unlockAllPhotosKey = KeyCode.P;

    [Tooltip("Press to print album status")]
    public KeyCode printStatusKey = KeyCode.I;

    private string[] dinosaurIDs = { "archeo", "triceratops", "diplo", "godzilla" };

    void Update()
    {
        if (PhotoAlbumData.Instance == null) return;

        // Unlock random photo
        if (Input.GetKeyDown(unlockRandomPhotoKey))
        {
            string randomDino = dinosaurIDs[Random.Range(0, dinosaurIDs.Length)];
            int randomQuality = Random.Range(1, 4); // 1-3
            PhotoAlbumData.Instance.SavePhoto(randomDino, randomQuality);
            Debug.Log($"[DEBUG] Unlocked {randomDino} with quality {randomQuality}");
        }

        // Clear all photos
        if (Input.GetKeyDown(clearAllPhotosKey))
        {
            PhotoAlbumData.Instance.ClearAllPhotos();
            Debug.Log("[DEBUG] Cleared all photos");
        }

        // Unlock all photos
        if (Input.GetKeyDown(unlockAllPhotosKey))
        {
            foreach (string dinoID in dinosaurIDs)
            {
                for (int quality = 1; quality <= 3; quality++)
                {
                    PhotoAlbumData.Instance.SavePhoto(dinoID, quality);
                }
            }
            Debug.Log("[DEBUG] Unlocked all photos (12 total)");
        }

        // Print status
        if (Input.GetKeyDown(printStatusKey))
        {
            PhotoAlbumData.Instance.DebugPrintAllPhotos();
        }
    }

    void OnGUI()
    {
        if (PhotoAlbumData.Instance == null) return;

        GUILayout.BeginArea(new Rect(10, 10, 350, 300));
        GUILayout.Label("=== Photo Album Debugger ===");
        GUILayout.Label($"Completion: {PhotoAlbumData.Instance.GetCompletionPercentage():F0}%");
        GUILayout.Label($"Total Photos: {PhotoAlbumData.Instance.GetTotalUnlockedPhotos()}/12");
        GUILayout.Label($"Complete Collections: {PhotoAlbumData.Instance.GetCompleteCollectionCount()}/4");

        GUILayout.Space(10);
        GUILayout.Label("Controls:");
        GUILayout.Label($"  {unlockRandomPhotoKey}: Unlock random photo");
        GUILayout.Label($"  {clearAllPhotosKey}: Clear all photos");
        GUILayout.Label($"  {unlockAllPhotosKey}: Unlock all photos");
        GUILayout.Label($"  {printStatusKey}: Print detailed status");

        GUILayout.Space(10);
        GUILayout.Label("Status by Dinosaur:");
        foreach (string dinoID in dinosaurIDs)
        {
            int count = PhotoAlbumData.Instance.GetUnlockedPhotoCount(dinoID);
            string status = PhotoAlbumData.Instance.IsCollectionComplete(dinoID) ? "[COMPLETE]" : $"[{count}/3]";
            GUILayout.Label($"  {dinoID}: {status}");
        }

        GUILayout.EndArea();
    }
}
```

---

## 🎨 Unity UI Setup Guide

### Step 1: Create Blurred Placeholder Sprites

**In Photoshop/GIMP (YOU MUST DO THIS):**
1. Open each dinosaur sprite (blurry/clear/perfect - doesn't matter which)
2. Apply **Gaussian Blur** filter (~15-20px radius)
3. Optional: Reduce opacity to 70% or add dark overlay
4. Export as PNG:
   - `archeo_locked.png`
   - `triceratops_locked.png`
   - `diplo_locked.png`
   - `godzilla_locked.png`
5. Place in `/Assets/Sprites/PhotoAlbum/Locked/`
6. Import in Unity with **100 PPU**

### Step 2: Create Album UI Hierarchy

**In Unity Hierarchy:**

```
Canvas (if not exists)
├─ PhotoAlbumPanel (Panel)
│  ├─ BackgroundImage (Image)
│  │  └─ Sprite: concept_02_vide.png
│  │
│  ├─ CompletionText (TextMeshPro)
│  │  └─ Text: "0/12 Photos Collected (0%)"
│  │
│  ├─ CloseButton (Button - TextMeshPro)
│  │  └─ OnClick: PhotoAlbumUI.CloseAlbum()
│  │
│  ├─ ArcheoGroup (Empty RectTransform)
│  │  ├─ Label (TextMeshPro): "Archeopteryx"
│  │  ├─ PhotosContainer (Empty RectTransform)
│  │  │  ├─ Photo_Blurry (Image + PhotoSlotUI)
│  │  │  │  └─ Position: (-200, 0), Rotation Z: -5°
│  │  │  ├─ Photo_Clear (Image + PhotoSlotUI)
│  │  │  │  └─ Position: (0, 0), Rotation Z: 0°
│  │  │  └─ Photo_Perfect (Image + PhotoSlotUI)
│  │  │     └─ Position: (200, 0), Rotation Z: +5°
│  │
│  ├─ TriceratopsGroup (same structure as ArcheoGroup)
│  ├─ DiploGroup (same structure)
│  └─ GodzillaGroup (same structure)
```

### Step 3: Configure PhotoSlotUI Components

**For EACH of the 12 photo slots:**
1. Add Component: **Image** (for the photo display)
2. Add Component: **PhotoSlotUI**
3. In PhotoSlotUI Inspector:
   - Photo Image: drag the Image component
   - Rotation Angle: -5° (blurry), 0° (clear), +5° (perfect)
4. Image settings:
   - Preserve Aspect: Checked
   - Raycast Target: Unchecked
   - Size: Let scale handle it

### Step 4: Configure PhotoAlbumUI Component

**On PhotoAlbumPanel:**
1. Add Component: **PhotoAlbumUI**
2. Assign all references:
   - Album Panel: PhotoAlbumPanel itself
   - Completion Text: the TextMeshPro component
   - All 12 PhotoSlotUI references (drag each slot)
   - All 4 DinosaurData ScriptableObjects from `/Assets/Data/Dinosaurs/`
   - All 4 locked sprites from `/Assets/Sprites/PhotoAlbum/Locked/`

### Step 5: Position Groups on Background

**Layout suggestions (adjust to concept_02 design):**
- **ArcheoGroup**: Top-left (~X: -400, Y: 200)
- **TriceratopsGroup**: Top-right (~X: 400, Y: 200)
- **DiploGroup**: Bottom-left (~X: -400, Y: -200)
- **GodzillaGroup**: Bottom-right (~X: 400, Y: -200)

**Scale**: Each group scale ~0.2 (so 1126x1080px photos display at ~225x216px)

Add slight rotation to groups for scrapbook feel (-2° to +2°)

### Step 6: Add Album Button to Menu

**In your main menu scene:**
1. Create Button "Photo Album"
2. OnClick: `MenuNavigation.ShowPhotoAlbum()`
3. Assign PhotoAlbumPanel to MenuNavigation's `photoAlbumPanel` field

---

## 🧪 Testing Workflow

### Phase 1: Backend Testing (After scripts 1-4)
1. Create empty GameObject "DebugManager"
2. Add `PhotoAlbumDebugger.cs` component
3. Enter Play mode
4. Press **U** to unlock random photos
5. Check Console for "[PhotoAlbumData] ✓ Unlocked..." messages
6. Press **I** to see detailed status
7. Press **C** to clear, **P** to unlock all

### Phase 2: QTE Integration Testing (After scripts 1-4)
1. Play through a QTE minigame
2. Check Console for "[PhotoManager] Saved photo..." message
3. Verify quality matches performance (fail=1, success=2, perfect=3)
4. Replay same dinosaur with different result
5. Verify photos accumulate (both should be saved)

### Phase 3: UI Testing (After scripts 5-6 + Unity setup)
1. Open album from menu
2. Verify blurred placeholders show for locked photos
3. Use debugger (U key) to unlock photos
4. Close and reopen album to see updates
5. Verify fan layout with rotations
6. Check completion counter updates

### Phase 4: Full Flow Testing
1. Clear all photos (C key in debugger)
2. Play QTE → fail (should unlock blurry)
3. Open album → verify blurry visible, others locked
4. Play same QTE → success (should unlock clear)
5. Open album → verify both blurry + clear visible
6. Play same QTE → perfect (should unlock perfect)
7. Open album → verify all 3 visible (complete collection)

---

## ✅ Checklist for Completion

### Code Implementation
- [ ] PhotoAlbumData.cs created and complete
- [ ] PhotoManager.cs modified with new signature
- [ ] DinosaurQTE.cs modified to pass dinosaurID
- [ ] GameManager.cs initializes PhotoAlbumData
- [ ] PhotoSlotUI.cs created
- [ ] PhotoAlbumUI.cs created
- [ ] MenuNavigation.cs modified with ShowPhotoAlbum()
- [ ] PhotoAlbumDebugger.cs created (optional but recommended)

### Asset Preparation
- [ ] 4 blurred sprites created in Photoshop/GIMP
- [ ] Blurred sprites imported at 100 PPU
- [ ] concept_02_vide.png set as background

### Unity Configuration
- [ ] Album UI hierarchy created
- [ ] 12 PhotoSlotUI components configured
- [ ] PhotoAlbumUI references assigned
- [ ] Fan layout positions set (-200, 0, +200)
- [ ] Rotations applied (-5°, 0°, +5°)
- [ ] Groups positioned on background
- [ ] Scale adjusted (~0.2)
- [ ] Album button added to menu
- [ ] PhotoAlbumPanel assigned to MenuNavigation

### Testing
- [ ] Backend saves photos correctly (debugger test)
- [ ] QTE integration works (play test)
- [ ] UI displays correctly (visual test)
- [ ] Locked/unlocked states work
- [ ] Cumulative collection works (multiple QTE plays)
- [ ] Completion counter accurate
- [ ] Album opens/closes properly
- [ ] Cursor/TimeScale handled correctly

---

## 🚀 Next Steps When Resuming

**If resuming on another machine:**

1. Pull latest code: `git pull origin feature/photo-album`
2. Open this file: `/Docs/PHOTO_ALBUM_IMPLEMENTATION.md`
3. Check "Implementation Status" section to see what's done
4. Continue from the first unchecked item in the checklist
5. Use code snippets above to copy/paste remaining scripts
6. Follow Unity UI Setup Guide for visual configuration
7. Test incrementally using the Testing Workflow

**Context to provide to Claude:**

> "I'm implementing a photo album system for a Unity game. Here's the progress doc: [paste/attach this file]. Continue from where I left off."

---

## 📚 Key Files Reference

- **Persistence**: `PhotoAlbumData.cs`
- **QTE Integration**: `PhotoManager.cs`, `DinosaurQTE.cs`
- **UI Display**: `PhotoAlbumUI.cs`, `PhotoSlotUI.cs`
- **Navigation**: `MenuNavigation.cs`
- **Testing**: `PhotoAlbumDebugger.cs`
- **Data**: DinosaurData ScriptableObjects in `/Assets/Data/Dinosaurs/`
- **Sprites**: `/Assets/Sprites/QTE/{DinoName}/` (real photos)
- **Locked Sprites**: `/Assets/Sprites/PhotoAlbum/Locked/` (blurred versions)

---

## 💡 Design Rationale

### Why PlayerPrefs?
- Simple, built-in to Unity
- Sufficient for 12 boolean flags
- No external dependencies
- Easy to debug (can view in Registry/plist)

### Why Cumulative Collection?
- More engaging than "best only"
- Encourages replay for completionism
- Shows progression visually
- Allows "failed but still collected" satisfaction

### Why Fan Layout?
- Visually interesting
- Scrapbook aesthetic
- Shows all 3 qualities simultaneously
- Clear visual progression when unlocking

### Why Blurred Placeholders?
- Teaser of what photo will look like
- More interesting than black silhouette
- Consistent with "camera focus" theme
- Reuses existing assets (no new art needed)

---

**Document Version**: 1.0
**Last Updated**: 2025-11-22
**Status**: PhotoAlbumData.cs in progress, backend implementation phase
