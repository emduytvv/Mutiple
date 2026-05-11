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

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadIcon();
        this.LoadAmount();
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
        if (eventData.button != PointerEventData.InputButton.Right) return;
        if (_currentItem?._info == null) return;
        if (_currentItem._info._typeItem != TypeItem.Equipment) return;
        UIItemContextMenu.Instance.Show(eventData.position, SlotIndex);
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