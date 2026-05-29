using UnityEngine;
using UnityEngine.UI;

public class BtnCloseParent : BaseBtn
{
    protected override void OnClick()
    {
        base.OnClick();
        transform.parent.gameObject.SetActive(false);
    }
}
