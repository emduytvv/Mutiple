using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : SaiMonoBehaviour
{
    [SerializeField] private List<UIInventorySlot> _slotItems = new List<UIInventorySlot>();
    [SerializeField] private int _totalSlot = 9;
    private InventoryManager _inventory;

    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponents()
    {
        if (_slotItems.Count > 0) return;
        foreach (Transform slot in transform)
        {
            _slotItems.Add(slot.GetComponent<UIInventorySlot>());
        }
    }
    protected void OnEnable()
    {
        Debug.Log(transform.name + ": ShowUI");
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

        var local = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        _inventory = local.GetComponentInChildren<InventoryManager>();
        Debug.Log(transform.name + ": InventoryManager: " + _inventory.Items.Count);
        for (int i = 0; i < _totalSlot; i++)
        {
            if (i < _inventory.Items.Count)
            {
                _slotItems[i].SetItem(_inventory?.Items[i]);
            }
            else
            {
                _slotItems[i].SetItem(null);
            }
        }
    }

}

