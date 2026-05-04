using UnityEngine;
using UnityEngine.UI;

public class BtnCloseParent : BaseBtn
{
    protected override void OnClick()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
