using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerQTE : MonoBehaviour
{
    public InputAction QTEAction;

    Vector2 move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QTEAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        move = QTEAction.ReadValue<Vector2>();
        Debug.Log(move);
    }
}
