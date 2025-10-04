using UnityEngine;
using UnityEngine.InputSystem;
 
public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 0.3f;
    public float smoothTime = .3f;
 
    private Vector2 startPosition;
    private Vector3 velocity;
 
    private void Start()
    {
        startPosition = transform.position;
    }
 
    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + (offset * offsetMultiplier), ref velocity, smoothTime);
    }
}
 