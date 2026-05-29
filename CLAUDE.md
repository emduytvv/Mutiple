# Project

2D co-op action platformer, 1–2 người chơi qua mạng.

**Stack:** Unity 6 · Photon PUN 2 · New Input System · URP 17.3 · TextMeshPro  
**Mục tiêu:** Portfolio project — 4 tháng kinh nghiệm Unity  
**Scope:** Tutorial map + Map 1 (playable) · Map 2+ locked · 10–15 phút/map

**GitHub:** https://github.com/emduytvv/Mutiple

## Phong cách trả lời

Bạn là 1 expert Unity 8 năm kinh nghiệm

Khi user đề xuất một cấu trúc code/architecture, hãy:
- So sánh với pattern phổ biến trong Unity/game dev
- Nếu có cách tốt hơn hoặc thông dụng hơn → nói thẳng và giải thích lý do
- Không chỉ xác nhận "đúng rồi" nếu có vấn đề hoặc tradeoff đáng nói
- Đưa ra quan điểm cụ thể: "cách bạn làm được, nhưng thông thường người ta làm X vì..."

## Core Loop

````
Vào map → Khám phá tự do → Bước vào TriggerZone → Wave kẻ thù
→ Dọn sạch tất cả zone → Boss → Arena thu nhỏ → Kill Boss → WIN
Cả 2 chết → GAME OVER
``

Không có: save · checkpoint · minimap

# Scenes

- `_Scenes/SampleScene.unity` — lobby/menu (PhotonLogin, PhotonRoom, PhotonRoomAuto)
- `_Scenes/Duy.unity` — game scene (PhotonPlaying, WaveManager, TriggerZone, SpawnPointsManager)

# Code Style

- Tất cả scripts kế thừa `SaiMonoBehaviour` — override `LoadComponents()` và `ResetValue()`, không viết `Awake()` / `Start()` trực tiếp
- `LoadComponents()` là nơi duy nhất gọi `GetComponent` — không gọi trong `Update`/`FixedUpdate`
- `Update`/`FixedUpdate` chỉ chứa guard check + gọi 1 method — không viết logic trực tiếp trong đó
- RPC phải đặt trên cùng GameObject với `PhotonView` — Photon không tìm xuống children
- Chỉ `IsMasterClient` được spawn enemy, điều khiển boss, gửi damage từ enemy

# Architecture

## Ctrl Pattern (Hub)

Root Ctrl class (EnemyCtrl, PlayerCtrl) load và expose tất cả shared refs. Child components chỉ load `_ctrl` duy nhất.

```csharp
// EnemyCtrl expose:
public PhotonView PhotonView => _photonView;
public EnemyDamageReceiver DamageReceiver => _damageReceiver;
public EnemyAnimation EnemyAnimation => _enemyAnimation;
public Rigidbody2D Rigidbody2D => _rigidbody2D;
public EnemyDespawn EnemyDespawn => _enemyDespawn;

// Child chỉ load ctrl:
[SerializeField] protected EnemyCtrl _enemyCtrl;
private void LoadEnemyCtrl() { if (_enemyCtrl != null) return; _enemyCtrl = GetComponentInParent<EnemyCtrl>(); }
```

## Generic Ctrl Pattern

Base class dùng `<TCtrl>` để child class tự động có đúng kiểu ctrl, không cần field thứ 2 trong inspector.

```csharp
// Base class — khai báo generic
public abstract class EnemyMovement<TCtrl> : Movement where TCtrl : EnemyCtrl
{
    [SerializeField] protected TCtrl _enemyCtrl;
    private void LoadEnemyCtrl() { _enemyCtrl = GetComponentInParent<TCtrl>(); }
}

// Tầng giữa — truyền generic xuống
public abstract class EnemyMovementToTarget<TCtrl> : EnemyMovement<TCtrl> where TCtrl : EnemyCtrl { }

// Leaf class — đóng generic lại bằng type cụ thể
public class EnemyMeleeMovement : EnemyMovementToTarget<EnemyMeleeCtrl> { }
public class EnemyFlyMovement   : EnemyMovementToTarget<EnemyCtrl> { }
```

Áp dụng cho cả `EnemyCombat<TCtrl>`. Khi leaf class cần method của ctrl con (VD: `MeleeMovement`, `ShooterMovement`), ctrl con đó phải expose property ở Ctrl của nó.

**Khi nào dùng cast property thay vì generic:**
Nếu base class bị referenced BY ctrl (VD: `EnemyAnimation`, `EnemyDamageReceiver` lưu trong `EnemyCtrl`) thì không thể make generic — dùng cast property thay:
```csharp
// Áp dụng cho bất kỳ class nào kế thừa từ class được EnemyCtrl giữ ref
// EnemyMeleeAnimation : EnemyAnimation
private EnemyMeleeCtrl MeleeCtrl => _enemyCtrl as EnemyMeleeCtrl;

// SlimeDamageReceiver : EnemyDamageReceiver
private SlimeCtrl _slimeCtrl => _enemyCtrl as SlimeCtrl;
```

## Photon Sync

| Loại dữ liệu              | Cách sync                                      |
| ------------------------- | ---------------------------------------------- |
| Movement/position         | PhotonTransformView + PhotonAnimatorView       |
| Damage/HP                 | RPC (RpcTarget.All)                            |
| Downed/Revive             | RPC                                            |
| Enemy spawn/despawn       | Host spawn → PhotonNetwork.Instantiate/Destroy |
| Anim state                | RPC (SyncAnimState, RpcSetTrigger)             |
| Item pickup/trade/gold/XP | RPC                                            |
| Boss phase/HP             | RPC (Host authority)                           |

## PlayerAnimation States

```csharp
public enum PlayerState { Idle, Run, Jump, Drop, Land, Aim, Shoot, Dash, Die }
```

## Build Order

9 giai đoạn, không bỏ qua thứ tự:

1. Nền tảng + Photon cơ bản ✅
2. Nhân vật + Di chuyển ✅
3. Chiến đấu + Hồi sinh ✅
4. Enemy + Wave 🔄 ← Factory ✅ · Wave system ✅ — còn: downed/revive sync 🌐, wave balance
5. Boss
6. Inventory + Shop + Level 🔄 ← Data SO hierarchy ✅ · EquipmentManager ✅ · UICharacterPanel ✅ · NPCShopData ✅ — còn: NPCShopInteract, UIShopManager, shop logic, level system
7. Thiết kế Map 1
8. UI + Polish
9. Tutorial

Gặp ký hiệu 🌐 → test 2 máy trước khi tiếp tục.

# Những điều cần lưu ý (Gotchas)

- **RPC phải trên cùng GameObject với PhotonView** — Photon không lookup xuống children. Pattern: Ctrl nhận RPC → gọi xuống child.
- **Chỉ MasterClient spawn/destroy enemy** — đặt `if (!PhotonNetwork.IsMasterClient) return` trước mọi `PhotonNetwork.Instantiate` trong enemy system.
- **`PlayerCtrl._allPlayers` là static list** — dùng list này để tìm target gần nhất (BatCreator, EnemyMovementToTarget).
- **`PhotonPool` phải đăng ký tất cả spawners** — thêm loại projectile mới → phải đăng ký spawner vào PhotonPool.
- **`_Assets/Photon/` là readonly** — không sửa bất kỳ file nào trong đó.
````
