→ player.PhotonView.RPC("RpcReceive", All, phys, mag)
→ PlayerDamageReceiver.Receiver()

===== SCENE TRANSITION (CHUYEN SCENE) =====

--- KHÁI NIỆM ---

- SceneManager.LoadScene() → DESTROY tất cả object trong scene cũ
- DontDestroyOnLoad(gameObject) → giữ object sống qua scene mới
- PhotonNetwork.LoadLevel("SceneName") → thay SceneManager, tự sync tất cả client
- PhotonNetwork.AutomaticallySyncScene = true → set khi connect, tất cả client load cùng scene

--- NHÓM A: GIỮ LẠI (DontDestroyOnLoad) ---
Object tồn tại xuyên suốt cả trận, chỉ tạo 1 lần:

- InputManager ← singleton global
- EventSystem ← Unity UI cần đúng 1 cái, tạo lại sẽ conflict
- PhotonPlaying ← quản lý room + player spawn flow
- PhotonPool ← map tên prefab → Spawner, mất là Photon.Instantiate fail
- Players (Velvet/Raidon) ← mang theo HP, inventory, gold sang map mới
- Canvas (HUD) ← HP bar, inventory UI hiển thị liên tục
- CameraFollow ← follow player persistent

--- NHÓM B: TẠO LẠI MỖI SCENE (scene-bound) ---
Nằm trong từng scene, bị destroy và tạo mới theo map:

- WaveManager ← mỗi map có WaveDataSO riêng
- TriggerZoneManager ← layout trigger khác nhau mỗi map
- EnemyFactory + EnemySpawner ← pool kẻ thù reset, fresh start
- ArrowSpawner, BulletSpawner, FXSpawner, TextSpawner
- ItemDropSpawner, SkillBossSpawner
- BossSpawner ← không phải map nào cũng có boss
- NPCShop, NPCUpgrade ← nội dung shop có thể khác nhau
- Chest, Chest_1 ← map-specific pickup
- Map (geometry) ← đây chính là thứ thay đổi

--- VẤN ĐỀ SINGLETON + MULTI-SCENE ---
Scene 1: ArrowSpawner tạo, Instance = A
Scene 2 load: ArrowSpawner mới tạo, Instance vẫn = A (đã destroy) → NullRef!

Fix — scene-bound singleton (tạo lại mỗi scene):
protected override void LoadComponents()
{
base.LoadComponents();
Instance = this; // override instance cũ đã bị destroy
}

Fix — persistent singleton (DontDestroyOnLoad):
protected override void LoadComponents()
{
if (Instance != null && Instance != this) { Destroy(gameObject); return; }
Instance = this;
DontDestroyOnLoad(gameObject);
}

--- FLOW CHUYỂN SCENE ---
[MasterClient: all waves cleared + boss dead]
→ MasterClient gọi photonView.RPC("RpcLoadNextMap", All, mapIndex)
→ Tất cả client nhận RPC → hiện loading screen
→ MasterClient gọi PhotonNetwork.LoadLevel("Level1_Map2")
→ AutomaticallySyncScene = true → tất cả client cùng load
→ NHÓM B bị DESTROY (WaveManager, spawners, enemies, ...)
→ NHÓM A VẪN CÒN (players, canvas, photon objects, ...)
→ Scene mới load xong → PhotonPlaying.OnLevelWasLoaded() được gọi
→ Player đã tồn tại → chỉ reposition về spawn point
→ WaveManager mới tự init từ WaveDataSO của scene đó

--- CODE MẪU: SceneTransitionManager ---
public class SceneTransitionManager : SaiMonoBehaviour
{
[SerializeField] private int \_currentMapIndex = 1;

    public void LoadNextMap()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        _currentMapIndex++;
        photonView.RPC("RpcLoadNextMap", RpcTarget.All, _currentMapIndex);
    }

    [PunRPC]
    private void RpcLoadNextMap(int mapIndex)
    {
        // TODO: hiện loading screen
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel($"Level1_Map{mapIndex}");
    }

}

PhotonPlaying thêm callback:
void OnLevelWasLoaded(int level)
{
RepositionPlayersToSpawnPoints(); // Player đã exist, chỉ move về spawn
}
............................
Tier 1 — Phải làm (core Firebase skills)

1. Authentication (Google Sign-In)
   Thay Photon nickname bằng account thật. Học được: auth flow, token, user session.

Login Google → lấy uid → dùng uid làm key cho mọi thứ trong Firestore 2. Firestore — Cloud Save
Lưu player data persistent qua session:

users/{uid}/
├── gold: 500
├── inventory: [{id, amount}, ...]
├── equippedWeapon: "Bow_Epic"
└── equippedArmor: "Armor_Rare"
Học được: CRUD, realtime listener, offline cache.

Tier 2 — Nên làm (differentiate portfolio) 3. Leaderboard
Firestore collection lưu clear time tốt nhất mỗi map:

leaderboard/map1/
└── [{uid, nickname, clearTime, date}, ...]
Học được: query, orderBy, limit, pagination. Đây là feature rất hay để show trong portfolio.

4. Security Rules
   Viết rules để user chỉ đọc/ghi được data của mình:

match /users/{userId} {
allow read, write: if request.auth.uid == userId;
}
Học được: Firestore security — kỹ năng thực tế rất quan trọng.
