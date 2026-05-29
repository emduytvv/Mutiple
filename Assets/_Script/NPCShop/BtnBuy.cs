using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnBuy : BaseBtn
{
    protected override void OnClick()
    {
        base.OnClick();
        UIItemShopManager.Instance.TryBuyItem();
    }

}
