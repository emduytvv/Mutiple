using UnityEngine;
using UnityEngine.UI;

public class CenterCtrl : Singleton<CenterCtrl>
{
    public Transform UIInventory => _uiInventory;
    [SerializeField] protected Transform _uiInventory;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadUIInventory();
    }

    private void LoadUIInventory()
    {
        if (this._uiInventory != null) return;
        this._uiInventory = transform.Find("UIInventory");
        _uiInventory.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load UIInventory", gameObject);
    }
    public void OpenInventory()
    {
        _uiInventory.gameObject.SetActive(true);
    }
}
