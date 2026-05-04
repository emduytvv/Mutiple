using UnityEngine;
using UnityEngine.UI;

public class BtnOpenInventory : BaseBtn
{
    protected override void OnClick()
    {
        CenterCtrl.Instance.OpenInventory();
    }
}
