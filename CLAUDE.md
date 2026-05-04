# CLAUDE.md — 2D Co-op Action Platformer

## Project Overview

**Game:** 2D co-op action platformer, 1–2 người chơi qua mạng
**Stack:** Unity 6 · Photon PUN 2 · New Input System · URP 17.3 · TextMeshPro
**Mục tiêu:** Portfolio project để đi thực tập — 4 tháng kinh nghiệm Unity
**Scope:** Tutorial map + Map 1 (playable) · Map 2+ chỉ hiển thị locked
**Thời gian mỗi map:** 10–15 phút

## Đường dẫn Projects

| Project                         | Đường dẫn                                                | Unity Version |
| ------------------------------- | -------------------------------------------------------- | ------------- |
| **Multiple** (project hiện tại) | `C:\Users\Windows\Music\Học code\Unity Project\Multiple` | Unity 6       |
| **Surival** (project cũ)        | `C:\Users\Windows\Music\Học code\Project 1\Surival`      | 2022.3.62f3   |

**GitHub:** https://github.com/emduytvv/Mutiple

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
│   ├── Arrow/                  ← projectile: movement, damage sender, spawner
│   ├── Enemy/                  ← enemy damage receiver/sender
│   ├── Menu/                   ← Photon lobby/room scripts
│   ├── Parents/                ← base classes dùng chung (DamageReceiver, DamageSender, Movement, Despawn, GameEvents, InputManager)
│   ├── Player/                 ← gameplay: movement, animation, input, combat
│   ├── Spawner/                ← object pool, enemy/player/arrow spawner
│   └── UI/                     ← HP bar, follow player, base UI components
│       └── Parents/            ← BaseBtn, BaseSlider, BaseText
├── _Assets/
│   ├── Avatar/
│   ├── Editor/                 ← FixAnimations.cs, AudioMixerPostprocessor.cs
│   ├── IMPORTANT/
│   │   ├── AssetResources/
│   │   │   └── Character/AddressableResource/
│   │   │       ├── _Velvet/    ← Cung Thủ 1 (DPS)
│   │   │       ├── _Raidon/    ← Cung Thủ 2
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

**Quy tac GetComponent:**

- Khong dung GetComponent trong Update. Luon cache trong LoadComponents.
- Component tren chinh object: `GetComponent<T>()`
- Component tren cha: `GetComponentInParent<T>()`
- Component tren con: `GetComponentInChildren<T>()`
- Component cung cap (sibling — vi du: Movement, Despawn, DamageSender cung nam duoi 1 parent): `transform.parent.GetComponentInChildren<T>()`
- Runtime trigger (OnTriggerEnter2D,...): dung `TryGetComponent` thay cho `GetComponent` — khong allocate khi khong tim thay.

**Quy tac Update/FixedUpdate:**

- Update chi chua guard check + goi 1 ham duy nhat — khong viet logic truc tiep.
- Moi hanh dong tach thanh ham rieng co ten ro vai tro: `HandleAimInput()`, `StartAim()`, `ReleaseAim()`, ...
- Tinh toan phuc tap tach thanh ham rieng: `GetArrowSpawnPos()`, `UpdateAimAngle180()`, ...

```csharp
// DUNG
private void Update()
{
    if (!_photonView.IsMine) return;
    HandleAimInput();
}
private void HandleAimInput() { ... }

// SAI
private void Update()
{
    if (Input.GetMouseButtonDown(1)) { _isAiming = true; GameEvents... }
}
```

## GameEvents (Event Bus)

File: `Assets/_Script/Parents/GameEvents.cs`

## PlayerAnimation States

```csharp
public enum PlayerState { Idle, Run, Jump, Drop, Land, Aim, Shoot, Dash, Die }

### Managers

- Singleton pattern cho GameManager, WaveManager, BossManager
- `PhotonRoom.instance`, `PhotonPlaying.instance` — hiện dùng pattern này (ghi chú "Dont do this in your game" trong code là reminder để refactor sau)

### Events

- `GameEvents` static class làm event bus trung tâm (Observer pattern) — **ĐÃ CÓ**
- UI chỉ subscribe event, không biết logic game

- Chỉ Host (IsMasterClient) spawn enemy và control boss

### Object Pool

- Dùng cho: enemy, projectile (Archer), item drop, VFX
- `Spawner.cs` → `PlayerSpawner`, `EnemySpawner` đã có
- `PhotonPool.cs` tích hợp với Photon's IPunPrefabPool

## Photon RPC Rules

**RPC phai nam tren cung GameObject voi PhotonView** — Photon khong tim xuong children.

```

Root (PhotonView + EnemyCtrl) ← [PunRPC] dat o day ✓
└── EnemyDamageReceiver ← [PunRPC] o day = KHONG HOAT DONG ❌

```

Pattern chuan: tao Ctrl class tren root nhan RPC → goi xuong child component.
Vi du: PlayerCtrl (root, co PhotonView) nhan RpcReceive → goi PlayerDamageReceiver.Receiver()
Vi du: EnemyCtrl (root, co PhotonView) nhan RpcReceive → goi EnemyDamageReceiver.Receiver()

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
   |

Character assets: `_Velvet` (Cung Thủ 1) · `_Raidon` (Cung Thủ 2)

## Packages

| **Photon PUN2** | — | In `_Assets/Photon/` (KHÔNG sửa) |

## Build Order (Task Order)

9 giai đoạn, **không bỏ qua thứ tự**:

1. Nền tảng + Photon cơ bản ✅ (xong: login, room, pool)
2. Nhân vật + Di chuyển ✅ (xong: movement, animation, dash, input, shoot)
3. Chiến đấu + Hồi sinh 🔄 (đang làm: DamageSender/Receiver ✅, HP bar ✅, arrow ✅ — còn: kết nối damage, downed/revive, sync) ← **đang ở đây**
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
```
