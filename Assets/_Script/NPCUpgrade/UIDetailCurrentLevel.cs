using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDetailCurrentLevel : UIItemDetailBase
{
    public void Show(ItemInventoryBase item)
    {
        if (item == null || item._info == null) return;
        SetAvatar(item);
        SetNameItem(item);

        if (item._info is WeaponDataSO weapon)
            ShowWeapon(weapon, item._currentLevel);
    }

}
