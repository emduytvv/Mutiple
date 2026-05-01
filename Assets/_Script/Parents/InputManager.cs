using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    protected static InputManager instance;
    public static InputManager Instance => instance;
    protected Vector2 mousePosition;
    public Vector2 MousePosition => mousePosition;
    protected bool rightMouseDown;
    public bool RightMouseDown => rightMouseDown;
    protected bool rightMouse;
    public bool RightMouse => rightMouse;
    protected bool rightMouseUp;
    public bool RightMouseUp => rightMouseUp;

    protected void Start()
    {
        InputManager.instance = this;
    }

    void Update()
    {
        GetMousePosition();
        GetMouseButtons();
    }

    private void GetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void GetMouseButtons()
    {
        rightMouseDown = Input.GetMouseButtonDown(1);
        rightMouse     = Input.GetMouseButton(1);
        rightMouseUp   = Input.GetMouseButtonUp(1);
    }
}
