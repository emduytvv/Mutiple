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
    }



    private void LoadRaycaster()
    {
        if (_raycaster != null) return;
        _raycaster = GetComponentInParent<GraphicRaycaster>();
    }

    private void LoadInventoryUI()
    {
        if (_inventoryUI != null) return;
        _inventoryUI = GetComponentInParent<UIInventoryManager>();
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

        if (GetTransferTarget(eventData) != null)
        {
            TransferToTeammate(_saveSlot.SlotIndex);
            _saveSlot = null;
            return;
        }

        UIInventorySlot targetSlot = GetSlotToSwap(eventData);
        SwapSlot(targetSlot);
        _saveSlot = null;
    }

    private UITransferTarget GetTransferTarget(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        _raycaster.Raycast(eventData, results);
        foreach (var r in results)
        {
            var t = r.gameObject.GetComponent<UITransferTarget>();
            if (t != null) return t;
        }
        return null;
    }

    private void TransferToTeammate(int slotIndex)
    {
        PlayerCtrl local = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        PlayerCtrl teammate = PlayerCtrl.AllPlayers.Find(p => !p.PhotonView.IsMine);
        if (teammate == null) return;

        ItemInventoryBase item = local.InventoryManager.Items[slotIndex];
        if (item == null || item._info == null) return;

        string json = ItemTransferData.Serialize(item);
        local.InventoryManager.Remove(slotIndex);
        _inventoryUI.Refresh();

        teammate.PhotonView.RPC("RpcReceiveItem", teammate.PhotonView.Owner, json);
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
