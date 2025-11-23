using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    // GameObject References
    public DinosaurQTE dinosaur;

    // UI - Text
    [Header("Text Display (Optional)")]
    public bool useTextDisplay = true;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nextKeyText;

    [Header("Timer Bar")]
    public Slider timerBarSlider;
    [Tooltip("The background image/texture in the mask container that will change color")]
    public Image timerBarBackgroundImage;
    [Tooltip("The fill image of the slider (will be made transparent)")]
    public Image timerBarFillImage;

    [Header("Timer Bar Quality Thresholds")]
    [Tooltip("Minimum time ratio (0-1) required for perfect quality")]
    [Range(0f, 1f)] public float perfectThreshold = 0.5f; // 50% or more = perfect (green)
    [Tooltip("Minimum time ratio (0-1) required for good quality")]
    [Range(0f, 1f)] public float goodThreshold = 0.2f; // 20-50% = good (orange)
    // Below 20% = bad (red) - more visible now

    public Color perfectColor = new Color(0f, 0.66f, 0.22f); // Green (#00A837)
    public Color goodColor = new Color(1f, 0.75f, 0f); // Yellow-Orange (#FFC000)
    public Color badColor = new Color(0.9f, 0.15f, 0.15f); // Red (#E62626)

    [Header("Color Transition")]
    [Tooltip("Speed of color transitions (higher = faster)")]
    public float colorTransitionSpeed = 5f;

    private Color currentBarColor;

    [Header("Countdown Display")]
    public TextMeshProUGUI countdownText;

    [Header("Result Display")]
    public Image failImage; // L'Image UI qui contient déjà ton sprite de fail
    public Image successImage; // L'Image UI qui contient ton sprite de succès
    public Image perfectImage;
    public float resultDisplayDuration = 2f;

    // UI - Key Sprites
    [Header("Key Sprite Display")]
    public Image[] keyDisplaySlots = new Image[3]; // 3 visible slots

    [Header("QWERTY Key Sprites")]
    public Sprite spriteUpQwerty;      // W key
    public Sprite spriteDownQwerty;    // S key
    public Sprite spriteLeftQwerty;    // A key
    public Sprite spriteRightQwerty;   // D key

    [Header("QWERTY Pressed Key Sprites")]
    public Sprite spriteUpPressedQwerty;
    public Sprite spriteDownPressedQwerty;
    public Sprite spriteLeftPressedQwerty;
    public Sprite spriteRightPressedQwerty;

    [Header("AZERTY Key Sprites")]
    public Sprite spriteUpAzerty;      // Z key
    public Sprite spriteDownAzerty;    // S key
    public Sprite spriteLeftAzerty;    // Q key
    public Sprite spriteRightAzerty;   // D key

    [Header("AZERTY Pressed Key Sprites")]
    public Sprite spriteUpPressedAzerty;
    public Sprite spriteDownPressedAzerty;
    public Sprite spriteLeftPressedAzerty;
    public Sprite spriteRightPressedAzerty;

    [Header("Key Display Settings")]
    public float activeKeyScale = 1.5f;
    public Color activeKeyColor = Color.white;
    public Color queuedKeyColor = new Color(1f, 1f, 1f, 0.6f);
    public float pressedDisplayDuration = 0.2f;

    [Header("Success Effect Settings")]
    public bool useSuccessEffect = true;
    public QTESuccessHalo successHaloEffect;
    public float successEffectDuration = 0.5f;

    [Header("Camera Flash Effect")]
    public Image cameraFlashImage;
    public float flashDuration = 0.3f;
    public AnimationCurve flashCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public Animator playerAnimator;


    // DBM PULL
    [Header("Base Countdown Settings")]
    [Tooltip("Base countdown duration - will be adjusted by difficulty")]
    public float baseDbmPull = 3.0f;
    float dbmPull; // Actual countdown duration after difficulty adjustment
    float dbmTimer;
    bool showingGo = false;

    // QTE TIMER
    float timeLimit;
    float timer;

    // QTE Status
    public bool isActive { get { return qteActive; } }
    bool qteActive;
    public bool isSuccess { get { return success; } }
    public bool success;

    public bool perfect = true;

    string[] possibleKeys = { "up", "down", "left", "right" };
    List<string> qteKeyList = new List<string>();
    string qteNextKey = "up";

    void Start()
    {
        // Configure timer bar direction (right to left) with white fill
        if (timerBarSlider != null)
        {
            timerBarSlider.direction = Slider.Direction.RightToLeft;

            // Make the fill white so it covers the colored background as time runs out
            if (timerBarFillImage != null)
            {
                timerBarFillImage.color = Color.white;
            }
        }

        // Apply difficulty settings to countdown duration
        if (DifficultySettings.Instance != null)
        {
            dbmPull = DifficultySettings.Instance.GetCountdownDuration();
        }
        else
        {
            dbmPull = baseDbmPull;
        }

        dbmTimer = dbmPull;

        if (useTextDisplay && nextKeyText != null)
        {
            nextKeyText.text = Mathf.CeilToInt(dbmTimer).ToString();
        }
        if (useTextDisplay && timerText != null)
        {
            timerText.text = "Get Ready...";
        }

        // Hide all key slots at start
        foreach (var slot in keyDisplaySlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }

        // Hide timer bar during countdown
        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(false);
        }

        // Show countdown at start
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = Mathf.CeilToInt(dbmTimer).ToString();
        }

        // Hide result images at start
        if (failImage != null)
        {
            failImage.gameObject.SetActive(false);
        }
        if (successImage != null)
        {
            successImage.gameObject.SetActive(false);
        }
        if (perfectImage != null)
        {
            perfectImage.gameObject.SetActive(false);
        }

        // Hide camera flash at start
        if (cameraFlashImage != null)
        {
            Color flashColor = cameraFlashImage.color;
            flashColor.a = 0f;
            cameraFlashImage.color = flashColor;
        }

        success = false;
        qteActive = false;
    }

    void Update()
    {
        // Prepull countdown
        if (!qteActive && dbmTimer > -0.5f)
        {
            dbmTimer -= Time.deltaTime;

            // Update countdown display (independent of useTextDisplay)
            if (countdownText != null)
            {
                int countdownNumber = Mathf.CeilToInt(dbmTimer);
                if (countdownNumber > 0)
                {
                    countdownText.text = countdownNumber.ToString();
                    showingGo = false;
                }
                else if (!showingGo)
                {
                    countdownText.text = "GO!";
                    showingGo = true;
                }
            }

            if (dbmTimer <= -0.5f) // Wait 0.5s after showing GO!
            {
                StartQTE();
            }
        }

        // In fight
        if (qteActive)
        {
            timer -= Time.deltaTime;

            // Clamp timer to prevent negative values
            float displayTimer = Mathf.Max(0f, timer);
            float timeRatio = displayTimer / timeLimit; // 1.0 = full time, 0.0 = no time left

            if (useTextDisplay && timerText != null)
            {
                timerText.text = $"Time left : {displayTimer.ToString("F2")} s";
            }

            // Update timer bar (only when QTE is active)
            if (timerBarSlider != null)
            {
                // Invert so fill grows as time decreases (right to left with white fill covering colored background)
                timerBarSlider.value = 1f - timeRatio; // 0 at start (no fill), 1 at end (fully covered)

                // Update timer bar background color with smooth transition
                if (timerBarBackgroundImage != null)
                {
                    Color targetColor = GetQualityColor(timeRatio);
                    currentBarColor = Color.Lerp(currentBarColor, targetColor, Time.deltaTime * colorTransitionSpeed);
                    timerBarBackgroundImage.color = currentBarColor;
                }

                // Debug: Log timer values every second
                if (Time.frameCount % 60 == 0)
                {
                    Debug.Log($"[Timer] timer={displayTimer:F2}s, timeLimit={timeLimit:F2}s, ratio={timeRatio:F2}, sliderValue={timerBarSlider.value:F2}");
                }
            }

            if (timer <= 0f)
            {
                QTEFail();
            }
        }
    }

    public void StartQTE()
    {
        // Debug.Log("StartQTE called - qteActive = true");

        // Validate dinosaur reference exists
        if (dinosaur == null)
        {
            Debug.LogError("[QTE] DinosaurQTE reference is NULL! Make sure QTESceneInitializer has configured it.");
            return;
        }

        // Apply difficulty settings to QTE parameters
        int adjustedKeyCount = dinosaur.baseKeyCount;
        float adjustedTimer = dinosaur.baseTimeLimit;

        if (DifficultySettings.Instance != null)
        {
            adjustedKeyCount = DifficultySettings.Instance.CalculateKeyCount(dinosaur.baseKeyCount);
            adjustedTimer = DifficultySettings.Instance.CalculateQTETimer(dinosaur.baseTimeLimit);
            Debug.Log($"[QTE] Difficulty: {DifficultySettings.Instance.GetDifficulty()} | Base: {dinosaur.baseKeyCount} keys, {dinosaur.baseTimeLimit}s | Adjusted: {adjustedKeyCount} keys, {adjustedTimer}s");
        }
        else
        {
            Debug.LogWarning("[QTE] DifficultySettings.Instance is NULL! Using base values.");
        }

        GenerateQTEKeyList(adjustedKeyCount);

        timeLimit = adjustedTimer;
        timer = adjustedTimer;
        Debug.Log($"[QTE] Timer initialized: timer={timer:F2}s, timeLimit={timeLimit:F2}s");
        qteActive = true;
        success = false;
        perfect = true;

        GetNextKey();

        if (useTextDisplay && nextKeyText != null)
        {
            nextKeyText.gameObject.SetActive(true);
        }
        if (useTextDisplay && timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        // Show timer bar and force immediate update
        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(true);
            timerBarSlider.value = 0f; // Start empty (no white fill, background fully visible)

            // Set initial color (green - perfect)
            if (timerBarBackgroundImage != null)
            {
                currentBarColor = GetQualityColor(1f);
                timerBarBackgroundImage.color = currentBarColor;
            }

            // Force immediate visual update
            timerBarSlider.value = 0.01f; // Tiny value to force Unity to update
            timerBarSlider.value = 0f; // Back to 0
        }

        // Hide countdown when QTE starts
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    void GenerateQTEKeyList(int length)
    {
        bool preventDuplicates = DifficultySettings.Instance != null &&
                                 DifficultySettings.Instance.ShouldPreventConsecutiveDuplicates();

        string lastKey = null;

        while (length > 0)
        {
            length--;
            string chosenKey;

            if (preventDuplicates && lastKey != null)
            {
                // Prevent consecutive duplicates by trying again if same key is chosen
                int maxAttempts = 10; // Prevent infinite loop
                int attempts = 0;
                do
                {
                    chosenKey = possibleKeys[Random.Range(0, possibleKeys.Count())];
                    attempts++;
                }
                while (chosenKey == lastKey && attempts < maxAttempts);
            }
            else
            {
                // No restriction, pick randomly
                chosenKey = possibleKeys[Random.Range(0, possibleKeys.Count())];
            }

            qteKeyList.Add(chosenKey);
            lastKey = chosenKey;
        }
    }

    bool GetNextKey()
    {
        if (qteKeyList.Count > 0)
        {
            qteNextKey = qteKeyList[0];
            qteKeyList.RemoveAt(0);
            UpdateKeyPreview();
            return true;
        }
        return false;
    }

    void UpdateKeyPreview()
    {
        // Update text (optional)
        if (useTextDisplay && nextKeyText != null)
        {
            string previewText = qteNextKey;
            if (qteKeyList.Count > 0)
            {
                previewText += $" - {qteKeyList[0]}";
            }
            if (qteKeyList.Count > 1)
            {
                previewText += $" - {qteKeyList[1]}";
            }
            nextKeyText.text = previewText;
        }

        // Update sprite displays
        UpdateKeySpriteDisplay();
    }

    void UpdateKeySpriteDisplay()
    {
        // Build list of keys to display: current key + next few queued keys (sliding window)
        List<string> keysToDisplay = new List<string> { qteNextKey };

        // Add only as many upcoming keys as we have slots for (minus 1 for current key)
        int upcomingKeysToShow = Mathf.Min(keyDisplaySlots.Length - 1, qteKeyList.Count);
        for (int i = 0; i < upcomingKeysToShow; i++)
        {
            keysToDisplay.Add(qteKeyList[i]);
        }

        // Update each slot
        for (int i = 0; i < keyDisplaySlots.Length; i++)
        {
            if (keyDisplaySlots[i] == null) continue;

            if (i < keysToDisplay.Count)
            {
                // Show key sprite
                keyDisplaySlots[i].gameObject.SetActive(true);
                keyDisplaySlots[i].sprite = GetSpriteForKey(keysToDisplay[i]);

                // First key is active (larger, full opacity)
                if (i == 0)
                {
                    keyDisplaySlots[i].transform.localScale = Vector3.one * 0.45f * activeKeyScale;
                    keyDisplaySlots[i].color = activeKeyColor;
                }
                else
                {
                    // Queued keys are smaller and semi-transparent
                    keyDisplaySlots[i].transform.localScale = Vector3.one * 0.45f;
                    keyDisplaySlots[i].color = queuedKeyColor;
                }
            }
            else
            {
                // Hide unused slots
                keyDisplaySlots[i].gameObject.SetActive(false);
            }
        }
    }

    Sprite GetSpriteForKey(string key, bool pressed = false)
    {
        // Determine current keyboard layout
        bool isQwerty = InputManager.Instance == null ||
                        InputManager.Instance.currentLayout == InputManager.KeyboardLayout.QWERTY;

        if (pressed)
        {
            if (isQwerty)
            {
                return key.ToLower() switch
                {
                    "up" => spriteUpPressedQwerty,
                    "down" => spriteDownPressedQwerty,
                    "left" => spriteLeftPressedQwerty,
                    "right" => spriteRightPressedQwerty,
                    _ => null
                };
            }
            else // AZERTY
            {
                return key.ToLower() switch
                {
                    "up" => spriteUpPressedAzerty,
                    "down" => spriteDownPressedAzerty,
                    "left" => spriteLeftPressedAzerty,
                    "right" => spriteRightPressedAzerty,
                    _ => null
                };
            }
        }
        else
        {
            if (isQwerty)
            {
                return key.ToLower() switch
                {
                    "up" => spriteUpQwerty,
                    "down" => spriteDownQwerty,
                    "left" => spriteLeftQwerty,
                    "right" => spriteRightQwerty,
                    _ => null
                };
            }
            else // AZERTY
            {
                return key.ToLower() switch
                {
                    "up" => spriteUpAzerty,
                    "down" => spriteDownAzerty,
                    "left" => spriteLeftAzerty,
                    "right" => spriteRightAzerty,
                    _ => null
                };
            }
        }
    }

    void QTESuccess()
    {
        success = true;
        qteActive = false;

        if (useTextDisplay && nextKeyText != null)
        {
            nextKeyText.text = "Success!";
        }

        // Set score on the current dinosaur
        if (dinosaur != null)
        {
            dinosaur.score = perfect ? 3 : 2;
        }
        else
        {
            Debug.LogError("[QTE] Cannot set score - dinosaur reference is null!");
        }

        Invoke(nameof(ExitQTE), 1.0f);
    }

    void QTESuccessComplete()
    {
        // Mark as success
        success = true;
        qteActive = false;

        // Hide all key slots after brief delay
        StartCoroutine(HideKeySlotsAfterDelay());

        // Hide timer bar
        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(false);
        }

        if (perfect)
        {
            // Show success image
            if (perfectImage != null)
            {
                // Debug.Log("Showing success image!");
                perfectImage.gameObject.SetActive(true);
                Invoke(nameof(HideResultImages), resultDisplayDuration);
            }
            else
            {
                Debug.LogWarning("Success image is NULL!");
            }
        }
        else
        {
            // Show success image
            if (successImage != null)
            {
                // Debug.Log("Showing success image!");
                successImage.gameObject.SetActive(true);
                Invoke(nameof(HideResultImages), resultDisplayDuration);
            }
            else
            {
                // Debug.LogWarning("Success image is NULL!");
            }
        }

        if (useTextDisplay && nextKeyText != null)
        {
            nextKeyText.text = "Success!";
        }
    }

    IEnumerator HideKeySlotsAfterDelay()
    {
        // Wait for last halo effect to finish
        yield return new WaitForSeconds(successEffectDuration);

        // Hide all key slots
        foreach (var slot in keyDisplaySlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    void QTEFail()
    {
        qteActive = false;

        if (useTextDisplay && nextKeyText != null)
        {
            nextKeyText.text = "Failed!";
        }

        // Hide timer bar
        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(false);
        }

        // Hide key slots
        foreach (var slot in keyDisplaySlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }

        // Show fail image
        if (failImage != null)
        {
            failImage.gameObject.SetActive(true);
            Invoke(nameof(HideResultImages), resultDisplayDuration);
        }
    }

    void HideResultImages()
    {
        if (failImage != null)
        {
            failImage.gameObject.SetActive(false);
        }
        if (successImage != null)
        {
            successImage.gameObject.SetActive(false);
        }
        if (perfectImage != null)
        {
            perfectImage.gameObject.SetActive(false);
        }
        // nextKeyText.text = "Failed!";
        // GameManager.Instance.Dino1.score = 1;
        // SceneTransition.Instance.TransitionToScene("Level 1");
        Invoke(nameof(ExitQTE), 1.0f);
    }

    void ExitQTE()
    {
        SceneTransition.Instance.TransitionToScene("Level 1");
    }

    public void DoQTE(string key)
    {
        if (key == qteNextKey)
        {
            // Show pressed sprite feedback and advance to next key
            StartCoroutine(DoQTESuccessCoroutine());
        }
        else
        {
            // Wrong key pressed - could add shake/red effect here later
            perfect = false;
            ShowFailedFeedback();
        }
    }

    IEnumerator DoQTESuccessCoroutine()
    {
        string currentKey = qteNextKey;
        bool isLastKey = (qteKeyList.Count == 0);

        // Show pressed sprite
        if (keyDisplaySlots.Length > 0 && keyDisplaySlots[0] != null)
        {
            Sprite pressedSprite = GetSpriteForKey(currentKey, pressed: true);
            Debug.Log($"[QTE] Showing pressed sprite for '{currentKey}' - Sprite: {(pressedSprite != null ? pressedSprite.name : "NULL")}");
            keyDisplaySlots[0].sprite = pressedSprite;

            // Spawn halo effect on successful key press
            if (useSuccessEffect && successHaloEffect != null)
            {
                successHaloEffect.SpawnHalo(keyDisplaySlots[0].transform);
            }
        }

        // Wait for feedback duration
        yield return new WaitForSeconds(pressedDisplayDuration);

        // Advance to next key
        if (!GetNextKey())
        {
            // This was the last key - trigger player animation, camera flash and then success
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Success");
            }

            if (cameraFlashImage != null)
            {
                StartCoroutine(CameraFlashCoroutine());
            }
            QTESuccessComplete();
        }
    }

    IEnumerator CameraFlashCoroutine()
    {
        if (cameraFlashImage == null) yield break;

        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;
            float alpha = flashCurve.Evaluate(t);

            Color flashColor = cameraFlashImage.color;
            flashColor.a = alpha;
            cameraFlashImage.color = flashColor;

            yield return null;
        }

        // Ensure flash is fully hidden at the end
        Color finalColor = cameraFlashImage.color;
        finalColor.a = 0f;
        cameraFlashImage.color = finalColor;
    }

    void ShowFailedFeedback()
    {
        // Placeholder for failed input feedback
        // TODO: Add shake animation, red flash, or particle effect
        // Debug.Log("Wrong key pressed!");
    }

    /// <summary>
    /// Stop the QTE completely
    /// </summary>
    public void StopQTE()
    {
        // Debug.Log("Stopping QTE...");

        qteActive = false;
        success = false;

        // Hide all UI elements
        HideResultImages();

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(false);
        }

        foreach (var slot in keyDisplaySlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Restart the QTE from the beginning
    /// </summary>
    public void RestartQTE()
    {
        // Debug.Log("Restarting QTE...");

        // Reapply difficulty settings to countdown duration
        if (DifficultySettings.Instance != null)
        {
            dbmPull = DifficultySettings.Instance.GetCountdownDuration();
        }
        else
        {
            dbmPull = baseDbmPull;
        }

        // Reset timer
        dbmTimer = dbmPull;
        showingGo = false;

        // Reset QTE state
        success = false;
        qteActive = false;

        // Clear key list
        qteKeyList.Clear();

        // Hide result images
        HideResultImages();

        // Reset UI
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = Mathf.CeilToInt(dbmTimer).ToString();
        }

        // Hide key slots
        foreach (var slot in keyDisplaySlots)
        {
            if (slot != null)
            {
                slot.gameObject.SetActive(false);
            }
        }

        // Hide timer bar
        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Refresh key sprites when keyboard layout changes
    /// Called by InputManager when layout is switched
    /// </summary>
    public void RefreshKeySprites()
    {
        // Update the sprite display to use new layout sprites
        UpdateKeySpriteDisplay();
    }

    /// <summary>
    /// Get the color for the timer bar based on quality thresholds
    /// </summary>
    Color GetQualityColor(float timeRatio)
    {
        if (timeRatio >= perfectThreshold)
        {
            return perfectColor; // Green - Perfect quality
        }
        else if (timeRatio >= goodThreshold)
        {
            return goodColor; // Orange - Good quality
        }
        else
        {
            return badColor; // Red - Bad quality
        }
    }
}
