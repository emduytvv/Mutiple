using UnityEngine;
using UnityEngine.UI;

public class BtnOpenInventory : BaseBtn
{
    protected override void OnClick()
    {
        base.OnClick();
        CenterCtrl.Instance.OpenInventory();
    }
    protected void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;
        CenterCtrl.Instance.OpenInventory();

    }
}
