using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Popup nhỏ hiện khi chuột phải vào item trong inventory
// Singleton: toàn UI chỉ có 1 popup dùng chung cho mọi slot
public class UIItemContextMenu : Singleton<UIItemContextMenu>
{
    [SerializeField] private Button _btnUse;
    [SerializeField] private RectTransform _canvasRect;

    private int _InventoryIndex;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnUse();
        LoadCanvasRect();
    }
    private void LoadBtnUse()
    {
        if (_btnUse != null) return;
        _btnUse = transform.Find("BtnEquip").GetComponent<Button>();
        Debug.Log(transform.name + ": Load BtnUse", gameObject);
    }

    private void LoadCanvasRect()
    {
        if (_canvasRect != null) return;
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Debug.Log(transform.name + ": Load CanvasRect", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        _btnUse.onClick.AddListener(OnClickUse);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        HanleClick();
    }

    private void HanleClick()
    {
        if (!InputManager.Instance.LeftMouseDown) return;
        if (IsPointerOverSelf()) return;
        Hide();
    }


    private bool IsPointerOverSelf()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, Camera.main);
    }
    public void Show(Vector2 screenPos, int inventoryIndex)
    {
        _InventoryIndex = inventoryIndex;
        SetPosition(screenPos);
        gameObject.SetActive(true);
    }

    private void SetPosition(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, Camera.main, out Vector2 localPos);
        GetComponent<RectTransform>().anchoredPosition = localPos;
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickUse()
    {
        UICharacterPanel.Instance.TryEquip(_InventoryIndex);
        Hide();
    }
}
