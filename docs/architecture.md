# Folder Structure
```
Assets/
├── _Script/
│   ├── SaiMonoBehaviour.cs
│   ├── Parents/            ← base classes, singletons, GameEvents
│   ├── Player/
│   │   └── Shoot/          ← IShootStrategy + implementations (Strategy pattern)
│   ├── Enemy/
│   │   ├── EnemyFactory/   ← Factory pattern (EnemyFactory, EnemyCreator, BatCreator, ...)
│   │   └── EnemyGroundShooter/
│   ├── Arrow/              ← ArrowCtrl + Movement/DamageSender base classes
│   ├── Bullet/
│   ├── Wave/               ← WaveManager, WaveDataSO, TriggerZone, SpawnPointsManager
│   ├── Spawner/
│   ├── TextSpawner/        ← floating damage text (TextDamageCtrl, TextSpawner pool)
│   ├── Chest/              ← Chest system + IntrinsicSkillSO, BaseInteract
│   ├── IntrinsicSkill/     ← passive skills activated from Chest (Strategy pattern)
│   ├── Menu/
│   ├── UI/
│   │   └── Inventory/
│   │       └── New/        ← UIInventorySlot, UIEquipSlot, UIItemContextMenu, UICharacterPanel
│   ├── Inventory/
│   │   └── Weapon/         ← WeaponDataSO, WeaponSkillSO, WeaponLevelData, enums
│   └── NPCShop/            ← NPCShopManager, NPCShopData, UIShopManager, UIShopSlot
├── _Assets/
│   ├── IMPORTANT/AssetResources/Character/AddressableResource/
│   │   ├── _Velvet/        ← Cung Thủ 1 (DPS)
│   │   └── _Raidon/        ← Cung Thủ 2
│   └── Photon/             ← Photon PUN2 (KHÔNG SỬA — 206 files)
├── _Scenes/
├── Resources/
│   ├── ItemData/Equipment/ ← EquipmentDataSO assets (Armor/Pants × 4 rarity)
│   ├── ItemData/Weapon/    ← WeaponDataSO assets (Bow × 4 rarity)
│   ├── ItemData/PowerUp/   ← PowerUpDataSO assets
│   ├── IntrinsicSkill/     ← IntrinsicSkillSO assets
│   └── WaveData/           ← WaveDataSO per scene
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
  InputManager                   ← singleton: mousePosition, rightMouse, F key
  GameEvents                     ← static event bus (C# Actions)
  CameraFollow                   ← smooth LateUpdate follow + SetTarget()

Player/
  PlayerCtrl                     ← root hub: PhotonView, AllPlayers static list, RPC entry point
  PlayerMovement                 ← Rigidbody2D, jump (maxJumpCount=1), ground check, dash
  PlayerAnimation                ← state machine: PlayerState enum (9 states)
  PlayerAbilityDash              ← dash (force=15, cooldown=1s, duration=0.2s)
  PlayerDamageReceiver           ← HP=30, Revive(), OnPlayerDied event
  PlayerDamageSender             ← gửi damage → EnemyCtrl.RpcReceive (RpcTarget.All)
  PlayerDespawn                  ← despawn sau 40s khi chết
  PlayerReviveHelper             ← giữ F 5s gần đồng đội ngã để hồi sinh
  PlayerProfile                  ← data: nickName
  PhotonPlaying                  ← spawn player (Raidon) theo actor number, assign camera

  Shoot/
    IShootStrategy               ← interface: Shoot(prefabName, spawnCenter, angle180)
    PlayerShoot                  ← aim angle + dispatch qua IShootStrategy, subscribe GameEvents.OnEquipmentChanged để RefreshStrategy()
    SingleShot                   ← 1 mũi tên thẳng
    DoubleShot                   ← 2 phát liên tiếp (coroutine)
    SpreadThreeShot              ← 3 mũi tên tỏa -15°/0°/+15°
    SpreadFiveShot               ← 5 mũi tên tỏa
    DoubleArrow                  ← 2 mũi tên song song cùng lúc
    TripleArrow                  ← 3 mũi tên song song cùng lúc

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
  WaveManager                    ← singleton: load WaveDataSO per scene, spawn per wave, track alive, OnAllWavesCleared
  WaveDataSO                     ← ScriptableObject: List<SpawnEnemyProfile>
  SpawnEnemyGroup.cs             ← file chứa class SpawnEnemyProfile: EnemyType + count [Serializable]
  SpawnPointsManager             ← load spawn points từ child transforms (Points_Bat, ...)
  TriggerZone                    ← OnTriggerEnter2D → spawn wave qua EnemyFactory

Arrow/
  ArrowCtrl                      ← hub: PhotonView, ArrowDespawn
  ArrowMovement                  ← Translate X với speed=50
  ArrowDamageSender              ← OnTriggerEnter2D → EnemyCtrl.RpcReceive (one-hit flag)
  ArrowDespawn                   ← DespawnByTime 3s + PhotonNetwork.Destroy
  ArrowSpawner                   ← object pool singleton (max 100)
  ArrowExplosion                 ← OverlapCircleAll(r=1.5) → RpcReceive AoE damage khi nổ
  ArrowType (enum)               ← Normal, Explosive, Piercing
  Movement                       ← abstract base: maxSpeed, baseMaxSpeed — FixedUpdate → Move()

Bullet/
  BulletCtrl                     ← hub: PhotonView, BulletDespawn
  BulletMovement                 ← Translate X với speed=10
  BulletDamageSender             ← OnTriggerEnter2D → PlayerCtrl.RpcReceive (one-hit flag)
  BulletDespawn                  ← DespawnByTime 5s + PhotonNetwork.Destroy
  BulletSpawner                  ← object pool singleton (max 100)

TextSpawner/
  TextSpawner                    ← extends Spawner, singleton, SpawnText(pos, physDmg, magDmg) max 100
  TextDamageCtrl                 ← hub: TextMeshPro TextPhys + TextMagic
  TextDamageMovement             ← float-up animation
  TextDamageDespawn              ← tự despawn sau thời gian

Chest/
  BaseInteract                   ← abstract: IconKeyE, OnTriggerEnter/Exit2D, Update → Interact()
  ChestCtrl                      ← root hub: ChestData, ChestModel
  ChestData                      ← load List<IntrinsicSkillSO> từ Resources/IntrinsicSkill, pick 3 ngẫu nhiên khi Start
  ChestModel                     ← visual/animation của rương
  ChestInteract                  ← extends BaseInteract: mở UI chest khi nhấn E
  UIChestSkillSlot               ← 1 ô skill trong UI chest
  UIChestSkillPanel              ← panel hiển thị 3 skill để chọn
  IntrinsicSkillSO               ← ScriptableObject: _id, _icon, _name (IntrinsicSkillName), _description
  IntrinsicSkillName (enum)      ← tên các passive skill

IntrinsicSkill/
  BaseIntrinsicSkill             ← abstract: _isActive, _data (SO), _player (PlayerCtrl), Activate(so)
  PlayerIntrinsicSkillManager    ← load GetComponentsInChildren<BaseIntrinsicSkill>, AddSkill(so) → Activate
  BuffHP                         ← +flat HP
  BuffPercentHP                  ← +% HP
  BuffCritical                   ← +crit rate
  BuffDamagePhysical             ← +physical damage
  BuffDamageMagical              ← +magical damage
  BuffArmorPenetration           ← +armor penetration
  AutoShield                     ← tự động shield khi bị tấn công
  CrisisArmor                    ← tăng armor khi HP thấp
  DashHeal                       ← hồi HP khi dash
  LastStand                      ← buff damage khi HP thấp
  ReviveBurst                    ← buff khi được hồi sinh

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

  Inventory/ (cũ — sẽ refactor)
    DragController               ← singleton: drag icon giữa slots
    UIInventoryManager           ← 9 slot inventory, Tab để mở, Refresh() lấy data từ local player
    BtnOpenInventory / BtnCloseParent ← mở/đóng panel
    BtnEquip                     ← extends BaseBtn (stub)

  Inventory/New/ (mới — đang dùng)
    UIInventorySlot              ← drag&drop + right-click → UIItemContextMenu, hiện icon + amount
    UIEquipSlot                  ← 1 ô trang bị (Weapon/Armor/Pants), SetItem()
    UIItemContextMenu            ← singleton popup: chuột phải item Equipment → nút Equip
    UICharacterPanel             ← singleton: 3 UIEquipSlot + stats text, TryEquip(), RefreshSlotEquip()

Inventory/
  ItemDataSO                     ← ScriptableObject base: _id, _icon, _typeItem
  EquippableDataSO               ← extends ItemDataSO: _equipType, _equipmentRarity (base cho trang bị)
  EquipmentDataSO                ← extends EquippableDataSO: _physicalDefense, _magicalDefense, _hpBonus
  PowerUpDataSO                  ← extends ItemDataSO: _value, _duration (0=vĩnh viễn)
  ItemInventoryBase              ← [Serializable]: _info (ItemDataSO), _amount, _currentLevel
  InventoryManager               ← MaxSlot=9, Swap(), Remove(), FindSlotFirstEmpty()
  EquipmentManager               ← 3-slot (Weapon/Armor/Pants), Equip(), GetCurrentEquip()
  TypeItem (enum)                ← Equipment=0, PowerUp=1
  EquipType (enum)               ← Weapon=0, Armor=1, Pants=2, Null=3

  Weapon/
    WeaponDataSO                 ← extends EquippableDataSO: _levels[], _skills[], _arrowType
    WeaponSkillSO                ← ScriptableObject: _skillName, _description, _type, _skillRarity
    WeaponSkillName (enum)       ← SingleShot, DoubleShot, SpreadThreeShot, SpreadFiveShot, DoubleArrow, TripleArrow, ...
    WeaponLevelData              ← [Serializable]: armorPenetration, criticalRate, physicalDamageBonus, magicalDamageBonus, upgradeCost
    EquipmentRarity (enum)       ← Common, Rare, Epic, Legend
    SkillWeaponRarity (enum)     ← Common, Rare, Epic, Legend

NPCShop/
  NPCShopManager                 ← singleton (extends Singleton<T>): expose NPCShopData
  NPCShopData                    ← load List<EquipmentDataSO/WeaponDataSO/PowerUpDataSO> từ Resources
  NPCShopInteract                ← extends BaseInteract: mở UI shop khi nhấn E
  UIShopManager                  ← quản lý UI shop (stub)
  UIItemDetailManager            ← hiển thị chi tiết item khi hover/click (stub)
  UIShopSlot                     ← 1 ô shop: icon image, SetItem(), IPointerClickHandler
```
