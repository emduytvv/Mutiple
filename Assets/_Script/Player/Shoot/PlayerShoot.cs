using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : SaiMonoBehaviour
{
    [SerializeField] protected PlayerCtrl _playerCtrl;
    [SerializeField] private float _aimAngle90;
    [SerializeField] protected float _aimAngle180;
    private bool _isAiming;
    //  private float _maxChargeTime = 0.4f;
    private float _minChargeTime = 0.2f;
    private float _chargeTimer = 0f;
    private bool _canShoot;
    protected Vector3 centerAim = Vector3.up * 0.85f;

    private Dictionary<WeaponSkillName, IShootStrategy> _strategyMap;
    private List<IShootStrategy> _iShoot = new();
    [SerializeField] private ArrowName _arrowPrefabName = ArrowName.ArrowNormal;

    private static readonly Dictionary<ArrowType, ArrowName> _arrowPrefabMap = new()
    {
        [ArrowType.Normal] = ArrowName.ArrowNormal,
        [ArrowType.Ricochet] = ArrowName.ArrowRicochet,
        [ArrowType.Piercing] = ArrowName.ArrowPiercing,
        [ArrowType.Explosive] = ArrowName.ArrowExplosive,
        [ArrowType.Gold] = ArrowName.ArrowGold,
    };

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
        this.BuildStrategyMap();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }

    private void BuildStrategyMap()
    {
        _strategyMap = new Dictionary<WeaponSkillName, IShootStrategy>
        {
            [WeaponSkillName.SingleShot] = new SingleShot(),
            [WeaponSkillName.SpreadThreeShot] = new SpreadThreeShot(),
            [WeaponSkillName.SpreadFiveShot] = new SpreadFiveShot(),
            [WeaponSkillName.DoubleShot] = new DoubleShot(this),
            [WeaponSkillName.DoubleArrow] = new DoubleArrow(),
            [WeaponSkillName.TripleArrow] = new TripleArrow(),
        };
        _iShoot.Add(_strategyMap[WeaponSkillName.SingleShot]);
    }

    private void OnEnable() => GameEvents.OnEquipmentChanged += RefreshStrategy;
    private void OnDisable() => GameEvents.OnEquipmentChanged -= RefreshStrategy;

    protected override void Start()
    {
        base.Start();
        if (_playerCtrl.PhotonView.IsMine) RefreshStrategy();
    }

    private void RefreshStrategy()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        _iShoot.Clear();

        var item = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Weapon);
        if (item == null) return;
        var weapon = item?._info as WeaponDataSO;
        foreach (var skill in weapon._skills)
            if (_strategyMap.TryGetValue(skill._name, out var s))
            { _iShoot.Add(s); }

        if (_iShoot.Count == 0)
            _iShoot.Add(_strategyMap[WeaponSkillName.SingleShot]);
        RefreshArrow(weapon);
    }

    private void RefreshArrow(WeaponDataSO weapon)
    {
        if (_arrowPrefabMap.TryGetValue(weapon._arrowType, out var name))
            _arrowPrefabName = name;
    }


    private void Update()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (InputManager.Instance == null) return;
        if (_playerCtrl.PlayerAnimation.CurrentState == PlayerState.Die)
        {
            _isAiming = false;
            return;
        }
        HandleAimInput();
    }

    private void HandleAimInput()
    {
        if (InputManager.Instance.RightMouseDown) StartAim();
        if (_isAiming)
        {
            UpdateAimAngle();
            CanShoot();
        }
        if (InputManager.Instance.RightMouseUp) OnShoot();

    }
    private void StartAim()
    {
        _isAiming = true;
        //   AudioManager.Instance.PlaySFX(AudioManager.Instance.Aim);
        GameEvents.OnPlayerStartAim?.Invoke();
    }
    private void CanShoot()
    {
        _chargeTimer += Time.deltaTime;
        _canShoot = _chargeTimer >= _minChargeTime;
    }
    private void OnShoot()
    {
        ResetAim();
        GameEvents.OnPlayerShoot?.Invoke();
        if (!_canShoot) return;
        Shoot();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ShootSFX);
    }

    private void ResetAim()
    {
        _chargeTimer = 0f;
        _isAiming = false;
    }


    private void UpdateAimAngle()
    {
        Vector2 dir = InputManager.Instance.MousePosition - (Vector2)(transform.parent.position + centerAim);
        _aimAngle90 = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;
        _aimAngle90 = Mathf.Clamp(_aimAngle90, 0f, 90f);
        GameEvents.OnPlayerAimAngleChanged?.Invoke(_aimAngle90);
    }

    private void Shoot()
    {
        UpdateAimAngle180();
        var (phys, mag, pen) = _playerCtrl.PlayerDamageSender.BuildArrowDamage();
        Vector3 center = transform.parent.position + centerAim;
        foreach (var s in _iShoot)
            s.Shoot(_arrowPrefabName.ToString(), center, _aimAngle180, phys, mag, pen);
    }

    private void UpdateAimAngle180()
    {
        bool facingRight = _playerCtrl.PlayerAnimation.transform.localScale.x > 0;
        _aimAngle180 = facingRight ? _aimAngle90 : 180f - _aimAngle90;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.parent.position + centerAim, 0.5f);
    }
}
