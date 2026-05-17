using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    [Header("Weapon Layer")]
    [SerializeField] private float _equipmentPhysicalDefenseBonus = 0f;
    [SerializeField] private float _equipmentMagicalDefenseBonus = 0f;
    [SerializeField] public float basePhysicalDefense = 0f;
    [SerializeField] public float baseMagicalDefense = 0f;
    [SerializeField] private float _physicalDefenseBonus = 0f;
    [SerializeField] private float _magicalDefenseBonus = 0f;
    [SerializeField] private float _hpEquipmentBonus = 0f;
    public AutoShield AutoShield => _autoShield;
    [SerializeField] protected AutoShield _autoShield;
    private PlayerCtrl _playerCtrl;

    protected override void OnEnable()
    {
        base.OnEnable();
        CalculateTotalStats();
        GameEvents.OnPlayerRevived += OnRevived;
        GameEvents.OnEquipmentChanged += UpdateEquipmentStats;
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerRevived -= OnRevived;
        GameEvents.OnEquipmentChanged -= UpdateEquipmentStats;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCtrl();
        LoadAutoShield();
    }

    private void LoadCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    private void LoadAutoShield()
    {
        if (_autoShield != null) return;
        _autoShield = transform.parent.GetComponentInChildren<AutoShield>();
        Debug.Log(transform.name + ": Load AutoShield", gameObject);
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        baseMaxHP = 100f;
    }
    private void OnRevived(int viewId)
    {
        if (_photonView.ViewID != viewId) return;
        Revive();
    }

    public void Revive()
    {
        isDead = false;
        currentHp = maxHP * 0.3f;
    }
    private void CalculateTotalStats()
    {
        physicalDefenseTotal = basePhysicalDefense + _equipmentPhysicalDefenseBonus + _physicalDefenseBonus;
        magicalDefenseTotal = baseMagicalDefense + _equipmentMagicalDefenseBonus + _magicalDefenseBonus;
    }

    public void AddPhysicalDefense(float amount)
    {
        _physicalDefenseBonus += amount;
        CalculateTotalStats();
    }

    public void AddMagicalDefense(float amount)
    {
        _magicalDefenseBonus += amount;
        CalculateTotalStats();
    }
    public override void Receiver(float physDamage, float magDamage, float armorPen = 0f)
    {
        if (_autoShield.HasShield)
        {
            _autoShield.SetActiveShield(false);
            return;
        }
        base.Receiver(physDamage, magDamage, armorPen);

    }
    protected override void SetTotalMaxHP()
    {
        maxHP = (baseMaxHP + _hpEquipmentBonus) * (percentHPBonus + 1);
    }
    public float PhysicalDefenseTotal => physicalDefenseTotal;
    public float MagicalDefenseTotal => magicalDefenseTotal;
    public float GetCurrrentHPPercent() => currentHp / maxHP;
    //Gọi đi change trang bí trong EquipmentManager
    private void UpdateEquipmentStats()
    {
        var armor = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Armor)?._info as EquipmentDataSO;
        var pants = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Pants)?._info as EquipmentDataSO;
        _equipmentPhysicalDefenseBonus = (armor?._physicalDefense ?? 0) + (pants?._physicalDefense ?? 0);
        _equipmentMagicalDefenseBonus = (armor?._magicalDefense ?? 0) + (pants?._magicalDefense ?? 0);
        _hpEquipmentBonus = (armor?._hp ?? 0) + (pants?._hp ?? 0);
        SetTotalMaxHP();
        CalculateTotalStats();
    }

    protected override void OnDead()
    {
        GameEvents.OnPlayerDied?.Invoke(_photonView.ViewID);
    }
}
