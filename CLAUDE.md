# CLAUDE.md — 2D Co-op Action Platformer

## Project Overview

**Game:** 2D co-op action platformer, 1–2 người chơi qua mạng
**Stack:** Unity 6 · Photon PUN 2 · New Input System · URP 17.3 · TextMeshPro
**Mục tiêu:** Portfolio project để đi thực tập — 4 tháng kinh nghiệm Unity
**Scope:** Tutorial map + Map 1 (playable) · Map 2+ chỉ hiển thị locked
**Thời gian mỗi map:** 10–15 phút

## Đường dẫn Projects

| Project | Đường dẫn | Unity Version |
| ------- | --------- | ------------- |
| **Multiple** (project hiện tại) | `C:\Users\Windows\Music\Học code\Unity Project\Multiple` | Unity 6 |
| **Surival** (project cũ) | `C:\Users\Windows\Music\Học code\Project 1\Surival` | 2022.3.62f3 |

## Core Loop

```
Vào map → Khám phá tự do → Bước vào zone → Wave kẻ thù
→ Dọn sạch tất cả zone → Boss omen → Arena thu nhỏ
→ Kill Boss → WIN | Cả 2 chết → GAME OVER
```

Không có: save system · checkpoint · minimap

## Folder Structure

```
Assets/
├── _Script/                    ← tất cả C# scripts custom
│   ├── SaiMonoBehaviour.cs     ← base class
│   ├── Menu/                   ← Photon lobby/room scripts
│   ├── Player/                 ← gameplay: movement, animation, input, events
│   └── Spawner/                ← object pool, enemy/player spawner
├── _Assets/
│   ├── Avatar/
│   ├── Editor/                 ← FixAnimations.cs, AudioMixerPostprocessor.cs
│   ├── IMPORTANT/
│   │   ├── AssetResources/
│   │   │   └── Character/AddressableResource/
│   │   │       ├── _Velvet/    ← Archer character (DPS)
│   │   │       ├── _Raidon/    ← character asset
│   │   │       ├── Bathos/ Cala/ Lucy/ Mary/ Morrod/ Mortal/ Serp/ Veinka/
│   │   │       ├── Fonts/ HeroBackground/ HeroIcons/ Maps/
│   │   │       ├── Material/ Shader/ SkillsIcon/ Sounds/
│   │   │       ├── Statues/ Textures/ VFX-Resources/
│   │   │   └── (Shader, Sprite, TextMesh Pro, Texture2D)
│   ├── Mesh/
│   └── Photon/                 ← Photon PUN2 library (206 files, KHÔNG sửa)
│       ├── PhotonChat/
│       ├── PhotonRealtime/
│       └── PhotonUnityNetworking/
├── _Scenes/                    ← game scenes
│   ├── SampleScene.unity       ← lobby/menu
│   └── Duy.unity               ← game scene
├── _Recovery/                  ← scene backup cũ (0.unity đến 0 (4).unity)
├── AssetResources/Player/
├── Resources/                  ← prefabs load bằng Resources.Load()
└── Settings/                   ← URP, InputSystem_Actions.inputactions
```

> Convention: tiền tố `_` cho custom content. Scenes ở `_Scenes/`, không phải `Scenes/`.

## Base Class

Tất cả MonoBehaviour trong project kế thừa `SaiMonoBehaviour`:

```csharp
protected virtual void Awake()          → LoadComponents() → ResetValue()
protected virtual void Start()          → (override để dùng)
protected virtual void Reset()          → LoadComponents() → ResetValue()
protected virtual void LoadComponents() // GetComponent ở đây
protected virtual void ResetValue()     // set default values ở đây
```

**Quy tắc:** Không dùng `GetComponent` trong `Update`. Luôn cache trong `LoadComponents`.

## Coding Conventions (Unity C#)

- Class: `PascalCase` (ví dụ: `EnemyStateMachine`, `PhotonRoom`)
- Method: `PascalCase` (ví dụ: `LoadComponents`, `TakeDamage`)
- Field public: `CamelCase` (ví dụ: `PhotonPlayerName`, `InputAction`)
- Field private: `camelCase` (ví dụ: `currentHP`, `isDashing`)
- Constant: `UPPER_SNAKE_CASE`
- Interface: prefix `I` (ví dụ: `INetworkService`, `IBossBehavior`)
- ScriptableObject class: suffix `SO` (ví dụ: `CharacterStatsSO`, `EnemyDataSO`)
- Không hardcode số liệu — dùng ScriptableObject

## Scripts Đã Có (Giai đoạn 1 & 2)

### Base
| File | Class | Ghi chú |
| ---- | ----- | ------- |
| `SaiMonoBehaviour.cs` | `SaiMonoBehaviour : MonoBehaviour` | Base class toàn project |

### Menu / Lobby
| File | Class | Chức năng |
| ---- | ----- | --------- |
| `PhotonLogin.cs` | `: MonoBehaviourPunCallbacks` | Login, connect, join lobby |
| `PhotonLogout.cs` | `: MonoBehaviour` | Disconnect |
| `PhotonRoom.cs` | `: MonoBehaviourPunCallbacks` | Tạo/join/leave phòng, list UI |
| `PhotonRoomAuto.cs` | `: MonoBehaviourPunCallbacks` | Auto create/join room |
| `PhotonStatus.cs` | `: MonoBehaviourPunCallbacks` | Hiển thị trạng thái mạng |
| `RoomProfile.cs` | `[Serializable]` | Data: `string name` |
| `UIRoomProfile.cs` | `: MonoBehaviour` | Room item UI, click handler |
| `PlayerProfile.cs` | `[Serializable]` | Data: `string nickName` |

### Player / Gameplay
| File | Class | Chức năng |
| ---- | ----- | --------- |
| `PhotonPlaying.cs` | `: MonoBehaviourPunCallbacks` | Spawn player theo actor number, static instance |
| `PlayerCtrl.cs` | `: SaiMonoBehaviour` | Controller chính; load PlayerMovement, PhotonView, PlayerAnimation; RPC sync anim |
| `PlayerMovement.cs` | `: SaiMonoBehaviour` | Move, jump, ground check; `moveSpeed=4f`, `jumpForce=6f`, `maxJumpCount=1` |
| `PlayerAnimation.cs` | `: SaiMonoBehaviour` | State machine: Idle/Run/Jump/Drop/Land/Aim/Shoot/Dash; fire GameEvents |
| `PlayerAbilityDash.cs` | `: SaiMonoBehaviour` | Dash: `cooldown=1f`, `dashForce=15f`, `dashDuration=0.2f` |
| `InputManager.cs` | `: MonoBehaviour` (Singleton) | Mouse position, right mouse states |
| `GameEvents.cs` | `static class` | Event bus (xem bên dưới) |

### Spawner / Pool
| File | Class | Chức năng |
| ---- | ----- | --------- |
| `Spawner.cs` | `: SaiMonoBehaviour` | Object pool cơ bản: load từ folder, pool/despawn |
| `PlayerSpawner.cs` | `: Spawner` (Singleton) | Extend Spawner cho player |
| `EnemySpawner.cs` | `: Spawner` (Singleton) | Extend Spawner cho enemy; chỉ MasterClient spawn |
| `PhotonPool.cs` | `: SaiMonoBehaviour, IPunPrefabPool` | Tích hợp Photon với pool system |

## GameEvents (Event Bus)

```csharp
public static class GameEvents
{
    public static Action OnPlayerJumped;
    public static Action OnPlayerLanded;
    public static Action<float> OnPlayerDashed;     // float = dashForce
    public static Action OnPlayerDashEnded;
    public static Action<PlayerState> OnAnimStateChanged;
    public static Action<bool> OnPlayerFacingChanged;
}
```

## PlayerAnimation States

```csharp
public enum PlayerState { Idle, Run, Jump, Drop, Land, Aim, Shoot, Dash }
```

State handlers: `HandleIdle()`, `HandleRun()`, `HandleJump()`, `HandleDrop()`, `HandleLand()`, `HandleAim()`, `HandleShoot()`, `HandleDash()` — mỗi handler check điều kiện, đổi state, fire GameEvents.

## PlayerCtrl RPC Methods

```csharp
[PunRPC] void SyncAnimState(PlayerState state)
[PunRPC] void RpcSetTrigger(string triggerName)
[PunRPC] void RpcSetFacing(bool facingRight)
[PunRPC] void RpcSetIsAiming(bool isAiming)
[PunRPC] void RpcShoot(...)
```

## Chưa Có (Giai đoạn tiếp theo)

- `INetworkService` / `PhotonNetworkAdapter` (Adapter pattern)
- `CharacterStatsSO`, `EnemyDataSO` (ScriptableObject data)
- `CharacterRuntimeStats` (clone SO khi spawn)
- `GameManager`, `WaveManager`, `BossManager` (Singleton managers)
- Full `StateMachine` (PlayerStateMachine, EnemyStateMachine, BossStateMachine)
- Combat system (damage, HP, death, revive)
- Enemy AI & scripts
- Boss system

## Architecture Decisions

### Data
- Stats dùng `ScriptableObject` (CharacterStatsSO, EnemyDataSO, ItemProfileSO)
- **Không ghi trực tiếp vào SO** — SO là shared asset. Clone ra `CharacterRuntimeStats` khi spawn

### Managers
- Singleton pattern cho GameManager, WaveManager, BossManager
- `PhotonRoom.instance`, `PhotonPlaying.instance` — hiện dùng pattern này (ghi chú "Dont do this in your game" trong code là reminder để refactor sau)

### Events
- `GameEvents` static class làm event bus trung tâm (Observer pattern) — **ĐÃ CÓ**
- UI chỉ subscribe event, không biết logic game

### Networking
- **Vertical Slice:** mỗi system: local hoạt động → add Photon → test 2 máy → sang tiếp
- Photon được bọc trong `INetworkService` / `PhotonNetworkAdapter` (Adapter pattern) — **CHƯA CÓ**
- Không gọi Photon API trực tiếp trong game logic
- Chỉ Host (IsMasterClient) spawn enemy và control boss

### Object Pool
- Dùng cho: enemy, projectile (Archer), item drop, VFX
- `Spawner.cs` → `PlayerSpawner`, `EnemySpawner` đã có
- `PhotonPool.cs` tích hợp với Photon's IPunPrefabPool

## Design Patterns áp dụng

| Pattern   | Áp dụng                                                            | Trạng thái |
| --------- | ------------------------------------------------------------------ | ---------- |
| State     | PlayerAnimation (đã có), PlayerStateMachine, EnemyStateMachine, BossStateMachine | Một phần |
| Command   | Input → Command → Execute local + gửi qua INetworkService         | Chưa có |
| Strategy  | IAttackStrategy (Melee/Ranged), IBossBehavior (Phase1/Phase2)     | Chưa có |
| Observer  | GameEvents static class — tất cả system publish/subscribe         | Đã có |
| Factory   | EnemyFactory.Spawn(), ItemDropFactory.CreateDrop()                | Chưa có |
| Adapter   | INetworkService ← PhotonNetworkAdapter                            | Chưa có |
| Singleton | InputManager, PhotonPlaying, PlayerSpawner, EnemySpawner          | Đã có |
| Pool      | Spawner → PlayerSpawner, EnemySpawner, PhotonPool                 | Đã có |
| Template  | SaiMonoBehaviour (LoadComponents → ResetValue)                    | Đã có |

## Photon Sync Rules

| Cần sync        | Cách sync                                |
| --------------- | ---------------------------------------- |
| Movement        | PhotonTransformView + PhotonAnimatorView |
| Damage/HP       | RPC                                      |
| Downed/Revive   | RPC                                      |
| Enemy spawn/HP  | Host spawn, RPC sync                     |
| Boss phase/HP   | RPC (Host authority)                     |
| Item pickup     | RPC                                      |
| Item trade      | RPC                                      |
| Gold, XP, Level | RPC                                      |
| Ping marker     | RPC                                      |
| Anim state      | RPC (SyncAnimState, RpcSetTrigger, ...)  |

**Sync movement đúng cách:**
```
Owner → execute locally → PhotonTransformView gửi position + velocity
Remote → Lerp/Extrapolate để mượt (KHÔNG sync raw position từng frame)
```

## 2 Nhân vật

|          | Chiến Binh                                  | Cung Thủ (_Velvet)           |
| -------- | ------------------------------------------- | ---------------------------- |
| Vai trò  | Tank                                        | DPS                          |
| Tấn công | Melee (hitbox Collider2D + Animation Event) | Ranged (projectile từ pool)  |
| Đặc biệt | Đứng yên → giảm 50% damage nhận             | Bắn/skill được khi airborne  |
| Skill    | Phải đứng yên (velocity ≈ 0)                | Activate được khi nhảy       |

Character assets có sẵn trong `_Assets/IMPORTANT/AssetResources/Character/AddressableResource/`:
`_Velvet`, `_Raidon`, `Bathos`, `Cala`, `Lucy`, `Mary`, `Morrod`, `Mortal`, `Serp`, `Veinka`

## Packages

| Package | Version | Ghi chú |
| ------- | ------- | ------- |
| `com.unity.render-pipelines.universal` | 17.3.0 | URP (Unity 6) |
| `com.unity.inputsystem` | 1.19.0 | New Input System |
| `com.unity.2d.animation` | 13.0.4 | 2D Skeleton animation |
| `com.unity.2d.aseprite` | 3.0.1 | Import Aseprite |
| `com.unity.2d.psdimporter` | 12.0.1 | PSD import |
| `com.unity.2d.spriteshape` | 13.0.0 | Sprite shapes |
| `com.unity.2d.tilemap` | 1.0.0 | Tilemap |
| `com.unity.2d.tilemap.extras` | 6.0.1 | Tilemap extras |
| `com.unity.timeline` | 1.8.12 | Timeline |
| `com.unity.ugui` | 2.0.0 | uGUI + TextMeshPro |
| `com.unity.visualscripting` | 1.9.11 | Visual scripting |
| **Photon PUN2** | — | In `_Assets/Photon/` (KHÔNG sửa) |

## Build Order (Task Order)

9 giai đoạn, **không bỏ qua thứ tự**:

1. Nền tảng + Photon cơ bản ✅ (xong: login, room, pool)
2. Nhân vật + Di chuyển ✅ (xong: movement, animation, dash, input) ← **đang ở đây**
3. Chiến đấu + Hồi sinh
4. Enemy + Wave
5. Boss
6. Inventory + Shop + Level
7. Thiết kế Map 1
8. UI + Polish
9. Tutorial

**Rule:** Gặp ký hiệu 🌐 → test 2 máy trước khi tiếp tục.

## Scenes

- `_Scenes/SampleScene.unity` — lobby/menu (PhotonLogin, PhotonRoom)
- `_Scenes/Duy.unity` — game scene (PhotonPlaying, PhotonPlayer spawn)
- `_Recovery/` — scene backup cũ, không dùng

## Điều cần xác định sau

- Chi tiết 3 skill của Chiến Binh + Cung Thủ
- Loại và số lượng enemy cụ thể
- Visual theme / art style (character nào dùng: _Velvet cho Archer?)
- Balance: HP, damage, speed, gold drop, item price, XP curve
- SFX & Music
- Tutorial map design
