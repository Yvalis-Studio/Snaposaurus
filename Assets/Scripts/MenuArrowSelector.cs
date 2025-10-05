using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuArrowSelector : MonoBehaviour
{
    public RectTransform arrow;
    public float margin = 75f; // Distance entre flèche et texte
    public bool alignLeft = true; // true = gauche du texte, false = droite
    public float moveSpeed = 5f;
    public float panelChangeDelay = 0.5f; // Délai avant que la flèche devienne active

    private RectTransform currentTarget;
    private CanvasGroup arrowCanvas;
    private bool isActive = true;
    private float delayTimer = 0f;
    
    void Start()
    {
        if (arrow == null)
        {
            Debug.LogError("Arrow not assigned in MenuArrowSelector!");
            return;
        }

        arrowCanvas = arrow.GetComponent<CanvasGroup>();
        if (arrowCanvas == null)
            arrowCanvas = arrow.gameObject.AddComponent<CanvasGroup>();

        arrowCanvas.alpha = 0;
    }
    
    void Update()
    {
        if (arrowCanvas == null) return;

        // Gère le délai d'activation
        if (!isActive)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f)
            {
                isActive = true;
            }
            else
            {
                arrowCanvas.alpha = 0f; // Force à 0 pendant le délai
                return;
            }
        }

        if (currentTarget != null && isActive)
        {
            // Trouve le TextMeshPro dans le bouton
            TextMeshProUGUI buttonText = currentTarget.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                RectTransform textRect = buttonText.GetComponent<RectTransform>();

                // Position du texte en world space
                Vector3 textWorldPos = textRect.position;

                // Convertit en position locale du panel
                Vector2 textLocalPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform as RectTransform,
                    RectTransformUtility.WorldToScreenPoint(null, textWorldPos),
                    null,
                    out textLocalPos
                );

                // Calcule offset selon le côté du texte
                float textWidth = textRect.rect.width * textRect.lossyScale.x / arrow.lossyScale.x;
                Vector2 offset = alignLeft
                    ? new Vector2(-(textWidth / 2 + margin), 0)
                    : new Vector2(textWidth / 2 + margin, 0);

                Vector2 targetPos = textLocalPos + offset;
                arrow.anchoredPosition = Vector2.Lerp(arrow.anchoredPosition, targetPos, Time.deltaTime * moveSpeed);
            }
            else
            {
                // Fallback si pas de texte trouvé
                Vector2 targetPos = currentTarget.anchoredPosition + new Vector2(-margin, 0);
                arrow.anchoredPosition = Vector2.Lerp(arrow.anchoredPosition, targetPos, Time.deltaTime * moveSpeed);
            }

            arrowCanvas.alpha = Mathf.Lerp(arrowCanvas.alpha, 1f, Time.deltaTime * moveSpeed);
        }
        else
        {
            arrowCanvas.alpha = Mathf.Lerp(arrowCanvas.alpha, 0f, Time.deltaTime * moveSpeed);
        }
    }
    
    public void SetTarget(RectTransform target)
    {
        currentTarget = target;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }

    // Appelle cette méthode quand tu changes de panel
    public void DisableForPanelChange()
    {
        isActive = false;
        delayTimer = panelChangeDelay;
        currentTarget = null;
        if (arrowCanvas != null)
            arrowCanvas.alpha = 0; // Disparaît instantanément
    }
}