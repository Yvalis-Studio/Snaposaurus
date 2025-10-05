using System.Collections;
using UnityEngine;

public class PanelTransition : MonoBehaviour
{
    public float duration = 0.4f;
    public Vector2 slideOffset = new Vector2(0, -100);

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private MenuArrowSelector arrowSelector;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalPosition = rectTransform.anchoredPosition;

        // Trouve automatiquement le MenuArrowSelector dans la scène
        arrowSelector = FindFirstObjectByType<MenuArrowSelector>();
    }

    void OnEnable()
    {
        if (arrowSelector != null)
            arrowSelector.DisableForPanelChange();

        StartCoroutine(TransitionIn());
    }

    void OnDisable()
    {
        if (arrowSelector != null)
            arrowSelector.DisableForPanelChange();
    }
    
    IEnumerator TransitionIn()
    {
        canvasGroup.alpha = 0;
        rectTransform.anchoredPosition = originalPosition + slideOffset;
        
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = 1 - Mathf.Pow(1 - t, 2); // Ease out
            
            canvasGroup.alpha = t;
            rectTransform.anchoredPosition = Vector2.Lerp(originalPosition + slideOffset, originalPosition, t);
            yield return null;
        }
        
        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = originalPosition;
    }
}