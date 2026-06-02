using UnityEngine;
using UnityEngine.UI;

public class CenterCtrl : Singleton<CenterCtrl>
{
    [SerializeField] protected Transform _uiInventory;
    [SerializeField] protected Transform _uiShop;
    [SerializeField] protected Transform _uiUpgrade;
    [SerializeField] private Transform _uiChestSkillPanel;
    [SerializeField] private Transform _panelSetting;
    public Transform PanelSetting => _panelSetting;
    [SerializeField] private Transform _panelGameOver;
    public Transform PanelGameOver => _panelGameOver;
    [SerializeField] private Transform _panelGameWin;
    public Transform PanelGameWin => _panelGameWin;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUIInventory();
        LoadUIShop();
        LoadUIChestSkillPanel();
        LoadUIUpgrade();
        LoadPanelSetting();
        LoadPanelGameOver();
        LoadPanelGameWin();
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

    private void LoadPanelSetting()
    {
        if (_panelSetting != null) return;
        _panelSetting = transform.Find("PanelSetting");
        _panelSetting.gameObject.SetActive(false);
        Debug.Log(transform.name + ": LoadPanelSetting", gameObject);
    }

    private void LoadPanelGameOver()
    {
        if (_panelGameOver != null) return;
        _panelGameOver = transform.Find("PanelGameOver");
        _panelGameOver.gameObject.SetActive(false);
        Debug.Log(transform.name + ": LoadPanelGameOver", gameObject);
    }

    private void LoadPanelGameWin()
    {
        if (_panelGameWin != null) return;
        _panelGameWin = transform.Find("PanelGameWin");
        _panelGameWin.gameObject.SetActive(false);
        Debug.Log(transform.name + ": LoadPanelGameWin", gameObject);
    }
}
