using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDetailNextLevel : UIItemDetailBase
{
    [SerializeField] private TextMeshProUGUI _price;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPrice();
    }
    private void LoadPrice()
    {
        if (_price != null) return;
        _price = transform.Find("Upgrade").Find("Price").GetComponent<TextMeshProUGUI>();
    }
    public void Show(ItemInventoryBase item)
    {
        if (item == null || item._info == null) return;
        SetAvatar(item);
        SetNameItem(item);

        if (item._info is WeaponDataSO weapon)
            ShowWeapon(weapon, item._currentLevel + 1);
        SetPrice(item);
    }
    public void SetPrice(ItemInventoryBase item)
    {
        _price.text = item._info._price.ToString();
        _price.gameObject.SetActive(true);
    }
    protected override void SetNameItem(ItemInventoryBase item)
    {
        _nameItem.text = item._info._name;
        SetColor(item);
        if (item._info is not WeaponDataSO) return;
        _nameItem.text += " +" + (item._currentLevel + 1);
    }
}
