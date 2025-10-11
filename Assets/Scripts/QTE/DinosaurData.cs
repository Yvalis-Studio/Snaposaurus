using UnityEngine;

/// <summary>
/// ScriptableObject that stores all data for a specific dinosaur's QTE encounter.
/// Create one asset per dinosaur (triceratops, archeo, diplo, godzilla, etc.)
/// </summary>
[CreateAssetMenu(fileName = "New Dinosaur", menuName = "Snaposaurus/Dinosaur Data")]
public class DinosaurData : ScriptableObject
{
    [Header("Dinosaur Identity")]
    [Tooltip("Display name of the dinosaur (e.g., 'Triceratops')")]
    public string dinosaurName = "T-Rex";

    [Tooltip("Unique ID for this dinosaur (e.g., 'triceratops', 'archeo')")]
    public string dinosaurID = "trex";

    [Header("Visual Appearance")]
    [Tooltip("The dinosaur sprite/prefab to show during QTE")]
    public GameObject dinosaurPrefab;

    [Tooltip("Alternative: Just the sprite if using a generic prefab")]
    public Sprite dinosaurSprite;

    [Header("QTE Difficulty Settings")]
    [Tooltip("BASE time limit in seconds. Difficulty multiplies this (Easy: 1.5x, Normal: 1.0x, Hard: 0.75x)")]
    public float baseTimeLimit = 5f;

    [Tooltip("BASE number of keys to press. Difficulty adds to this (Easy: +0, Normal: +2, Hard: +4)")]
    public int baseKeyCount = 3;

    [Header("Photo Sprites")]
    [Tooltip("Photo sprite shown when player gets perfect score")]
    public Sprite perfectPhoto;

    [Tooltip("Photo sprite shown when player succeeds")]
    public Sprite clearPhoto;

    [Tooltip("Photo sprite shown when player fails")]
    public Sprite blurryPhoto;

    [Header("Optional: Custom Settings")]
    [Tooltip("Override the player sprite for this specific encounter (leave null to use default)")]
    public Sprite customPlayerSprite;
}
