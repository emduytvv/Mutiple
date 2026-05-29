using Photon.Pun;
using UnityEngine;

public class PlayerDamageSender : DamageSender
{
    [SerializeField] private PlayerCtrl _playerCtrl;

    [Header("Weapon Layer")]
    [SerializeField] private float _weaponPhysicalBonus = 0f;
    [SerializeField] private float _weaponMagicalBonus = 0f;
    [SerializeField] private float _weaponArmorPenetration = 0f;
    [SerializeField] private float _weaponCriticalRate = 0f;

    [Header("Other Bonus (PowerUp / Buff)")]
    [SerializeField] private float _physicalDamageBonus = 0f;
    [SerializeField] private float _magicalDamageBonus = 0f;
    [SerializeField] private float _percentPhysicalDamage = 0f;
    [SerializeField] private float _percentMagicalDamage = 0f;
    [Header("Base")]
    [SerializeField] private float _armorPenetration = 0f;
    [SerializeField] private float _criticalRate = 0f;

    [Header("Calculated")]
    [SerializeField] protected float _physicalDamageTotal = 0f;
    [SerializeField] protected float _magicalDamageTotal = 0f;
    [SerializeField] private float _armorPenetrationTotal = 0f;
    [SerializeField] private float _criticalRateTotal = 0f;
    public float PhysicalDamageTotal => _physicalDamageTotal;
    public float MagicalDamageTotal => _magicalDamageTotal;
    public float CritTotal => _criticalRateTotal;
    public float ArmorPenTotal => _armorPenetrationTotal;
    [SerializeField] private float _percentDamage = 0f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }
    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }
    protected void OnEnable()
    {
        CaculateTotalStats();
        if (_playerCtrl != null && _playerCtrl.PhotonView.IsMine)
        {
            GameEvents.OnEquipmentChanged += UpdateWeaponStats;
            GameEvents.OnWeaponUpgraded += UpdateWeaponStats;
        }
    }
    protected void OnDestroy()
    {
        GameEvents.OnEquipmentChanged -= UpdateWeaponStats;
        GameEvents.OnWeaponUpgraded -= UpdateWeaponStats;
    }
    protected void CaculateTotalStats()
    {
        float totalPhysBonus = _weaponPhysicalBonus + _physicalDamageBonus;
        float totalMagBonus = _weaponMagicalBonus + _magicalDamageBonus;
        _physicalDamageTotal = (basePhysicalDamage + totalPhysBonus) * (_percentDamage + _percentPhysicalDamage + 1f);
        _magicalDamageTotal = (baseMagicalDamage + totalMagBonus) * (_percentDamage + _percentMagicalDamage + 1f);
        _armorPenetrationTotal = _weaponArmorPenetration + _armorPenetration;
        _criticalRateTotal = _weaponCriticalRate + _criticalRate;
    }
    public void AddPhysicalDamage(float amount)
    {
        _physicalDamageBonus += amount;
        CaculateTotalStats();
    }
    public void AddMagicDamage(float amount)
    {
        _magicalDamageBonus += amount;
        CaculateTotalStats();
    }
    public void AddPercentDamage(float amount)
    {
        _percentDamage += amount;
        CaculateTotalStats();
    }
    public void AddPercentPhysicalDamage(float amount) { _percentPhysicalDamage += amount; CaculateTotalStats(); }
    public void AddPercentMagicalDamage(float amount) { _percentMagicalDamage += amount; CaculateTotalStats(); }
    public void AddCriticalRate(float amount) { _criticalRate += amount; CaculateTotalStats(); }
    public void AddArmorPenetration(float amount) { _armorPenetration += amount; CaculateTotalStats(); }
    // Gọi bởi PlayerShoot khi spawn Arrow
    public (float phys, float mag, float pen) BuildArrowDamage()
    {
        float critMult = Random.value < _criticalRateTotal ? 2f : 1f;
        return (_physicalDamageTotal * critMult, _magicalDamageTotal * critMult, _armorPenetrationTotal);
    }


    public void UpdateWeaponStats()
    {
        var weaponSlot = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Weapon);
        if (weaponSlot == null) return;
        var weapon = weaponSlot._info as WeaponDataSO;
        int level = weaponSlot._currentLevel;

        WeaponLevelData data = weapon._levels[level];
        _weaponPhysicalBonus = data._physicalDamageBonus;
        _weaponMagicalBonus = data._magicalDamageBonus;
        _weaponArmorPenetration = data._armorPenetration;
        _weaponCriticalRate = data._criticalRate;
        CaculateTotalStats();
    }
}
