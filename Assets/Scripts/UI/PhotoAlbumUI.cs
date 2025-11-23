using UnityEngine;
using TMPro;
using System.Collections;

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

    void Awake()
    {
        // If albumPanel not assigned, assume this script is on the panel itself
        if (albumPanel == null)
        {
            albumPanel = gameObject;
        }
    }

    /// <summary>
    /// Reset all collected photos (for "Reset Progress" feature).
    /// Call this from a settings/options button.
    /// </summary>
    public void ResetAllPhotos()
    {
        if (PhotoAlbumData.Instance != null)
        {
            PhotoAlbumData.Instance.ClearAllPhotos();
            Debug.Log("[PhotoAlbumUI] All photos cleared - progress reset!");

            // If album is currently open, refresh the UI
            if (albumPanel != null && albumPanel.activeSelf)
            {
                UpdateCompletionText();
                PopulateAlbum();
            }
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

        StartCoroutine(OpenAlbumCoroutine());
    }

    private IEnumerator OpenAlbumCoroutine()
    {
        Debug.Log("[PhotoAlbumUI] Opening album...");

        // Wait one frame for UI to initialize
        yield return null;

        // Update header
        UpdateCompletionText();

        // Populate all slots
        PopulateAlbum();

        // Pause game and show cursor
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("[PhotoAlbumUI] Album opened successfully");
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

        // Brachiosaurus (formerly Diplodocus)
        SetupDinosaurSlots(
            "brachiosaurus",
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