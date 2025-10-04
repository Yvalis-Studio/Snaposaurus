using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQTE : MonoBehaviour
{
    public InputAction up;
    public InputAction down;
    public InputAction left;
    public InputAction right;

    public QTEManager qteManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        up.Enable();
        down.Enable();
        left.Enable();
        right.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (up.WasPressedThisFrame())
        {
            Debug.Log("Up");
            qteManager.DoQTE("up");
        }
        if (down.WasPressedThisFrame())
        {
            Debug.Log("Down");
            qteManager.DoQTE("down");
        }
        if (left.WasPressedThisFrame())
        {
            Debug.Log("Left");
            qteManager.DoQTE("left");
        }
        if (right.WasPressedThisFrame())
        {
            Debug.Log("Right");
            qteManager.DoQTE("right");
        }
    }
}
