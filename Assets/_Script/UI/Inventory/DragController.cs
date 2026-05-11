using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragController : Singleton<DragController>
{

    [SerializeField] private Image _cloneIcon;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private RectTransform _canvasRect;

    private UIInventorySlot _saveSlot;
    private UIInventoryManager _inventoryUI;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRaycaster();
        this.LoadInventoryUI();
        this.LoadCanvasRect();
    }

    private void LoadCanvasRect()
    {
        if (_canvasRect != null) return;
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Debug.Log(transform.name + ": Load CanvasRect", gameObject);
    }



    private void LoadRaycaster()
    {
        if (_raycaster != null) return;
        _raycaster = GetComponentInParent<GraphicRaycaster>();
        Debug.Log(transform.name + ": Load GraphicRaycaster", gameObject);
    }

    private void LoadInventoryUI()
    {
        if (_inventoryUI != null) return;
        _inventoryUI = GetComponentInParent<UIInventoryManager>();
        Debug.Log(transform.name + ": Load UIInventoryManager", gameObject);
    }

    public void BeginDrag(UIInventorySlot originSlot)
    {
        _saveSlot = originSlot;
        _cloneIcon.sprite = originSlot.Icon.sprite;
        _cloneIcon.gameObject.SetActive(true);
        originSlot.Icon.enabled = false;
    }

    private void Update()
    {
        if (!_cloneIcon.gameObject.activeSelf) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, Input.mousePosition, Camera.main, out Vector2 localPos);
        _cloneIcon.rectTransform.anchoredPosition = localPos;
    }

    public void EndDrag(PointerEventData eventData)
    {
        _cloneIcon.gameObject.SetActive(false);
        _saveSlot.Icon.enabled = true;

        UIInventorySlot targetSlot = GetSlotToSwap(eventData);
        SwapSlot(targetSlot);

        _saveSlot = null;
    }

    private void SwapSlot(UIInventorySlot targetSlot)
    {
        if (targetSlot != null && targetSlot != _saveSlot)
        {
            _inventoryUI.Inventory.Swap(_saveSlot.SlotIndex, targetSlot.SlotIndex);
            _inventoryUI.Refresh();
        }
    }
    private UIInventorySlot GetSlotToSwap(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        _raycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            var slot = result.gameObject.GetComponentInParent<UIInventorySlot>();
            if (slot != null) return slot;
        }
        return null;
    }
}
