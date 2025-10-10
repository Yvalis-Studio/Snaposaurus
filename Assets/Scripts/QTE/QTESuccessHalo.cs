using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QTESuccessHalo : MonoBehaviour
{
    [Header("Halo Settings")]
    [SerializeField] private GameObject haloPrefab;
    [SerializeField] private float haloScale = 2f;
    [SerializeField] private float haloFadeDuration = 0.5f;
    [SerializeField] private Color haloColor = new Color(0f, 1f, 0f, 0.8f); // Green

    /// <summary>
    /// Spawn a halo effect at the target position
    /// </summary>
    public void SpawnHalo(Transform target)
    {
        Debug.Log("SpawnHalo called for: " + target.name);

        if (haloPrefab != null)
        {
            Debug.Log("Using prefab");
            GameObject halo = Instantiate(haloPrefab, target.position, Quaternion.identity, target.parent);
            StartCoroutine(AnimateHalo(halo));
        }
        else
        {
            Debug.Log("Creating simple halo - no prefab assigned");
            // Create simple halo with Image if no prefab assigned
            StartCoroutine(CreateAndAnimateSimpleHalo(target));
        }
    }

    IEnumerator CreateAndAnimateSimpleHalo(Transform target)
    {
        Debug.Log("Creating halo object...");

        // Create halo GameObject
        GameObject haloObj = new GameObject("SuccessHalo");

        // Add RectTransform for UI
        RectTransform rectTransform = haloObj.AddComponent<RectTransform>();
        haloObj.transform.SetParent(target.parent, false);

        // Position at target
        rectTransform.anchoredPosition = target.GetComponent<RectTransform>().anchoredPosition;
        rectTransform.localScale = Vector3.one * haloScale;

        // Set size same as target
        RectTransform targetRect = target.GetComponent<RectTransform>();
        if (targetRect != null)
        {
            rectTransform.sizeDelta = targetRect.sizeDelta * 1.2f;
            Debug.Log("Halo size: " + rectTransform.sizeDelta);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(100, 100);
        }

        // Add Image component
        Image haloImage = haloObj.AddComponent<Image>();
        haloImage.color = haloColor;
        haloImage.raycastTarget = false;

        Debug.Log("Halo color: " + haloColor);

        // Put halo behind the key
        haloObj.transform.SetSiblingIndex(target.GetSiblingIndex());

        // Animate
        yield return AnimateHalo(haloObj);
    }

    IEnumerator AnimateHalo(GameObject halo)
    {
        Image haloImage = halo.GetComponent<Image>();
        if (haloImage == null) yield break;

        Vector3 startScale = halo.transform.localScale;
        Vector3 endScale = startScale * 1.5f;
        Color startColor = haloImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsed = 0f;
        while (elapsed < haloFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / haloFadeDuration;

            // Scale up
            halo.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            // Fade out
            haloImage.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        // Destroy halo
        Destroy(halo);
    }

    /// <summary>
    /// Spawn halo effect for all visible key slots
    /// </summary>
    public void SpawnHaloForAll(Image[] keySlots)
    {
        foreach (var slot in keySlots)
        {
            if (slot != null && slot.gameObject.activeSelf)
            {
                SpawnHalo(slot.transform);
            }
        }
    }
}
