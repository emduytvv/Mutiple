using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : SaiMonoBehaviour
{
    [SerializeField] private GameObject _slotEmpty;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _amount;
    public TextMeshProUGUI Amount => _amount;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSlotEmpty();
        this.LoadIcon();
        this.LoadAmount();
    }

    private void LoadSlotEmpty()
    {
        if (this._slotEmpty != null) return;
        this._slotEmpty = transform.Find("Lock").gameObject;
        Debug.Log(transform.name + ": Load SlotEmpty", gameObject);
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

    public void SetItem(ItemBase item)
    {
        bool hasItem = item != null;
        _icon.sprite = hasItem ? item._info._icon : null;
        _icon.enabled = hasItem;
        _amount.text = hasItem ? item._amount.ToString() : "";
        _slotEmpty.SetActive(!hasItem);
    }
}