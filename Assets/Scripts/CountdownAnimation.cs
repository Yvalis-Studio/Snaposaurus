using UnityEngine;
using TMPro;

public class CountdownAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private bool pulseOnChange = true;
    [SerializeField] private float pulseScale = 1.5f;
    [SerializeField] private float pulseSpeed = 8f;

    [Header("Color Settings")]
    [SerializeField] private bool changeColor = true;
    [SerializeField] private Color color3 = Color.red;
    [SerializeField] private Color color2 = new Color(1f, 0.5f, 0f); // Orange
    [SerializeField] private Color color1 = Color.yellow;
    [SerializeField] private Color colorGo = Color.green;

    private TextMeshProUGUI textMesh;
    private string lastText = "";
    private Vector3 originalScale;
    private float pulseTime = 0f;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalScale = transform.localScale;
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
            switch (lastText)
            {
                case "3":
                    textMesh.color = color3;
                    break;
                case "2":
                    textMesh.color = color2;
                    break;
                case "1":
                    textMesh.color = color1;
                    break;
                case "GO!":
                    textMesh.color = colorGo;
                    break;
            }
        }
    }
}
