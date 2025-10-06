using UnityEngine;
using UnityEngine.UI;

public class ScrollingTexture : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;

    private Image image;
    private Material material;

    void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            // Create a material instance
            material = new Material(image.material);
            image.material = material;
        }
    }

    void Update()
    {
        if (material != null)
        {
            // Scroll the texture
            Vector2 offset = material.mainTextureOffset;
            offset.x += scrollSpeed * Time.deltaTime;
            material.mainTextureOffset = offset;
        }
    }

    void OnDestroy()
    {
        // Clean up material instance
        if (material != null)
        {
            Destroy(material);
        }
    }
}
