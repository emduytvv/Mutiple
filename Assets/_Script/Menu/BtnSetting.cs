using Firebase.Auth;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnSetting : BaseBtn
{
    protected override void OnClick()
    {
        base.OnClick();
        CenterCtrl.Instance.PanelSetting.gameObject.SetActive(true);
    }
}
