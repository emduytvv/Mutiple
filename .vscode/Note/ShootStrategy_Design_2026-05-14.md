# Shoot Strategy Pattern — Thiết kế (chưa implement)

> Ghi chú: chưa áp dụng RepeatDecorator. Design này là bản draft để review trước khi code.

---

## Mục tiêu

Tách "bắn bao nhiêu arrow" ra khỏi `PlayerShoot`.
**Tất cả loại bắn — kể cả bắn thường — đều là skill trong `WeaponDataSO._skills[]`.**
Khi bắt đầu game, nhân vật được trao weapon có sẵn skill NormalAttack (SingleShot).
Strategy swap theo `WeaponSkillName` của skill NormalAttack — không if/else.

---

## File & Folder Structure

```
Assets/_Script/
├── Inventory/Weapon/
│   ├── WeaponDataSO.cs          ← sửa: bỏ _normalAttackPattern
│   ├── WeaponSkillSO.cs         ← sửa: _name đổi từ string → WeaponSkillName
│   └── WeaponSkillName.cs       ← NEW enum
└── Player/
    ├── PlayerShoot.cs           ← sửa: _strategyMap dùng WeaponSkillName
    └── Shoot/                   ← NEW folder
        ├── IShootStrategy.cs
        ├── SingleShotStrategy.cs
        ├── SpreadShotStrategy.cs
        ├── DoubleShotStrategy.cs   ← 2 arrow song song, cùng góc, lệch vị trí
        └── DoubleTapStrategy.cs    ← bắn 1 arrow, delay, bắn thêm 1 arrow
```

> `ShootPatternType.cs` — **không tạo**, dùng `WeaponSkillName` thay thế.
> `ShootStrategyFactory.cs` — **không tạo**, dùng dictionary trong `PlayerShoot`.
> `ShootPatternConfig.cs` — **không tạo**, thông số của từng strategy nằm trong strategy đó luôn.

---

## Enum mới

### `WeaponSkillName.cs`

```csharp
public enum WeaponSkillName
{
    SingleShot  = 0,  // bắn 1 arrow — NormalAttack mặc định
    SpreadShot3 = 1,  // bắn 3 arrows tỏa góc
    SpreadShot5 = 2,  // bắn 5 arrows tỏa góc
    DoubleShot  = 3,  // bắn 2 arrows song song, cùng góc, lệch vị trí
    DoubleTap   = 4,  // bắn 1 arrow → delay 0.15s → bắn thêm 1 arrow
}
```

---

## Scripts mới (pure C#, không MonoBehaviour)

### `IShootStrategy.cs`

```csharp
public interface IShootStrategy
{
    void Shoot(string prefabName, Vector3 spawnCenter, float angle180);
}
```

### `SingleShotStrategy.cs`

```csharp
using Photon.Pun;
using UnityEngine;

public class SingleShotStrategy : IShootStrategy
{
    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180)
    {
        float rad = angle180 * Mathf.Deg2Rad;
        Vector3 pos = spawnCenter + new Vector3(Mathf.Cos(rad) * 0.5f, Mathf.Sin(rad) * 0.5f, 0);
        PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(0, 0, angle180));
    }
}
```

### `SpreadShotStrategy.cs`

```csharp
using Photon.Pun;
using UnityEngine;

public class SpreadShotStrategy : IShootStrategy
{
    private readonly int _count;
    private readonly float _spreadDeg;

    public SpreadShotStrategy(int count, float spreadDeg)
    {
        _count     = count;
        _spreadDeg = spreadDeg;
    }

    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180)
    {
        float startAngle = angle180 - _spreadDeg * (_count - 1) / 2f;
        for (int i = 0; i < _count; i++)
        {
            float angle = startAngle + _spreadDeg * i;
            float rad   = angle * Mathf.Deg2Rad;
            Vector3 pos = spawnCenter + new Vector3(Mathf.Cos(rad) * 0.5f, Mathf.Sin(rad) * 0.5f, 0);
            PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(0, 0, angle));
        }
    }
}
// Ví dụ SpreadShot3, spreadDeg=15, angle180=45°:
//   arrow[0] → 30°
//   arrow[1] → 45°  (thẳng)
//   arrow[2] → 60°
```

### `DoubleShotStrategy.cs`

```csharp
using Photon.Pun;
using UnityEngine;

public class DoubleShotStrategy : IShootStrategy
{
    private const float SideOffset = 0.2f;  // khoảng lệch vuông góc giữa 2 arrow

    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180)
    {
        float rad    = angle180 * Mathf.Deg2Rad;
        Vector3 dir  = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
        Vector3 perp = new Vector3(-dir.y, dir.x, 0);  // vuông góc với hướng bắn

        Vector3 pos1 = spawnCenter + dir * 0.5f + perp * SideOffset;
        Vector3 pos2 = spawnCenter + dir * 0.5f - perp * SideOffset;

        Quaternion rot = Quaternion.Euler(0, 0, angle180);
        PhotonNetwork.Instantiate(prefabName, pos1, rot);
        PhotonNetwork.Instantiate(prefabName, pos2, rot);
    }
}
// Visualize (nhìn từ trên, bắn sang phải →):
//   ● pos1 (trên)  →→→
//   ● pos2 (dưới)  →→→
```

### `DoubleTapStrategy.cs`

```csharp
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class DoubleTapStrategy : IShootStrategy
{
    private readonly MonoBehaviour _runner;  // PlayerShoot pass vào để StartCoroutine
    private readonly float _delay;

    public DoubleTapStrategy(MonoBehaviour runner, float delay = 0.15f)
    {
        _runner = runner;
        _delay  = delay;
    }

    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180)
    {
        _runner.StartCoroutine(ShootTwice(prefabName, spawnCenter, angle180));
    }

    private IEnumerator ShootTwice(string prefabName, Vector3 spawnCenter, float angle180)
    {
        SpawnArrow(prefabName, spawnCenter, angle180);
        yield return new WaitForSeconds(_delay);
        SpawnArrow(prefabName, spawnCenter, angle180);
    }

    private void SpawnArrow(string prefabName, Vector3 spawnCenter, float angle180)
    {
        float rad   = angle180 * Mathf.Deg2Rad;
        Vector3 pos = spawnCenter + new Vector3(Mathf.Cos(rad) * 0.5f, Mathf.Sin(rad) * 0.5f, 0);
        PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(0, 0, angle180));
    }
}
```

> **Lưu ý:** `DoubleTapStrategy` cần `MonoBehaviour` để chạy coroutine.
> `PlayerShoot` pass `this` vào lúc `BuildStrategyMap()`.

---

## Scripts sửa

### `WeaponSkillSO.cs` — đổi \_name từ string → WeaponSkillName

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponSkillSO")]
public class WeaponSkillSO : ScriptableObject
{
    public WeaponSkillName   _name;
    public string            _description;
    public WeaponSkillType   _type;
    public SkillWeaponRarity _rarity;
}
```

### `WeaponDataSO.cs` — bỏ \_normalAttackPattern

```csharp
[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "SO/WeaponDataSO")]
public class WeaponDataSO : EquippableDataSO
{
    public WeaponLevelData[] _levels;
    public WeaponSkillSO[]   _skills;
    // _normalAttackPattern đã bỏ — tất cả loại bắn nằm trong _skills[]
}
```

### `PlayerShoot.cs` — strategyMap, không còn \_patternConfigs

```csharp
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : SaiMonoBehaviour
{
    [SerializeField] protected PlayerCtrl _playerCtrl;
    [SerializeField] private string _arrowPrefabName = "Arrow_Raidon";
    [SerializeField] private float _aimAngle90;
    [SerializeField] protected float _aimAngle180;
    private bool _isAiming;
    protected Vector3 centerAim = Vector3.up * 0.85f;

    private Dictionary<WeaponSkillName, IShootStrategy> _strategyMap;
    private List<IShootStrategy> _strategies = new();

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
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    private void BuildStrategyMap()
    {
        _strategyMap = new Dictionary<WeaponSkillName, IShootStrategy>
        {
            [WeaponSkillName.SingleShot]  = new SingleShotStrategy(),
            [WeaponSkillName.SpreadShot3] = new SpreadShotStrategy(3, 15f),
            [WeaponSkillName.SpreadShot5] = new SpreadShotStrategy(5, 10f),
            [WeaponSkillName.DoubleShot]  = new DoubleShotStrategy(),
            [WeaponSkillName.DoubleTap]   = new DoubleTapStrategy(this, 0.15f),  // this = PlayerShoot
        };
        _strategies.Add(_strategyMap[WeaponSkillName.SingleShot]);  // default
    }

    private void OnEnable()  => GameEvents.OnEquipmentChanged += RefreshStrategy;
    private void OnDisable() => GameEvents.OnEquipmentChanged -= RefreshStrategy;

    private void RefreshStrategy()
    {
        _strategies.Clear();

        var item   = _playerCtrl.EquipmentManager.GetCurrentEquip(EquipType.Weapon);
        var weapon = item?._info as WeaponDataSO;

        var normalSkills = Array.FindAll(weapon?._skills ?? Array.Empty<WeaponSkillSO>(),
                                         s => s._type == WeaponSkillType.NormalAttack);

        foreach (var skill in normalSkills)
            if (_strategyMap.TryGetValue(skill._name, out var s))
                _strategies.Add(s);

        if (_strategies.Count == 0)
            _strategies.Add(_strategyMap[WeaponSkillName.SingleShot]);
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
        if (_isAiming) UpdateAimAngle();
        if (InputManager.Instance.RightMouseUp) OnShoot();
    }

    private void StartAim()
    {
        _isAiming = true;
        GameEvents.OnPlayerStartAim?.Invoke();
    }

    private void OnShoot()
    {
        _isAiming = false;
        Shoot();
        GameEvents.OnPlayerShoot?.Invoke();
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
        Vector3 center = transform.parent.position + centerAim;
        foreach (var s in _strategies)
            s.Shoot(_arrowPrefabName, center, _aimAngle180);
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
```

---

## SO Setup trong Unity

```
WeaponSkillSO assets cần tạo:
  SkillSO_SingleShot   → _name: SingleShot,  _type: NormalAttack, _rarity: Common
  SkillSO_SpreadShot3  → _name: SpreadShot3, _type: NormalAttack, _rarity: Rare
  SkillSO_SpreadShot5  → _name: SpreadShot5, _type: NormalAttack, _rarity: Epic
  SkillSO_DoubleShot   → _name: DoubleShot,  _type: NormalAttack, _rarity: Rare
  SkillSO_DoubleTap    → _name: DoubleTap,   _type: NormalAttack, _rarity: Common

WeaponDataSO mặc định (trao lúc bắt đầu game):
  _skills[0] = SkillSO_SingleShot   ← NormalAttack mặc định
```

---

## Hierarchy — không thêm GameObject nào

```
Player_Raidon (root)
├── [PlayerCtrl]
├── [EquipmentManager]      ← Equip() gọi GameEvents.OnEquipmentChanged
├── [PlayerShoot]
│     _arrowPrefabName = "Arrow_Raidon"
│     _strategyMap = { SingleShot→SingleShot(), SpreadShot3→SpreadShot(3,15), ... }
│     _strategies  = List — build từ tất cả NormalAttack skill của weapon
│                    Shoot() gọi foreach trên list này
└── ...
```

---

## Wiring Flow

```
Game start → player nhận WeaponDataSO mặc định (_skills: [SkillSO_SingleShot])
         ↓
GameEvents.OnEquipmentChanged → RefreshStrategy()
  → FindAll NormalAttack → [SingleShot]
  → _strategies = [SingleShotStrategy]

Player equip Bow_Legend (_skills: [SkillSO_DoubleShot, SkillSO_SpreadShot3, SkillSO_DashAttack])
         ↓
EquipmentManager.Equip() → GameEvents.OnEquipmentChanged → RefreshStrategy()
  → FindAll NormalAttack → [DoubleShot, SpreadShot3]   (DashAttack bị bỏ qua)
  → _strategies = [DoubleShotStrategy, SpreadShotStrategy(3,15)]

Player nhả chuột phải
  → foreach _strategies → s.Shoot("Arrow_Raidon", center, angle)
  → DoubleShotStrategy  : PhotonNetwork.Instantiate × 2 (song song)
  → SpreadShotStrategy  : PhotonNetwork.Instantiate × 3 (tỏa góc)
  → tổng 5 arrow / 1 lần bắn
```

---

## Câu hỏi cần xác nhận trước khi implement

1. **Damage per arrow trong Spread:** Mỗi arrow cùng damage, hay giảm (×0.6)?
2. **Prefab Velvet:** Có `Arrow_Velvet` chưa, hay tạm dùng chung `Arrow_Raidon`?
3. **Spread angle:** 15° giữa mỗi viên có hợp lý không?
4. **`WeaponSkillName` enum values** — hiện tại đề xuất: `SingleShot, SpreadShot3, SpreadShot5`. Cần confirm.

---

## Ghi chú tương lai

- **RepeatDecorator**: chưa áp dụng — wrap strategy để bắn nhiều lần tự động (rapid fire).
- Thêm loại bắn mới: thêm value vào `WeaponSkillName` + thêm entry vào `BuildStrategyMap()` + tạo `WeaponSkillSO` asset.
- `WeaponSkillType.DashAttack` dùng cho skill khác (không liên quan đến PlayerShoot).
