using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverNotifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MenuArrowSelector arrowSelector;
    private RectTransform rectTransform;
    
    void Start()
    {
        // Trouve la flèche dans le parent (le Panel)
        arrowSelector = GetComponentInParent<MenuArrowSelector>();
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (arrowSelector != null)
            arrowSelector.SetTarget(rectTransform);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (arrowSelector != null)
            arrowSelector.ClearTarget();
    }
}