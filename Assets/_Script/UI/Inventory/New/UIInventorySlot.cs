using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : SaiMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _amount;
    public Image Icon => _icon;
    public int SlotIndex { get; set; }
    private ItemInventoryBase _currentItem;
    [SerializeField] private UIItemDetailInventory _uIItemDetailInventory;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadIcon();
        this.LoadAmount();
        LoadUIItemDetailManager();
    }

    private void LoadIcon()
    {
        if (this._icon != null) return;
        this._icon = transform.Find("Icon").GetComponent<Image>();
        Debug.Log(transform.name + ": Load Icon", gameObject);
    }

    private void LoadAmount()
    {
        if (this._amount != null) return;
        this._amount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + ": Load Amount", gameObject);
    }
    private void LoadUIItemDetailManager()
    {
        if (_uIItemDetailInventory != null) return;
        _uIItemDetailInventory = transform.parent.parent.GetComponentInChildren<UIItemDetailInventory>();
        Debug.Log(transform.name + ": Load UIItemDetailManager", gameObject);
    }
    public void SetItem(ItemInventoryBase item)
    {
        _currentItem = item;
        bool hasItem = item != null && item._info != null;
        _icon.sprite = hasItem ? item._info._icon : null;
        _icon.enabled = hasItem;
        _amount.text = hasItem ? item._amount.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnMouseLeftClick(eventData);
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnMouseRightClick(eventData);
            return;
        }
    }

    private void OnMouseRightClick(PointerEventData eventData)
    {
        if (_currentItem == null || _currentItem._info == null) return;
        UIItemContextMenu.Instance.Show(eventData.position, SlotIndex, _currentItem._info._typeItem);
    }
    private void OnMouseLeftClick(PointerEventData eventData)
    {
        if (_currentItem == null || _currentItem._info == null) return;
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        _uIItemDetailInventory.gameObject.SetActive(true);
        _uIItemDetailInventory.Show(_currentItem);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_icon.sprite == null) return;
        DragController.Instance.BeginDrag(this);
    }
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_icon.sprite == null) return;
        DragController.Instance.EndDrag(eventData);
    }
}