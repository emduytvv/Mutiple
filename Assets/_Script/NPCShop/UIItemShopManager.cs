using System;
using System.Collections.Generic;
using UnityEngine;

public class UIItemShopManager : Singleton<UIItemShopManager>
{
    [SerializeField] private List<UIShopSlot> _slotItems = new List<UIShopSlot>();
    [SerializeField] private List<ItemInventoryBase> _slotDatas = new List<ItemInventoryBase>();
    [SerializeField] protected UIShopSlot _currentSlot;

    protected PlayerGold _playerGold;
    protected InventoryManager _inventoryManager;
    [SerializeField] private const int _maxSlot = 6;

    protected override void Start()
    {
        base.Start();
        LoadPlayer();
    }
    protected override void LoadComponents()
    {
        LoadSlot();
    }

    private void LoadPlayer()
    {
        PlayerCtrl playerCtrl = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        _inventoryManager = playerCtrl.GetComponentInChildren<InventoryManager>();
        _playerGold = playerCtrl.GetComponentInChildren<PlayerGold>();
    }


    public void SetCurrentSlot(UIShopSlot slot)
    {
        _currentSlot = slot;
    }
    private void LoadSlot()
    {
        if (_slotItems.Count > 0) return;
        foreach (Transform slot in transform)
        {
            UIShopSlot ui = slot.GetComponent<UIShopSlot>();
            if (ui == null) continue;
            _slotItems.Add(ui);
        }
    }
    protected void OnEnable()
    {
        LoadShopData();
        ShowUI();
    }

    private void LoadShopData()
    {
        _slotDatas = NPCShopManager.Instance.NPCShopData.Shop;
    }

    private void ShowUI()
    {
        for (int i = 0; i < _slotItems.Count; i++)
        {
            _slotItems[i].SlotIndex = i;
        }
        Refresh();
    }
    public void Refresh()
    {
        Debug.Log("Refresh");
        for (int i = 0; i < _maxSlot; i++)
        {
            ItemInventoryBase item = i < _slotDatas.Count ? _slotDatas[i] : null;
            _slotItems[i].SetItem(item);
        }
    }
    public void TryBuyItem()
    {
        if (_currentSlot.IsSold == true) return;
        int cost = _slotDatas[_currentSlot.SlotIndex]._info._price;
        if (!_playerGold.TrySpendGold(cost))
        {
            SpawnText("Not enough gold");
            return;
        }
        _currentSlot.SetSold();
        _inventoryManager.AddItem(_slotDatas[_currentSlot.SlotIndex]);
        SpawnText($"Buy success " + "-" + cost.ToString());
    }
    protected void SpawnText(string content)
    {
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Transform obj = TextSpawner.Instance.SpawnTextDefault(center);
        obj.GetComponent<TextDefaultCtrl>().SetText(content);
    }
}
