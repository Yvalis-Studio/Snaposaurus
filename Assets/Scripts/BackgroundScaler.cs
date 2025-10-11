using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool maintainAspect = true;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            ScaleToFitCamera();
    }

    void ScaleToFitCamera()
    {
        if (spriteRenderer.sprite == null) return;

        // Get camera dimensions
        float cameraHeight = targetCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        // Get sprite dimensions
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        // Calculate scale needed
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        if (maintainAspect)
        {
            // Use the larger scale to ensure coverage (may crop edges slightly)
            float scale = Mathf.Max(scaleX, scaleY);
            transform.localScale = new Vector3(scale, scale, 1f);
        }
        else
        {
            // Stretch to fit (may distort aspect ratio)
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }
    }

    // Call this if you change camera size or aspect ratio at runtime
    public void UpdateScale()
    {
        ScaleToFitCamera();
    }
}
