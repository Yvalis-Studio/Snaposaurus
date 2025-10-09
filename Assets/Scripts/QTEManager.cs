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
    public float activeKeyScale = 1.2f;
    public Color activeKeyColor = Color.white;
    public Color queuedKeyColor = new Color(1f, 1f, 1f, 0.6f);
    public float pressedDisplayDuration = 0.2f;

    [Header("Success Effect Settings")]
    public bool useSuccessEffect = true;
    public QTESuccessHalo successHaloEffect;
    public float successEffectDuration = 0.5f;


    // DBM PULL
    public float dbmPull = 3.0f;
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

            if (useTextDisplay && timerText != null)
            {
                timerText.text = $"Time left : {timer.ToString("F2")} s";
            }

            // Update timer bar (only when QTE is active)
            if (timerBarSlider != null)
            {
                timerBarSlider.value = 1f - (timer / timeLimit); // Inverse: vide quand timer diminue
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
        GenerateQTEKeyList(dinosaur.qteLength);

        timeLimit = dinosaur.qteTimer;
        timer = dinosaur.qteTimer;
        // Debug.Log($"Timer initialized: timer={timer}, timeLimit={timeLimit}");
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

        // Show timer bar
        if (timerBarSlider != null)
        {
            timerBarSlider.gameObject.SetActive(true);
            timerBarSlider.value = 0f; // Start vide (blanc)
        }

        // Hide countdown when QTE starts
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    void GenerateQTEKeyList(int length)
    {
        while (length > 0)
        {
            length--;
            string chosenKey = possibleKeys[Random.Range(0, possibleKeys.Count() - 1)];
            qteKeyList.Add(chosenKey);
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
        // Build list of keys to display: current key + queued keys
        List<string> keysToDisplay = new List<string> { qteNextKey };
        keysToDisplay.AddRange(qteKeyList);

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
                    keyDisplaySlots[i].transform.localScale = Vector3.one * activeKeyScale;
                    keyDisplaySlots[i].color = activeKeyColor;
                }
                else
                {
                    // Queued keys are smaller and semi-transparent
                    keyDisplaySlots[i].transform.localScale = Vector3.one;
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
        // Invoke(nameof(HidePrompt), 0.5f);
        if (perfect)
        {
            GameManager.Instance.Dino1.score = 3;
        }
        else
        {
            GameManager.Instance.Dino1.score = 2;
        }
        // SceneTransition.Instance.TransitionToScene("Level 1");
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
            // This was the last key - trigger success
            QTESuccessComplete();
        }
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
}
