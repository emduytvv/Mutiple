# Folder Structure
```
Assets/
├── _Script/
│   ├── SaiMonoBehaviour.cs
│   ├── Parents/            ← base classes, singletons, GameEvents
│   ├── Player/
│   ├── Enemy/
│   │   ├── EnemyFactory/   ← Factory pattern (EnemyFactory, EnemyCreator, BatCreator, ...)
│   │   └── EnemyGroundShooter/
│   ├── Arrow/
│   ├── Bullet/
│   ├── Wave/               ← WaveManager, WaveDataSO, TriggerZone, SpawnPointsManager
│   ├── Spawner/
│   ├── Menu/
│   ├── UI/
│   └── Inventory/
├── _Assets/
│   ├── IMPORTANT/AssetResources/Character/AddressableResource/
│   │   ├── _Velvet/        ← Cung Thủ 1 (DPS)
│   │   └── _Raidon/        ← Cung Thủ 2
│   └── Photon/             ← Photon PUN2 (KHÔNG SỬA — 206 files)
├── _Scenes/
├── Resources/              ← prefabs load bằng Resources.Load()
└── Settings/               ← URP, InputSystem_Actions.inputactions
```

# Scripts Reference
```
SaiMonoBehaviour.cs              ← base class cho tất cả, LoadComponents() + ResetValue()

Parents/
  Singleton<T>                   ← generic singleton (kế thừa SaiMonoBehaviour)
  DamageReceiver                 ← abstract: HP, Receiver(), Reduce(), OnDead()
  DamageSender                   ← base: maxDamage, baseDamage
  Despawn                        ← abstract: DespawnObject(), CanDespawn()
  DespawnByTime                  ← auto-despawn sau N giây, reset timer OnEnable()
  Movement                       ← abstract: FixedUpdate → Move()
  InputManager                   ← singleton: mousePosition, rightMouse, F key
  GameEvents                     ← static event bus (C# Actions)
  CameraFollow                   ← smooth LateUpdate follow + SetTarget()

Player/
  PlayerCtrl                     ← root hub: PhotonView, AllPlayers list, RPC entry point
  PlayerMovement                 ← Rigidbody2D, jump (maxJumpCount=1), ground check, dash
  PlayerAnimation                ← state machine: PlayerState enum (9 states)
  PlayerAbilityDash              ← dash (force=15, cooldown=1s, duration=0.2s)
  PlayerShoot                    ← aim angle + spawn Arrow via PhotonNetwork.Instantiate
  PlayerDamageReceiver           ← HP=30, Revive(), OnPlayerDied event
  PlayerDamageSender             ← gửi damage → EnemyCtrl.RpcReceive (RpcTarget.All)
  PlayerDespawn                  ← despawn sau 40s khi chết
  PlayerReviveHelper             ← giữ F 5s gần đồng đội ngã để hồi sinh
  PlayerProfile                  ← data: nickName
  PhotonPlaying                  ← spawn player (Raidon) theo actor number, assign camera

Enemy/
  EnemyCtrl                      ← root hub: expose PhotonView, DamageReceiver, Animation, Rigidbody2D, Despawn
  EnemyDamageReceiver            ← HP=2, OnEnemyDied event, trigger hurt animation
  EnemyDamageSender              ← Send() chỉ MasterClient, RPC → PlayerCtrl
  EnemyAnimation                 ← idle/hurt/attack/die, DespawnByEvent()
  EnemyCombat                    ← abstract: load _enemyCtrl + _playerLayer
  EnemyMovement                  ← abstract: guard IsMine + isDead, moveSpeed=2
  EnemyMovementToTarget          ← thêm _target, SetTarget(), minDistanceToStop=1
  EnemyFlyMovement               ← velocity trực tiếp đến target (dùng cho Bat)
  EnemyRotate                    ← flip sprite theo velocity.x
  EnemyDespawn                   ← PhotonNetwork.Destroy qua ctrl.PhotonView
  BatCombat                      ← melee OverlapCircle, distanceToAttack=1.5, cooldown=1s

  EnemyFactory/
    EnemyFactory                 ← singleton: Dict<EnemyType, EnemyCreator>, Create()
    EnemyCreator                 ← abstract: GetName(), Create(), OnCreated() callback
    BatCreator                   ← tạo BatOrange, set first player làm target
    MagicMiniCreator             ← tạo MagicMini_Pink
    ShooterOnPlatformCreator     ← tạo WandererMagican
    EnemyType (enum)             ← Bat, MagicMini, ShooterOnPlatform
    EnemyName (enum)             ← BatOrange, MagicMini_Pink, WandererMagican

  EnemyGroundShooter/
    EnemyShooterCtrl             ← extends EnemyCtrl (placeholder)
    EnemyShooterMovement         ← patrol + raycast ground check, SetMoving()
    EnemyShooterCombat           ← detect(r=10) → prepare(0.7s) → shoot → cooldown(5s)
    EnemyShooterAnimation        ← extends EnemyAnimation (placeholder)

Wave/
  WaveManager                    ← singleton: load WaveDataSO, spawn per wave, track alive count, OnAllWavesCleared
  WaveDataSO                     ← ScriptableObject: List<SpawnEnemyProfile>
  SpawnEnemyProfile              ← EnemyType + count
  SpawnPointsManager             ← load spawn points từ child transforms (Points_Bat, ...)
  TriggerZone                    ← OnTriggerEnter2D → spawn wave qua EnemyFactory

Arrow/
  ArrowCtrl                      ← hub: PhotonView, ArrowDespawn
  ArrowMovement                  ← Translate X với speed=50
  ArrowDamageSender              ← OnTriggerEnter2D → EnemyCtrl.RpcReceive (one-hit flag)
  ArrowDespawn                   ← DespawnByTime 3s + PhotonNetwork.Destroy
  ArrowSpawner                   ← object pool singleton (max 100)

Bullet/
  BulletCtrl                     ← hub: PhotonView, BulletDespawn
  BulletMovement                 ← Translate X với speed=10
  BulletDamageSender             ← OnTriggerEnter2D → PlayerCtrl.RpcReceive (one-hit flag)
  BulletDespawn                  ← DespawnByTime 5s + PhotonNetwork.Destroy
  BulletSpawner                  ← object pool singleton (max 100)

Spawner/
  Spawner                        ← generic pool: FolderPrefabs, spawn/despawn by name or transform
  PhotonPool                     ← IPunPrefabPool → bridge Photon.Instantiate → Spawner
  PlayerSpawner / EnemySpawner   ← singletons

Menu/
  PhotonLogin                    ← connect + set nickname
  PhotonLogout                   ← disconnect
  PhotonRoom                     ← tạo/join room, lobby list UI
  PhotonRoomAuto                 ← auto create/join (test only)
  PhotonStatus                   ← hiện connection state
  RoomProfile                    ← data: room name
  UIRoomProfile                  ← room list item UI, click to select

UI/
  CenterCtrl                     ← singleton: quản lý panels chính
  FollowPlayer                   ← world-space UI facing camera (LateUpdate)
  PlayerHPSlider                 ← HP bar theo PlayerDamageReceiver
  Parents/ BaseBtn, BaseSlider, BaseText  ← abstract UI bases
  Inventory/ DragController, UIInventoryManager, UIInventorySlot, BtnCloseParent, BtnOpenInventory

Inventory/
  InventoryManager               ← List<ItemBase>, Swap(), Remove()
  ItemBase                       ← ItemDataSO + amount
  ItemDataSO                     ← ScriptableObject: id, icon, TypeItem
  TypeItem (enum)                ← Equipment, PowerUp
```
