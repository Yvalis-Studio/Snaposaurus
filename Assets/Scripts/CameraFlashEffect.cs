using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFlashEffect : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private int pulseCount = 3;
    [SerializeField] private float pulseDuration = 0.15f;
    [SerializeField] private float delayBetweenPulses = 0.1f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Scale Settings")]
    [SerializeField] private bool scaleWithFlash = true;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 1.5f);

    [Header("Loop Settings")]
    [SerializeField] private bool loopOnStart = true;
    [SerializeField] private float delayBetweenLoops = 2f;

    private Image flashImage;
    private Vector3 originalScale;
    private bool isFlashing = false;
    private bool shouldLoop = false;

    void Awake()
    {
        flashImage = GetComponent<Image>();
        if (flashImage == null)
        {
            Debug.LogError("CameraFlashEffect requires an Image component!");
            return;
        }

        originalScale = transform.localScale;

        // Start invisible
        Color col = flashImage.color;
        col.a = 0f;
        flashImage.color = col;
    }

    void OnEnable()
    {
        if (loopOnStart)
        {
            StartLooping();
        }
    }

    void OnDisable()
    {
        StopLooping();
    }

    /// <summary>
    /// Trigger the flash effect
    /// </summary>
    public void TriggerFlash()
    {
        if (!isFlashing && flashImage != null)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    /// <summary>
    /// Trigger flash with custom pulse count
    /// </summary>
    public void TriggerFlash(int customPulseCount)
    {
        if (!isFlashing && flashImage != null)
        {
            StartCoroutine(FlashCoroutine(customPulseCount));
        }
    }

    private IEnumerator FlashCoroutine(int customPulseCount = -1)
    {
        isFlashing = true;
        int pulses = customPulseCount > 0 ? customPulseCount : pulseCount;

        for (int i = 0; i < pulses; i++)
        {
            // Flash in
            yield return StartCoroutine(PulseFlash(true));

            // Flash out
            yield return StartCoroutine(PulseFlash(false));

            // Delay between pulses (except on last pulse)
            if (i < pulses - 1)
            {
                yield return new WaitForSeconds(delayBetweenPulses);
            }
        }

        isFlashing = false;
    }

    private IEnumerator PulseFlash(bool fadeIn)
    {
        float elapsed = 0f;

        while (elapsed < pulseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pulseDuration;

            if (!fadeIn)
            {
                t = 1f - t;
            }

            // Apply intensity curve
            float curveValue = intensityCurve.Evaluate(t);

            // Update alpha
            Color col = flashImage.color;
            col.a = curveValue * maxIntensity;
            flashImage.color = col;

            // Update scale if enabled
            if (scaleWithFlash)
            {
                float scaleValue = scaleCurve.Evaluate(t);
                transform.localScale = originalScale * scaleValue;
            }

            yield return null;
        }

        // Ensure final values
        if (!fadeIn)
        {
            Color col = flashImage.color;
            col.a = 0f;
            flashImage.color = col;

            if (scaleWithFlash)
            {
                transform.localScale = originalScale;
            }
        }
    }

    /// <summary>
    /// Stop flash effect immediately
    /// </summary>
    public void StopFlash()
    {
        StopAllCoroutines();
        isFlashing = false;

        Color col = flashImage.color;
        col.a = 0f;
        flashImage.color = col;

        transform.localScale = originalScale;
    }

    /// <summary>
    /// Start looping the flash effect
    /// </summary>
    public void StartLooping()
    {
        shouldLoop = true;
        if (!isFlashing)
        {
            StartCoroutine(LoopFlashCoroutine());
        }
    }

    /// <summary>
    /// Stop looping the flash effect
    /// </summary>
    public void StopLooping()
    {
        shouldLoop = false;
        StopFlash();
    }

    private IEnumerator LoopFlashCoroutine()
    {
        while (shouldLoop)
        {
            yield return StartCoroutine(FlashCoroutine());
            yield return new WaitForSeconds(delayBetweenLoops);
        }
    }

    public bool IsFlashing()
    {
        return isFlashing;
    }
}
