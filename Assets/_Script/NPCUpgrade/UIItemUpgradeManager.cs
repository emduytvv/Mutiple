using System;
using System.Collections.Generic;
using UnityEngine;

public class UIItemUpgradeManager : SaiMonoBehaviour
{
    [SerializeField] private ItemInventoryBase _currentWeapon;
    protected PlayerGold _playerGold;
    protected EquipmentManager _equipmentManager;
    [SerializeField] protected UIDetailCurrentLevel _detailCurrentLevel;
    [SerializeField] protected UIDetailNextLevel _detailNextLevel;
    private const int MaxLevel = 10;

    protected override void Start()
    {
        base.Start();
        LoadPlayer();
        ShowUI();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadDetailCurrentLevel();
        LoadDetailNextLevel();
    }
    protected void OnEnable()
    {
        if (_equipmentManager == null || _playerGold == null) return;
        ShowUI();
    }
    private void LoadPlayer()
    {
        if (_equipmentManager != null && _playerGold != null) return;
        PlayerCtrl playerCtrl = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        _equipmentManager = playerCtrl.GetComponentInChildren<EquipmentManager>();
        _playerGold = playerCtrl.GetComponentInChildren<PlayerGold>();
    }

    private void LoadDetailCurrentLevel()
    {
        if (_detailCurrentLevel != null) return;
        _detailCurrentLevel = GetComponentInChildren<UIDetailCurrentLevel>();
    }

    private void LoadDetailNextLevel()
    {
        if (_detailNextLevel != null) return;
        _detailNextLevel = GetComponentInChildren<UIDetailNextLevel>();
    }

    public void ShowUI()
    {
        _currentWeapon = _equipmentManager.GetCurrentEquip(EquipType.Weapon);
        if (_currentWeapon == null || _currentWeapon._info == null) return;
        if (_currentWeapon._currentLevel >= MaxLevel)
        {
            SpawnText("Max level");
            return;
        }
        _detailCurrentLevel.Show(_currentWeapon);
        _detailNextLevel.Show(_currentWeapon);
    }

    public void TryUpgradeWeapon()
    {
        int cost = _currentWeapon._info._price;
        if (!_playerGold.TrySpendGold(cost))
        {
            SpawnText("Not enough gold");
            return;
        }
        _currentWeapon._currentLevel++;
        GameEvents.OnWeaponUpgraded?.Invoke();
        SpawnText("Upgrade success");
        ShowUI();
    }

    protected void SpawnText(string content)
    {
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Transform obj = TextSpawner.Instance.SpawnTextDefault(center);
        obj.GetComponent<TextDefaultCtrl>().SetText(content);
    }
}
