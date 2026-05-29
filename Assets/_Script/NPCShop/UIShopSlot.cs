using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class UIShopSlot : SaiMonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UIItemDetailShop _uIItemDetailShop;
    [SerializeField] private UIItemShopManager _uIShopManager;
    [SerializeField] private Image _icon;
    public Image Icon => _icon;
    [SerializeField] private Image _lockIcon;
    public int SlotIndex { get; set; }
    private ItemInventoryBase _currentItem;
    [SerializeField] private bool _isSold = false;
    public bool IsSold => _isSold;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadIcon();
        this.LoadLockIcon();
        LoadUIItemDetailManager();
        LoadUIShopManager();
    }

    private void LoadLockIcon()
    {
        if (_lockIcon != null) return;
        _lockIcon = transform.Find("LockIcon").GetComponent<Image>();
        _lockIcon.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load LockIcon", gameObject);
    }
    private void LoadUIItemDetailManager()
    {
        if (_uIItemDetailShop != null) return;
        _uIItemDetailShop = transform.parent.parent.GetComponentInChildren<UIItemDetailShop>();
        Debug.Log(transform.name + ": Load UIItemDetailManager", gameObject);
    }
    private void LoadUIShopManager()
    {
        if (_uIShopManager != null) return;
        _uIShopManager = transform.parent.GetComponent<UIItemShopManager>();
        Debug.Log(transform.name + ": LoadUIShopManager", gameObject);
    }


    private void LoadIcon()
    {
        if (this._icon != null) return;
        this._icon = transform.Find("Icon").GetComponent<Image>();
        Debug.Log(transform.name + ": Load Icon", gameObject);
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
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        _uIShopManager.SetCurrentSlot(this);
        _uIItemDetailShop.gameObject.SetActive(true);
        _uIItemDetailShop.Show(_currentItem, this);
    }
    public void SetSold()
    {
        _isSold = true;
        _lockIcon.gameObject.SetActive(true);
        _uIItemDetailShop.SetBuyItem(_currentItem, this);
    }

}
