using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIEquipSlot : SaiMonoBehaviour//, IPointerClickHandler
{
    [SerializeField] private EquipType _slotType;
    public EquipType SlotType => _slotType;
    [SerializeField] private Image _icon;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadIcon();
    }


    private void LoadIcon()
    {
        if (_icon != null) return;
        _icon = transform.Find("Icon").GetComponent<Image>();
        Debug.Log(transform.name + ": Load Icon", gameObject);
    }
    public void SetItem(ItemInventoryBase item)
    {
        bool hasItem = item != null && item._info != null;
        _icon.sprite = hasItem ? item._info._icon : null;
        _icon.enabled = hasItem;
    }

    // // Được Unity gọi khi người dùng click vào slot này
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     UICharacterPanel.Instance.TryUnequip(_slotType);
    // }
}
