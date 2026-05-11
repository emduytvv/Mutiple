# Project

2D co-op action platformer, 1–2 người chơi qua mạng.

**Stack:** Unity 6 · Photon PUN 2 · New Input System · URP 17.3 · TextMeshPro  
**Mục tiêu:** Portfolio project — 4 tháng kinh nghiệm Unity  
**Scope:** Tutorial map + Map 1 (playable) · Map 2+ locked · 10–15 phút/map

**GitHub:** https://github.com/emduytvv/Mutiple

## Phong cách trả lời

Bạn là 1 expert Unity 8 năm kinh nghiệm

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
````

## GetComponent Rules

| Vị trí            | Method                                           |
| ----------------- | ------------------------------------------------ |
| Chính object      | `GetComponent<T>()`                              |
| Cha               | `GetComponentInParent<T>()`                      |
| Con               | `GetComponentInChildren<T>()`                    |
| Sibling           | `transform.parent.GetComponentInChildren<T>()`   |
| Runtime collision | `TryGetComponent<T>()` (không allocate khi miss) |

**Ngoại lệ được load riêng** (không nằm trên Ctrl): component của chính object đó, LayerMask, `transform.Find()`, sibling type-specific.

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
6. Inventory + Shop + Level
7. Thiết kế Map 1
8. UI + Polish
9. Tutorial

Gặp ký hiệu 🌐 → test 2 máy trước khi tiếp tục.

# Những điều cần lưu ý (Gotchas)

- **RPC phải trên cùng GameObject với PhotonView** — Photon không lookup xuống children. Pattern: Ctrl nhận RPC → gọi xuống child.
- **Chỉ MasterClient spawn/destroy enemy** — đặt `if (!PhotonNetwork.IsMasterClient) return` trước mọi `PhotonNetwork.Instantiate` trong enemy system.
- **`_hasHit` flag trong projectile** — Arrow/Bullet dùng flag này để tránh double-hit khi collider overlap.
- **`DespawnByTime` reset timer trong `OnEnable()`** — lý do pool có thể reuse object mà không despawn ngay.
- **`EnemyDespawn.CanDespawn()` luôn trả về false** — despawn được trigger qua `EnemyAnimation.DespawnByEvent()`, không phải timer.
- **`PlayerCtrl._allPlayers` là static list** — dùng list này để tìm target gần nhất (BatCreator, EnemyMovementToTarget).
- **`PhotonPool` phải đăng ký tất cả spawners** — thêm loại projectile mới → phải đăng ký spawner vào PhotonPool.
- **`_Assets/Photon/` là readonly** — không sửa bất kỳ file nào trong đó.

# Scripts Index

**Quy tắc bắt buộc:** Trước khi dùng Glob hoặc Grep để tìm file → đọc `docs/architecture.md` trước.

- Script đã có trong đó → dùng `Read` với path trực tiếp, **không được Glob/Grep**
- Script chưa có → mới được phép dùng Glob/Grep
