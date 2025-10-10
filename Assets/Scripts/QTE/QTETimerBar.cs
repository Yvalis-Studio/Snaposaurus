using UnityEngine;
using UnityEngine.UI;

public class QTETimerBar : MonoBehaviour
{
    [Header("Bar Components")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image borderImage;

    [Header("Fill Settings")]
    [SerializeField] private bool scrollTexture = true;
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private Image.FillMethod fillMethod = Image.FillMethod.Horizontal;

    [Header("Color Settings")]
    [SerializeField] private bool useColorGradient = true;
    [SerializeField] private Color fullColor = Color.green;
    [SerializeField] private Color halfColor = Color.yellow;
    [SerializeField] private Color lowColor = Color.red;

    private Material fillMaterial;
    private float currentFillAmount = 1f;

    void Awake()
    {
        // Create a material instance for texture scrolling
        if (fillImage != null && scrollTexture)
        {
            fillMaterial = new Material(fillImage.material);
            fillImage.material = fillMaterial;
        }
    }

    void Update()
    {
        // Scroll texture if enabled
        if (scrollTexture && fillMaterial != null)
        {
            Vector2 offset = fillMaterial.mainTextureOffset;
            offset.x += scrollSpeed * Time.deltaTime;
            fillMaterial.mainTextureOffset = offset;
        }
    }

    /// <summary>
    /// Set the fill amount (0 to 1)
    /// </summary>
    public void SetFillAmount(float amount)
    {
        currentFillAmount = Mathf.Clamp01(amount);

        if (fillImage != null)
        {
            fillImage.fillAmount = currentFillAmount;

            // Update color based on fill amount
            if (useColorGradient)
            {
                if (currentFillAmount > 0.5f)
                {
                    // Interpolate between full and half
                    float t = (currentFillAmount - 0.5f) * 2f;
                    fillImage.color = Color.Lerp(halfColor, fullColor, t);
                }
                else
                {
                    // Interpolate between low and half
                    float t = currentFillAmount * 2f;
                    fillImage.color = Color.Lerp(lowColor, halfColor, t);
                }
            }
        }
    }

    /// <summary>
    /// Update timer display (current time / max time)
    /// </summary>
    public void UpdateTimer(float currentTime, float maxTime)
    {
        float fillAmount = maxTime > 0 ? currentTime / maxTime : 0f;
        SetFillAmount(fillAmount);
    }

    /// <summary>
    /// Show the timer bar
    /// </summary>
    public void Show()
    {
        Debug.Log("QTETimerBar.Show() called");
        gameObject.SetActive(true);
        SetFillAmount(1f);
    }

    /// <summary>
    /// Hide the timer bar
    /// </summary>
    public void Hide()
    {
        Debug.Log("QTETimerBar.Hide() called");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Reset the timer bar to full
    /// </summary>
    public void Reset()
    {
        SetFillAmount(1f);
    }

    void OnDestroy()
    {
        // Clean up material instance
        if (fillMaterial != null)
        {
            Destroy(fillMaterial);
        }
    }
}
