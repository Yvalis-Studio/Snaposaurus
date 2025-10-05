using UnityEngine;
using UnityEngine.EventSystems;

public class BackButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation Settings")]
    [SerializeField] private AnimationType animType = AnimationType.RotateAndScale;
    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float hoverScale = 1.2f;
    
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private bool isHovering = false;
    
    public enum AnimationType
    {
        RotateAndScale,      // Rotation + agrandissement
        Pulse,               // Pulsation
        Shake,               // Tremblement
        RotateContinuous     // Rotation continue
    }
    
    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.localRotation;
    }
    
    void Update()
    {
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
        }
    }
    
    void AnimateRotateAndScale()
    {
        // Scale
        Vector3 targetScale = isHovering ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
        
        // Rotation
        float targetRotation = isHovering ? 90f : 0f;
        Quaternion target = originalRotation * Quaternion.Euler(0, 0, targetRotation);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.deltaTime * animationSpeed);
    }
    
    void AnimatePulse()
    {
        if (isHovering)
        {
            float pulse = 1f + Mathf.Sin(Time.time * animationSpeed) * 0.1f;
            transform.localScale = originalScale * pulse * hoverScale;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }
    
    void AnimateShake()
    {
        if (isHovering)
        {
            float shakeAmount = 5f;
            float shake = Mathf.Sin(Time.time * animationSpeed * 5) * shakeAmount;
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, shake);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, Time.deltaTime * animationSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * animationSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }
    
    void AnimateRotateContinuous()
    {
        if (isHovering)
        {
            transform.Rotate(0, 0, animationSpeed * 100 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, Time.deltaTime * animationSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * animationSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}