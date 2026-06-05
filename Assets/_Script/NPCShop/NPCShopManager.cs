using UnityEngine;

public class NPCShopManager : Singleton<NPCShopManager>
{
    public NPCShopData NPCShopData => _npcShopData;
    [SerializeField] protected NPCShopData _npcShopData;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNPCShopData();
    }

    private void LoadNPCShopData()
    {
        if (_npcShopData != null) return;
        _npcShopData = GetComponentInChildren<NPCShopData>();
    }
}
