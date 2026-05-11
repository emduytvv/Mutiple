using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIShopSlot : SaiMonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    public Image Icon => _icon;
    public int SlotIndex { get; set; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadIcon();
    }

    private void LoadIcon()
    {
        if (this._icon != null) return;
        this._icon = transform.Find("Icon").GetComponent<Image>();
        Debug.Log(transform.name + ": Load Icon", gameObject);
    }
    public void SetItem(ItemInventoryBase item)
    {
        bool hasItem = item != null && item._info != null;
        _icon.sprite = hasItem ? item._info._icon : null;
        _icon.enabled = hasItem;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
    }
}
