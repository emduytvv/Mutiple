using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected Vector2 mousePosition;
    public Vector2 MousePosition => mousePosition;
    protected bool rightMouseDown;
    public bool RightMouseDown => rightMouseDown;
    protected bool rightMouse;
    public bool RightMouse => rightMouse;
    protected bool rightMouseUp;
    public bool RightMouseUp => rightMouseUp;
    void Update()
    {
        GetMousePosition();
        GetMouseButtons();
        GetMouseReviveHelper();
    }

    private void GetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void GetMouseButtons()
    {
        rightMouseDown = Input.GetMouseButtonDown(1);
        rightMouse = Input.GetMouseButton(1);
        rightMouseUp = Input.GetMouseButtonUp(1);
    }
    public bool GetMouseReviveHelper()
    {
        return Input.GetKey(KeyCode.F);
    }
}
