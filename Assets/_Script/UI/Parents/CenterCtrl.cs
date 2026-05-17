using UnityEngine;
using UnityEngine.UI;

public class CenterCtrl : Singleton<CenterCtrl>
{
    [SerializeField] protected Transform _uiInventory;
    [SerializeField] protected Transform _uiShop;
    [SerializeField] protected Transform _uiUpgrade;
    [SerializeField] private Transform _uiChestSkillPanel;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUIInventory();
        LoadUIShop();
        LoadUIChestSkillPanel();
        LoadUIUpgrade();
    }

    private void LoadUIInventory()
    {
        if (this._uiInventory != null) return;
        this._uiInventory = transform.Find("UIInventory");
        _uiInventory.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load UIInventory", gameObject);
    }
    private void LoadUIShop()
    {
        if (this._uiShop != null) return;
        this._uiShop = transform.Find("UIShopManager");
        _uiShop.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load UIInventory", gameObject);
    }
    private void LoadUIUpgrade()
    {
        if (this._uiUpgrade != null) return;
        this._uiUpgrade = transform.Find("UIUpgradeManager");
        _uiUpgrade.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load UIInventory", gameObject);
    }
    private void LoadUIChestSkillPanel()
    {
        if (_uiChestSkillPanel != null) return;
        _uiChestSkillPanel = transform.Find("UIChestSkillPanel");
        _uiChestSkillPanel.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load UIChestSkillPanel", gameObject);
    }
    public void OpenInventory()
    {
        _uiInventory.gameObject.SetActive(true);
    }
    public void OpenShop()
    {
        _uiShop.gameObject.SetActive(true);
    }
    public void OpenUpgrade()
    {
        _uiUpgrade.gameObject.SetActive(true);
    }
    public void OpenChestSkillPanel(ChestCtrl chest, PlayerCtrl player)
    {
        _uiChestSkillPanel.GetComponentInChildren<UIChestSkillPanel>().Show(chest.ChestData.OfferedSkills, player);
    }
}
