using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon.StructWrapping;

public class BtnUpgrade : BaseBtn
{
    private UIItemUpgradeManager _uiItemUpgradeManager;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _uiItemUpgradeManager = transform.parent.parent.GetComponent<UIItemUpgradeManager>();
    }

    protected override void OnClick()
    {
        _uiItemUpgradeManager.TryUpgradeWeapon();
    }

}
