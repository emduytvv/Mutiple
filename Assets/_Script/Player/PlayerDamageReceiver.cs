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
    [Header("PowerUp Reduction (0=none 0.5=-50% 0.8=-80%)")]
    [SerializeField] private float _physicalDamageReduction = 0f;
    [SerializeField] private float _magicalDamageReduction = 0f;
    public AutoShield AutoShield => _autoShield;
    [SerializeField] protected AutoShield _autoShield;
    private PlayerCtrl _playerCtrl;

    protected override void OnEnable()
    {
        base.OnEnable();
        CalculateTotalStats();
        GameEvents.OnPlayerRevived += OnRevived;
        if (_playerCtrl != null && _playerCtrl.PhotonView.IsMine)
            GameEvents.OnEquipmentChanged += UpdateEquipmentStats;
    }
    private void OnDisable()
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
    }

    private void LoadAutoShield()
    {
        if (_autoShield != null) return;
        _autoShield = transform.parent.GetComponentInChildren<AutoShield>();
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        if (_playerCtrl == null || _playerCtrl.CharacterData == null) return;
        baseMaxHP = _playerCtrl.CharacterData.baseMaxHP;
        basePhysicalDefense = _playerCtrl.CharacterData.basePhysicalDefense;
        baseMagicalDefense = _playerCtrl.CharacterData.baseMagicalDefense;
    }
    private void OnRevived(int viewId)
    {
        if (_photonView.ViewID != viewId) return;
        Revive();
    }

    public void Revive()
    {
        _isDead = false;
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

    public void AddPhysicalReduction(float amount)
    {
        _physicalDamageReduction = Mathf.Clamp01(_physicalDamageReduction + amount);
    }

    public void AddMagicalReduction(float amount)
    {
        _magicalDamageReduction = Mathf.Clamp01(_magicalDamageReduction + amount);
    }

    public override void Receiver(float physDamage, float magDamage, float armorPen = 0f)
    {
        if (_autoShield.HasShield)
        {
            _autoShield.SetActiveShield(false);
            return;
        }
        physDamage *= (1f - _physicalDamageReduction);
        magDamage *= (1f - _magicalDamageReduction);
        base.Receiver(physDamage, magDamage, armorPen);
    }
    protected override void SetTotalMaxHP()
    {
        maxHP = (baseMaxHP + _hpEquipmentBonus) * (percentHPBonus + 1);
    }
    public float PhysicalDefenseTotal => physicalDefenseTotal;
    public float MagicalDefenseTotal => magicalDefenseTotal;
    public float GetCurrrentHPPercent() => currentHp / maxHP;
    //GoÌ£i Ä‘i change trang bÃ­ trong EquipmentManager
    private void UpdateEquipmentStats()
    {
        var armor = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Armor)?._info as EquipmentDataSO;
        var pants = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Pants)?._info as EquipmentDataSO;
        float physDef = (armor?._physicalDefense ?? 0) + (pants?._physicalDefense ?? 0);
        float magDef = (armor?._magicalDefense ?? 0) + (pants?._magicalDefense ?? 0);
        float hp = (armor?._hp ?? 0) + (pants?._hp ?? 0);
        ApplyEquipmentDefenseBonus(physDef, magDef, hp);
        _playerCtrl.PhotonView.RPC("RpcSyncDefenseStats", Photon.Pun.RpcTarget.Others, physDef, magDef, hp);
    }

    public void ApplyEquipmentDefenseBonus(float physDef, float magDef, float hp)
    {
        _equipmentPhysicalDefenseBonus = physDef;
        _equipmentMagicalDefenseBonus = magDef;
        _hpEquipmentBonus = hp;
        SetTotalMaxHP();
        CalculateTotalStats();
    }

    protected override void OnDead()
    {
        GameEvents.OnPlayerDied?.Invoke(_photonView.ViewID);
    }
}
