using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image backgroundImage;

    [Header("Animation Settings")]
    [SerializeField] private bool pulseOnChange = true;
    [SerializeField] private float pulseScale = 1.5f;
    [SerializeField] private float pulseSpeed = 8f;
    [SerializeField] private float backgroundFadeDelay = 0.5f; // Delay before background starts fading
    [SerializeField] private float backgroundFadeDuration = 0.5f; // How long the fade takes

    [Header("Text Color Settings")]
    [SerializeField] private bool changeColor = true;
    [SerializeField] private Color textColor3 = Color.red;
    [SerializeField] private Color textColor2 = new Color(1f, 0.5f, 0f); // Orange
    [SerializeField] private Color textColor1 = Color.yellow;
    [SerializeField] private Color textColorGo = Color.green;

    [Header("Background Color Settings")]
    [SerializeField] private Color bgColor3 = new Color(0.2f, 0f, 0f, 0.8f); // Dark red with transparency
    [SerializeField] private Color bgColor2 = new Color(0.2f, 0.1f, 0f, 0.8f); // Dark orange with transparency
    [SerializeField] private Color bgColor1 = new Color(0.2f, 0.2f, 0f, 0.8f); // Dark yellow with transparency
    [SerializeField] private Color bgColorGo = new Color(0f, 0.2f, 0f, 0.8f); // Dark green with transparency

    private TextMeshProUGUI textMesh;
    private string lastText = "";
    private Vector3 originalScale;
    private float pulseTime = 0f;
    private float backgroundFadeTimer = -1f;
    private Color originalBgColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalScale = transform.localScale;
    }

    void OnDisable()
    {
        // If this GameObject gets disabled, also disable the background
        if (backgroundImage != null && backgroundImage.gameObject.activeSelf)
        {
            backgroundImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (textMesh == null) return;

        // Detect text change
        if (textMesh.text != lastText)
        {
            lastText = textMesh.text;
            OnTextChanged();
        }

        // Pulse animation
        if (pulseOnChange && pulseTime > 0)
        {
            pulseTime -= Time.deltaTime * pulseSpeed;
            float scale = Mathf.Lerp(1f, pulseScale, pulseTime);
            transform.localScale = originalScale * scale;
        }
        else
        {
            transform.localScale = originalScale;
        }

        // Background fade animation
        if (backgroundFadeTimer >= 0f && backgroundImage != null)
        {
            backgroundFadeTimer += Time.deltaTime;

            if (backgroundFadeTimer >= backgroundFadeDelay)
            {
                float fadeProgress = (backgroundFadeTimer - backgroundFadeDelay) / backgroundFadeDuration;
                fadeProgress = Mathf.Clamp01(fadeProgress);

                Color currentColor = originalBgColor;
                currentColor.a = Mathf.Lerp(originalBgColor.a, 0f, fadeProgress);
                backgroundImage.color = currentColor;

                // Disable the background GameObject once fade is complete
                if (fadeProgress >= 1f)
                {
                    backgroundFadeTimer = -1f;
                    backgroundImage.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnTextChanged()
    {
        // Trigger pulse
        if (pulseOnChange)
        {
            pulseTime = 1f;
        }

        // Change color based on text
        if (changeColor && textMesh != null)
        {
            Color targetTextColor = Color.white;
            Color targetBgColor = Color.white;

            switch (lastText)
            {
                case "3":
                    targetTextColor = textColor3;
                    targetBgColor = bgColor3;
                    break;
                case "2":
                    targetTextColor = textColor2;
                    targetBgColor = bgColor2;
                    break;
                case "1":
                    targetTextColor = textColor1;
                    targetBgColor = bgColor1;
                    break;
                case "GO!":
                    targetTextColor = textColorGo;
                    targetBgColor = bgColorGo;
                    break;
            }

            textMesh.color = targetTextColor;

            // Apply separate color to background image if assigned
            if (backgroundImage != null)
            {
                backgroundImage.color = targetBgColor;
                originalBgColor = targetBgColor;

                // Start fade timer when "GO!" appears
                if (lastText == "GO!")
                {
                    backgroundFadeTimer = 0f;
                }
            }
        }
    }
}
