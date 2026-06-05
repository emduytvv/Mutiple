using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetailShop : UIItemDetailBase
{
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _sold;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPrice();
        LoadSold();
    }
    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
    private void LoadPrice()
    {
        if (_price != null) return;
        _price = transform.Find("Buy").Find("Price").GetComponent<TextMeshProUGUI>();

    }
    private void LoadSold()
    {
        if (_sold != null) return;
        _sold = transform.Find("Buy").Find("Sold").GetComponent<TextMeshProUGUI>();
        _sold.gameObject.SetActive(false);
    }
    public void Show(ItemInventoryBase item, UIShopSlot slot)
    {
        if (item == null || item._info == null) return;
        SetAvatar(item);
        SetNameItem(item);

        if (item._info is WeaponDataSO weapon)
            ShowWeapon(weapon, item._currentLevel);
        else if (item._info is EquipmentDataSO equipment)
            ShowEquipment(equipment);
        else if (item._info is PowerUpDataSO powerUp)
            ShowPowerUp(powerUp);

        SetBuyItem(item, slot);
    }
    public void SetBuyItem(ItemInventoryBase item, UIShopSlot slot)
    {
        _price.text = item._info._price.ToString();
        _sold.gameObject.SetActive(slot.IsSold);
        _price.gameObject.SetActive(!slot.IsSold);
    }
}
