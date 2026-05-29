# Folder Structure

```
Assets/
├── _Script/
│   ├── SaiMonoBehaviour.cs
│   ├── Parents/            ← base classes, singletons, GameEvents, DamageableCtrl
│   ├── Player/
│   │   └── Shoot/          ← IShootStrategy + implementations (Strategy pattern)
│   ├── Enemy/
│   │   ├── EnemyBat/       ← EnemyBatCtrl + BatCombat + EnemyFlyMovement
│   │   ├── EnemyExplosion/ ← EnemyExplosionCtrl + Combat/Animation/Movement/DamageReceiver
│   │   ├── EnemyMelee/     ← EnemyMeleeCtrl + Animation/Combat/Movement
│   │   ├── EnemySlime/     ← SlimeCtrl + variants (Blue/Carrot/Green)
│   │   ├── EnemyFactory/   ← Factory pattern (EnemyFactory, creators, enums)
│   │   ├── EnemyGroundShooter/ ← EnemyShooterCtrl + Airm + OnPlatform variants
│   │   └── Boss/
│   │       ├── BossFactory/
│   │       ├── SkillBossSpawner/
│   │       └── Devil/
│   │           ├── Skill/
│   │           │   └── DevilExplosion/
│   │           ├── Strategy/
│   │           └── UI/
│   ├── Arrow/              ← ArrowCtrl + Movement/DamageSender hierarchy
│   ├── Bullet/             ← BulletCtrl + movement variants + BulletDevilMovement
│   ├── FX/                 ← FXCtrl/FXSpawner/FXDespawn, FXName enum
│   ├── Wave/               ← WaveManager, WaveDataSO, TriggerZone, SpawnPointsManager
│   ├── Spawner/
│   ├── ItemDrop/           ← drop system: ItemDropCtrl, ItemDropSO, GoldDropSO, TableItemDrop
│   ├── TextSpawner/        ← floating text: damage (TextDamageCtrl) + default (TextDefaultCtrl)
│   ├── Chest/              ← Chest system + IntrinsicSkillSO, BaseInteract
│   ├── IntrinsicSkill/     ← passive skills activated from Chest (Strategy pattern)
│   ├── Menu/
│   ├── UI/
│   │   └── Inventory/
│   │       └── New/        ← UIInventorySlot, UIEquipSlot, UIItemContextMenu, UICharacterPanel
│   ├── Inventory/
│   │   └── Weapon/         ← WeaponDataSO, WeaponSkillSO, WeaponLevelData, enums
│   ├── NPCShop/            ← NPCShopManager, NPCShopData, UIItemShopManager, UIItemDetailBase hierarchy
│   └── NPCUpgrade/         ← NPCUpgradeManager, NPCUpgradeInteract, UI upgrade panels
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
  DamageableCtrl               ← abstract: PhotonView + RpcReceive() — kế thừa bởi EnemyCtrl và BossCtrl
  Singleton<T>                 ← generic singleton (kế thừa SaiMonoBehaviour)
  DamageReceiver               ← abstract: HP, Receiver(), Reduce(), OnDead()
  DamageSender                 ← base: maxDamage, baseDamage
  Despawn                      ← abstract: DespawnObject(), CanDespawn()
  DespawnByTime                ← auto-despawn sau N giây, reset timer OnEnable()
  InputManager                 ← singleton: mousePosition, rightMouse, F key
  GameEvents                   ← static event bus (C# Actions)
  CameraFollow                 ← smooth LateUpdate follow + SetTarget()
  GameManager                  ← singleton persistent: _sceneOrder[], stat multipliers per map, LoadNextScene() (MasterClient only)
  AudioManager                 ← singleton persistent: musicSource/SFXSource/UISource, PlaySFX/PlayUI, clips: shoot/dash/jump/hit/aim/explosion/gold
  GateReady                    ← khi OnAllWavesCleared → hiện gate, track players bước vào (RPC) → đủ tất cả → GameManager.LoadNextScene()
  FirebaseDatabaseManager      ← Firebase RTDB: WriteDatabase/ReadDatabase (stub/test)
  FirebaseLoginManager         ← Firebase Auth: register + login form UI, SwitchForm()

Player/
  PlayerCtrl                   ← root hub: PhotonView, AllPlayers static list, RPC entry point
  PlayerMovement               ← Rigidbody2D, jump (maxJumpCount=1), ground check, dash
  PlayerAnimation              ← state machine: PlayerState enum (9 states)
  PlayerAbilityDash            ← dash (force=15, cooldown=1s, duration=0.2s)
  PlayerDamageReceiver         ← HP=30, Revive(), OnPlayerDied event
  PlayerDamageSender           ← gửi damage → EnemyCtrl.RpcReceive (RpcTarget.All)
  PlayerDespawn                ← despawn sau 40s khi chết
  PlayerReviveHelper           ← giữ F 5s gần đồng đội ngã để hồi sinh
  PlayerGold                   ← maxGold=100000, AddGold(), TrySpendGold(), tự +10 gold mỗi 2s
  PlayerPickup                 ← OnTriggerEnter2D → nhặt ItemDrop, gọi PlayerGold hoặc InventoryManager
  PlayerItemTransfer           ← ReceiveItem(json) → ItemTransferData.Deserialize → InventoryManager.AddItem, spawn text "Inventory full!"
  PlayerHPBar                  ← world-space HP slider, subscribe PlayerDamageReceiver
  PlayerShootChargeBar         ← world-space charge bar khi hold shoot
  PlayerProfile                ← data: nickName
  PhotonPlaying                ← spawn player (Raidon) theo actor number, assign camera

  Shoot/
    IShootStrategy             ← interface: Shoot(prefabName, spawnCenter, angle180)
    PlayerShoot                ← aim angle + dispatch qua IShootStrategy, subscribe GameEvents.OnEquipmentChanged → RefreshStrategy()
    SingleShot                 ← 1 mũi tên thẳng
    DoubleShot                 ← 2 phát liên tiếp (coroutine)
    SpreadThreeShot            ← 3 mũi tên tỏa -15°/0°/+15°
    SpreadFiveShot             ← 5 mũi tên tỏa
    DoubleArrow                ← 2 mũi tên song song cùng lúc
    TripleArrow                ← 3 mũi tên song song cùng lúc

Enemy/ (base classes dùng chung)
  EnemyCtrl                   ← root hub (extends DamageableCtrl): expose DamageReceiver, Animation, Rigidbody2D, Despawn
  EnemyDamageReceiver         ← HP từ EnemyStatsSO, OnEnemyDied event, trigger hurt animation
  EnemyDamageSender           ← Send() chỉ MasterClient, RPC → PlayerCtrl
  EnemyAnimation              ← idle/hurt/attack/die, DespawnByEvent()
  EnemyCombatBase             ← abstract: virtual Send() — root của hierarchy combat
  EnemyCombat<TCtrl>          ← abstract generic (extends EnemyCombatBase): load _enemyCtrl + _playerLayer
  EnemyMeleeCombatBase<TCtrl> ← abstract melee (extends EnemyCombat): PointAttack, distanceToAttack=1.5, rangeAttack=0.5, UpdateTarget() abstract → OverlapCircle Send()
  EnemyMovement<TCtrl>        ← abstract generic: guard IsMine + isDead, moveSpeed=2
  EnemyMovementToTarget<TCtrl>← thêm _target, SetTarget(), minDistanceToStop=1
  EnemyRotate                 ← flip sprite theo velocity.x
  EnemyDespawn                ← PhotonNetwork.Destroy qua ctrl.PhotonView
  EnemyItemDropper            ← subscribe OnEnemyDied → spawn item theo TableItemDrop trong EnemyStatsSO
  EnemyStatsSO                ← ScriptableObject: baseMaxHP, physicalDefense, magicalDefense, List<TableItemDrop>
  EnemySpawner                ← singleton pool

  EnemyBat/
    EnemyBatCtrl              ← extends EnemyCtrl
    BatCombat                 ← melee OverlapCircle, distanceToAttack=1.5, cooldown=1s
    EnemyFlyMovement          ← velocity trực tiếp đến target (không dùng Rigidbody gravity)

  EnemyMelee/
    EnemyMeleeCtrl            ← extends EnemyCtrl
    EnemyMeleeAnimation       ← extends EnemyAnimation
    EnemyMeleeCombat          ← extends EnemyCombat<EnemyMeleeCtrl>
    EnemyMeleeMovement        ← extends EnemyMovementToTarget<EnemyMeleeCtrl>

  EnemySlime/
    SlimeCtrl                 ← extends EnemyCtrl
    SlimeDamageReceiver       ← extends EnemyDamageReceiver
    SlimeCombat               ← bắn 4 bullet theo 4 hướng (+45°/+135°/+225°/+315°), dùng NameBullet.Bullet_Fire
      SlimeBlueCombat         ← extends SlimeCombat
      SlimeCarrotCombat       ← extends SlimeCombat
      SlimeGreenCombat        ← extends SlimeCombat

  EnemyExplosion/
    EnemyExplosionCtrl        ← extends EnemyCtrl
    EnemyExplosionAnimation   ← extends EnemyAnimation
    EnemyExplosionCombat      ← combat logic đặc thù (nổ khi tới gần)
    EnemyExplosionDamageReceiver ← extends EnemyDamageReceiver
    EnemyExplosionMovement    ← extends EnemyMovementToTarget

  EnemyFactory/
    EnemyFactory              ← singleton: Dict<EnemyType, EnemyCreator>, Create()
    EnemyCreator              ← abstract: GetName(), Create(), OnCreated() callback
    BatCreator                ← tạo BatOrange, set first player làm target
    MagicMiniCreator          ← tạo MagicMini_Pink
    MeleeCreator              ← tạo enemy melee
    SlimeCreator              ← tạo enemy slime
    ExplosionCreator          ← tạo enemy explosion
    AirmCreator               ← tạo enemy air shooter
    ShooterOnPlatformCreator  ← tạo WandererMagican
    EnemyType (enum)          ← Bat, MagicMini, ShooterOnPlatform, Melee, Slime, Explosion, Airm
    EnemyName (enum)          ← BatOrange, MagicMini_Pink, WandererMagican, ...

  EnemyGroundShooter/
    EnemyShooterCtrl          ← extends EnemyCtrl
    EnemyShooterMovement      ← patrol + raycast ground check, SetMoving()
    EnemyShooterCombatBase    ← abstract (extends EnemyCombat<EnemyShooterCtrl>): PointShoot, detect(r=10/12), prepare → shoot → SpawnBullet()
    EnemyShooterCombat        ← extends EnemyShooterCombatBase: detect(r=10) → prepare(0.7s) → shoot → cooldown(5s)
    MagicMiniCombat           ← extends EnemyShooterCombat: bulletName=Bullet_Fire
    EnemyShooterAnimation     ← extends EnemyAnimation
    EnemyAirmCombat           ← combat variant cho enemy bắn trên không
    EnemyMovementOnPlatform   ← patrol trên platform

  Boss/
    BossCtrl                  ← root hub (extends DamageableCtrl): expose DamageReceiver, BossAnimation, Rigidbody2D, BossDespawn, BossRotate
    BossDamageReceiver        ← abstract base HP cho boss
    BossDamageSender          ← Send() chỉ MasterClient
    BossAnimation             ← base animation cho boss
    BossCombat<TCtrl>         ← abstract generic: load _bossCtrl, chỉ MasterClient thực thi
    BossMovement              ← abstract: movement cho boss
    BossRotate                ← flip sprite
    BossDespawn               ← PhotonNetwork.Destroy

    BossFactory/
      BossCreator             ← abstract
      BossName (enum)         ← Devil, ...
      BossSpawner             ← singleton pool

    SkillBossSpawner/
      SkillBossSpawner        ← singleton pool cho projectile/FX của boss skill

    Devil/
      DevilCtrl               ← extends BossCtrl: thêm DevilMovement, DevilCombat, ChildLeft/ChildRight transform
      DevilAnimation          ← extends BossAnimation: SetSkill1Trigger(), SetSkill2Trigger()
      DevilCombat             ← extends BossCombat<DevilCtrl>: chọn phase strategy, cooldown=10s, ExecuteRandomSkill()
      DevilDamageReceiver     ← phase transition tại 50% HP → SetPhase2()
      DevilDamageSender       ← extends BossDamageSender
      DevilMovement           ← extends BossMovement
      DevilRotate             ← extends BossRotate

      Strategy/
        IBossPhaseStrategy    ← interface: Execute()
        BossPhaseStrategy     ← abstract base: load _devilCtrl, _abilities[]
        DevilPhase1Strategy   ← extends BossPhaseStrategy: chọn ngẫu nhiên ability phase 1
        DevilPhase2Strategy   ← extends BossPhaseStrategy: chọn ngẫu nhiên ability phase 2

      Skill/
        BaseBossAbility       ← abstract: load _devilCtrl, SetTargets(), Execute()
        NameSkillBoss (enum)  ← BulletVolley, ShootHorizantal, ShootVertical, SpikeBurst, Summon, VoidZone
        BulletVolleyAbility   ← abstract: bắn N viên đạn từ ChildLeft+ChildRight về 2 player, interval giữa mỗi viên
          BulletVolleyPhase1  ← BulletCount ít hơn
          BulletVolleyPhase2  ← BulletCount nhiều hơn
        ShootHorizantal       ← abstract: bắn ngang
          ShootHorizantalPhase1 / Phase2
        ShootVertical         ← abstract: bắn dọc
          ShootVerticalPhase1 / Phase2
        SpikeBurstAbility     ← abstract: spike burst AoE
          SpikeBurstPhase1 / Phase2
        SummonAbility         ← abstract: summon thêm enemy
          SummonPhase1 / Phase2
        VoidZoneAbility       ← abstract: tạo vùng damage theo thời gian
          VoidZonePhase1 / Phase2

        DevilExplosion/
          DevilExplosionCtrl            ← hub
          DevilExplosionAnimation       ← animation nổ
          DevilExplosionDamageSender    ← AoE damage khi nổ
          DevilExplosionDespawn         ← despawn sau khi animation kết thúc

      UI/
        DevilPhysHPBar        ← thanh HP vật lý của Devil
        DevilMagHPBar         ← thanh HP phép của Devil

Wave/
  WaveManager                ← singleton: load WaveDataSO per scene, spawn per wave, track alive, OnAllWavesCleared
  WaveDataSO                 ← ScriptableObject: List<SpawnEnemyProfile>
  SpawnEnemyGroup.cs         ← file chứa class SpawnEnemyProfile: EnemyType + count [Serializable]
  SpawnPointsManager         ← load spawn points từ child transforms (Points_Bat, ...)
  TriggerZone                ← OnTriggerEnter2D → spawn wave qua EnemyFactory

Arrow/
  ArrowCtrl                  ← hub: PhotonView, ArrowDespawn
  ArrowMovement              ← Translate X với speed=50
  ArrowDespawn               ← DespawnByTime 3s + PhotonNetwork.Destroy
  ArrowSpawner               ← object pool singleton (max 100)
  ArrowType (enum)           ← Normal, Explosive, Piercing
  ArrowName (enum)           ← tên prefab arrow
  Movement                   ← abstract base: maxSpeed, baseMaxSpeed — FixedUpdate → Move()

  ArrowDamageSender          ← abstract base: load ArrowCtrl, SetDamage(phys,mag,pen), _hasHit flag
    ArrowNormalDamageSender  ← 1 hit → RpcReceive → despawn
    ArrowPiercingDamageSender  ← xuyên _maxHit=3 kẻ thù rồi despawn
    ArrowExplosiveDamageSender ← hit + AoE OverlapCircle(r=1.5) + spawn FX ImpactArrowExplosive
    ArrowRicochetDamageSender  ← hit + spawn _ricochetCount=1 ArrowNormal ngẫu nhiên hướng
    ArrowGoldDamageSender    ← x1.5 damage bonus

Bullet/
  BulletCtrl                 ← hub: PhotonView, BulletDespawn
  BulletMovement             ← Translate X với speed=10
  BulletDevilMovement        ← extends Movement, tốc độ riêng cho Devil bullets
  BulletDamageSender         ← OnTriggerEnter2D → PlayerCtrl.RpcReceive (one-hit flag)
  BulletDespawn              ← DespawnByTime 5s + PhotonNetwork.Destroy
  BulletSpawner              ← object pool singleton (max 100)
  NameBullet (enum)          ← Bullet_Fire, Bullet_Devil, ...

FX/
  FXCtrl                     ← hub cho VFX prefab
  FXSpawner                  ← extends Spawner, singleton, pool VFX max 50
  FXDespawn                  ← extends DespawnByTime, timeDespawn=1s
  FXName (enum)              ← ImpactArrowExplosive, ...

ItemDrop/
  ItemDropCtrl               ← hub: ItemDropDespawn, ItemPickupable, ItemDropMove, Rigidbody2D
  ItemDropSO                 ← ScriptableObject base: tên prefab, icon
    GoldDropSO               ← extends ItemDropSO: giá trị gold
  TableItemDrop              ← [Serializable]: ItemDropSO + _rate (xác suất drop)
  ItemPickupable             ← OnTriggerEnter2D → gọi PlayerPickup.Pickup(this)
  ItemDropMove               ← animation arc khi drop (nhảy lên sau đó rơi)
  ItemDropDespawn            ← extends DespawnByTime, tự despawn sau N giây
  ItemDropSpawner            ← singleton pool max 200
  NameItemDrop (enum)        ← tên prefab item drop

TextSpawner/
  TextSpawner                ← extends Spawner, singleton, SpawnText(pos,physDmg,magDmg) + SpawnTextDefault(pos) max 100
  TextDamageCtrl             ← hub: TextMeshPro TextPhys + TextMagic
  TextDamageMovement         ← float-up animation
  TextDamageDespawn          ← tự despawn sau thời gian
  TextDefaultCtrl            ← hub: TextMeshPro, SetText(string content)
  TextDefaultMovement        ← extends Movement, float-up theo Vector3.up
  TextDespawn                ← extends DespawnByTime (timeDespawn=0.5s), despawn về TextSpawner pool

Chest/
  BaseInteract               ← abstract: IconKeyE, OnTriggerEnter/Exit2D, Update → Interact()
  ChestCtrl                  ← root hub: ChestData, ChestModel
  ChestData                  ← load List<IntrinsicSkillSO> từ Resources/IntrinsicSkill, pick 3 ngẫu nhiên khi Start
  ChestModel                 ← visual/animation của rương
  ChestInteract              ← extends BaseInteract: mở UI chest khi nhấn E
  UIChestSkillSlot           ← 1 ô skill trong UI chest
  UIChestSkillPanel          ← panel hiển thị 3 skill để chọn
  IntrinsicSkillSO           ← ScriptableObject: _id, _icon, _name (IntrinsicSkillName), _description
  IntrinsicSkillName (enum)  ← tên các passive skill

IntrinsicSkill/
  BaseIntrinsicSkill         ← abstract: _isActive, _data (SO), _player (PlayerCtrl), Activate(so)
  PlayerIntrinsicSkillManager← load GetComponentsInChildren<BaseIntrinsicSkill>, AddSkill(so) → Activate
  BuffHP                     ← +flat HP
  BuffPercentHP              ← +% HP
  BuffCritical               ← +crit rate
  BuffDamagePhysical         ← +physical damage
  BuffDamageMagical          ← +magical damage
  BuffArmorPenetration       ← +armor penetration
  AutoShield                 ← tự động shield khi bị tấn công
  CrisisArmor                ← tăng armor khi HP thấp
  DashHeal                   ← hồi HP khi dash
  LastStand                  ← buff damage khi HP thấp
  ReviveBurst                ← buff khi được hồi sinh

Spawner/
  Spawner                    ← generic pool: FolderPrefabs, spawn/despawn by name or transform
  PhotonPool                 ← IPunPrefabPool → bridge Photon.Instantiate → Spawner
  PlayerSpawner / EnemySpawner ← singletons
  PlayerSpawnPoint           ← static singleton: Get() → spawn position cho PhotonPlaying

Menu/
  PhotonLogin                ← connect + set nickname
  PhotonLogout               ← disconnect
  PhotonRoom                 ← tạo/join room, lobby list UI
  PhotonRoomAuto             ← auto create/join (test only)
  PhotonStatus               ← hiện connection state
  RoomProfile                ← data: room name
  UIRoomProfile              ← room list item UI, click to select

UI/
  CenterCtrl                 ← singleton: quản lý panels chính (OpenShop, ...)
  FollowPlayer               ← world-space UI facing camera (LateUpdate)
  PlayerHPSlider             ← HP bar theo PlayerDamageReceiver
  UIEventSystem              ← singleton persistent (EventSystem wrapper)
  Parents/ BaseBtn, BaseSlider, BaseText  ← abstract UI bases

  Inventory/ (cũ — sẽ refactor)
    DragController           ← singleton: drag icon giữa slots
    UIInventoryManager       ← 9 slot inventory, Tab để mở, Refresh() lấy data từ local player
    BtnOpenInventory / BtnCloseParent ← mở/đóng panel
    BtnEquip                 ← extends BaseBtn (stub)
    UIItemDetailInventory    ← extends UIItemDetailBase, Show(item), ẩn khi click ngoài

  Inventory/New/ (mới — đang dùng)
    UIInventorySlot          ← drag&drop + right-click → UIItemContextMenu, hiện icon + amount
    UIEquipSlot              ← 1 ô trang bị (Weapon/Armor/Pants), SetItem()
    UIItemContextMenu        ← singleton popup: chuột phải item Equipment → nút Equip
    UICharacterPanel         ← singleton: 3 UIEquipSlot + stats text, TryEquip(), RefreshSlotEquip()
    UITransferTarget         ← drop zone kéo item → chuyển cho teammate (RPC), ẩn (alpha 0.3 + blocksRaycasts=false) nếu solo

Inventory/
  ItemDataSO                 ← ScriptableObject base: _id, _icon, _typeItem, _price
  EquippableDataSO           ← extends ItemDataSO: _equipType, _equipmentRarity (base cho trang bị)
  EquipmentDataSO            ← extends EquippableDataSO: _physicalDefense, _magicalDefense, _hpBonus
  PowerUpDataSO              ← extends ItemDataSO: _value, _duration (0=vĩnh viễn)
  ItemInventoryBase          ← [Serializable]: _info (ItemDataSO), _amount, _currentLevel
  InventoryManager           ← MaxSlot=9, Swap(), Remove(), FindSlotFirstEmpty(), AddItem()
  EquipmentManager           ← 3-slot (Weapon/Armor/Pants), Equip(), GetCurrentEquip()
  TypeItem (enum)            ← Equipment=0, PowerUp=1
  EquipType (enum)           ← Weapon=0, Armor=1, Pants=2, Null=3
  ItemRarity (enum)          ← Common/Rare/Epic/Legend + extension ToColor()
  ItemTransferData           ← [Serializable]: JSON bridge cho RPC item transfer, Serialize(ItemInventoryBase) / Deserialize(json) → ItemInventoryBase

  Weapon/
    WeaponDataSO             ← extends EquippableDataSO: _levels[], _skills[], _arrowType
    WeaponSkillSO            ← ScriptableObject: _skillName, _description, _type, _skillRarity
    WeaponSkillName (enum)   ← SingleShot, DoubleShot, SpreadThreeShot, SpreadFiveShot, DoubleArrow, TripleArrow, ...
    WeaponLevelData          ← [Serializable]: armorPenetration, criticalRate, physicalDamageBonus, magicalDamageBonus, upgradeCost
    EquipmentRarity (enum)   ← Common, Rare, Epic, Legend
    SkillWeaponRarity (enum) ← Common, Rare, Epic, Legend

NPCShop/
  NPCShopManager             ← singleton (extends Singleton<T>): expose NPCShopData
  NPCShopData                ← load List<EquipmentDataSO/WeaponDataSO/PowerUpDataSO> từ Resources
  NPCShopInteract            ← extends BaseInteract: mở shop qua CenterCtrl.OpenShop() khi nhấn E
  UIItemShopManager          ← singleton: 6 UIShopSlot, TryBuyItem() dùng PlayerGold + InventoryManager, Refresh()
  UIShopSlot                 ← 1 ô shop: icon image, SetItem(), IPointerClickHandler, IsSold flag
  BtnBuy                     ← extends BaseBtn, gọi UIItemShopManager.TryBuyItem()
  TextValueGold              ← extends BaseText, hiển thị CurrentGold của local player (lazy load)

  UIItemDetailBase           ← abstract base: avatar, name (color theo rarity), stat rows, ShowWeapon/Equipment/PowerUp
    UIItemDetailShop         ← extends UIItemDetailBase: thêm price + sold state, Show(item, slot)
    UIItemDetailInventory    ← extends UIItemDetailBase: Show(item), ẩn khi click ngoài panel

NPCUpgrade/
  NPCUpgradeManager          ← singleton (stub — chưa hoàn chỉnh)
  NPCUpgradeInteract         ← extends BaseInteract: mở upgrade UI khi nhấn E
  BtnUpgrade                 ← extends BaseBtn, trigger upgrade action
  UIDetailCurrentLevel       ← hiển thị stats cấp hiện tại của weapon/equipment
  UIDetailNextLevel          ← hiển thị stats cấp tiếp theo + upgrade cost
  UIItemUpgradeManager       ← singleton: quản lý panel nâng cấp item
```
