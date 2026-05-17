using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnBuy : BaseBtn
{
    protected override void OnClick()
    {
        UIItemShopManager.Instance.TryBuyItem();
    }

}
