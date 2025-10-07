using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BackButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation Settings")]
    [SerializeField] private AnimationType animType = AnimationType.RotateAndScale;
    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float hoverScale = 1.2f;

    [Header("Sprite Swap Settings")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;

    [Header("Keyboard Input Settings")]
    [SerializeField] private string triggerKey = "";
    [SerializeField] private Sprite keyPressedSprite;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private bool isHovering = false;
    private bool isKeyPressed = false;
    private Image imageComponent;

    public enum AnimationType
    {
        RotateAndScale,      // Rotation + agrandissement
        Pulse,               // Pulsation
        Shake,               // Tremblement
        RotateContinuous,    // Rotation continue
        SpriteSwap           // Échange de sprites
    }
    
    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
        imageComponent = GetComponent<Image>();
    }
    
    void Update()
    {
        // Check for keyboard input
        CheckKeyboardInput();

        switch (animType)
        {
            case AnimationType.RotateAndScale:
                AnimateRotateAndScale();
                break;
            case AnimationType.Pulse:
                AnimatePulse();
                break;
            case AnimationType.Shake:
                AnimateShake();
                break;
            case AnimationType.RotateContinuous:
                AnimateRotateContinuous();
                break;
            case AnimationType.SpriteSwap:
                // Sprite swap happens in OnPointerEnter/Exit and keyboard input
                break;
        }
    }

    void CheckKeyboardInput()
    {
        if (string.IsNullOrEmpty(triggerKey)) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        bool keyCurrentlyPressed = IsKeyPressed(keyboard, triggerKey.ToLower());

        // Handle key press/release for sprite swap
        if (keyCurrentlyPressed && !isKeyPressed)
        {
            isKeyPressed = true;
            ActivateKeyEffect();
        }
        else if (!keyCurrentlyPressed && isKeyPressed)
        {
            isKeyPressed = false;
            DeactivateKeyEffect();
        }
    }

    bool IsKeyPressed(Keyboard keyboard, string key)
    {
        return key switch
        {
            "z" => keyboard.zKey.isPressed,
            "q" => keyboard.qKey.isPressed,
            "w" => keyboard.wKey.isPressed,
            "a" => keyboard.aKey.isPressed,
            "s" => keyboard.sKey.isPressed,
            "d" => keyboard.dKey.isPressed,
            "esc" or "escape" => keyboard.escapeKey.isPressed,
            "space" => keyboard.spaceKey.isPressed,
            "e" => keyboard.eKey.isPressed,
            _ => false
        };
    }

    void ActivateHoverEffect()
    {
        if (animType == AnimationType.SpriteSwap && imageComponent != null && hoverSprite != null)
        {
            imageComponent.sprite = hoverSprite;
        }
    }

    void DeactivateHoverEffect()
    {
        if (!isHovering && !isKeyPressed && animType == AnimationType.SpriteSwap && imageComponent != null && normalSprite != null)
        {
            imageComponent.sprite = normalSprite;
        }
    }

    void ActivateKeyEffect()
    {
        if (animType == AnimationType.SpriteSwap && imageComponent != null && keyPressedSprite != null)
        {
            imageComponent.sprite = keyPressedSprite;
        }
    }

    void DeactivateKeyEffect()
    {
        if (!isHovering && animType == AnimationType.SpriteSwap && imageComponent != null && normalSprite != null)
        {
            imageComponent.sprite = normalSprite;
        }
        else if (isHovering && animType == AnimationType.SpriteSwap && imageComponent != null && hoverSprite != null)
        {
            imageComponent.sprite = hoverSprite;
        }
    }
    
    void AnimateRotateAndScale()
    {
        // Scale
        Vector3 targetScale = isHovering ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);

        // Rotation
        float targetRotation = isHovering ? 90f : 0f;
        Quaternion target = originalRotation * Quaternion.Euler(0, 0, targetRotation);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.unscaledDeltaTime * animationSpeed);
    }
    
    void AnimatePulse()
    {
        if (isHovering)
        {
            float pulse = 1f + Mathf.Sin(Time.unscaledTime * animationSpeed) * 0.1f;
            transform.localScale = originalScale * pulse * hoverScale;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.unscaledDeltaTime * animationSpeed);
        }
    }

    void AnimateShake()
    {
        if (isHovering)
        {
            float shakeAmount = 5f;
            float shake = Mathf.Sin(Time.unscaledTime * animationSpeed * 5) * shakeAmount;
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, shake);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, Time.unscaledDeltaTime * animationSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.unscaledDeltaTime * animationSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.unscaledDeltaTime * animationSpeed);
        }
    }

    void AnimateRotateContinuous()
    {
        if (isHovering)
        {
            transform.Rotate(0, 0, animationSpeed * 100 * Time.unscaledDeltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, Time.unscaledDeltaTime * animationSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.unscaledDeltaTime * animationSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.unscaledDeltaTime * animationSpeed);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ActivateHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        DeactivateHoverEffect();
    }
}