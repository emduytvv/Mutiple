using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetailInventory : UIItemDetailBase
{
    public void Show(ItemInventoryBase item)
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
    }
    private void Update()
    {
        HanleClick();
    }
    private void HanleClick()
    {
        if (!InputManager.Instance.LeftMouseDown) return;
        if (IsPointerOverSelf()) return;
        Hide();
    }
    private bool IsPointerOverSelf()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, Camera.main);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
