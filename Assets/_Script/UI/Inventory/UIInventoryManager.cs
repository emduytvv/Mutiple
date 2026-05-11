using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : SaiMonoBehaviour
{
    [SerializeField] private List<UIInventorySlot> _slotItems = new List<UIInventorySlot>();
    [SerializeField] private int _totalSlot = 9;
    private InventoryManager _inventory;
    public InventoryManager Inventory => _inventory;

    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponents()
    {
        if (_slotItems.Count > 0) return;
        foreach (Transform slot in transform)
        {
            UIInventorySlot ui = slot.GetComponent<UIInventorySlot>();
            if (ui == null) continue;
            _slotItems.Add(ui);
        }
    }
    protected void OnEnable()
    {
        ShowUI();

    }

    private void Update()
    {
        CheckOpen();
    }

    private void CheckOpen()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) Open();
    }


    private void Open()
    {
        ShowUI();
    }

    private void ShowUI()
    {
        if (_inventory == null)
        {
            var local = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
            _inventory = local.GetComponentInChildren<InventoryManager>();
        }

        for (int i = 0; i < _slotItems.Count; i++)
        {
            _slotItems[i].SlotIndex = i;
        }

        Refresh();
    }
    public void Refresh()
    {
        for (int i = 0; i < _totalSlot; i++)
        {
            ItemInventoryBase item = i < _inventory.Items.Count ? _inventory.Items[i] : null;
            _slotItems[i].SetItem(item);
        }
    }

}

