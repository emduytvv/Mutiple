using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIEquipSlot : SaiMonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipType _slotType;
    public EquipType SlotType => _slotType;
    [SerializeField] private Image _icon;
    [SerializeField] private UIItemDetailInventory _uIItemDetailInventory;
    protected ItemInventoryBase _currentItem = null;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadIcon();
        LoadUIItemDetailManager();
    }

    private void LoadUIItemDetailManager()
    {
        if (_uIItemDetailInventory != null) return;
        _uIItemDetailInventory = transform.parent.parent.parent.GetComponentInChildren<UIItemDetailInventory>();
    }
    private void LoadIcon()
    {
        if (_icon != null) return;
        _icon = transform.Find("Icon").GetComponent<Image>();
    }
    public void SetItem(ItemInventoryBase item)
    {
        _currentItem = item;
        bool hasItem = item != null && item._info != null;
        _icon.sprite = hasItem ? item._info._icon : null;
        _icon.enabled = hasItem;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        OnMouseLeftClick(eventData);
    }
    private void OnMouseLeftClick(PointerEventData eventData)
    {
        if (_currentItem == null) return;
        _uIItemDetailInventory.gameObject.SetActive(true);
        _uIItemDetailInventory.Show(_currentItem);
    }


}
