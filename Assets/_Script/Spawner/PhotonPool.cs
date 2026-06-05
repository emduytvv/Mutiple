using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonPool : SaiMonoBehaviour, IPunPrefabPool
{
    [SerializeField] private List<Spawner> spawners;

    protected override void Awake()
    {
        base.Awake();
        PhotonNetwork.PrefabPool = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawners.Clear();
    }

    private void RegisterSpawners()
    {
        TryAddSpawner(EnemySpawner.Instance);
        TryAddSpawner(PlayerSpawner.Instance);
        TryAddSpawner(ArrowSpawner.Instance);
        TryAddSpawner(BulletSpawner.Instance);
        TryAddSpawner(TextSpawner.Instance);
        TryAddSpawner(FXSpawner.Instance);
        TryAddSpawner(BossSpawner.Instance);
        TryAddSpawner(SkillBossSpawner.Instance);
        TryAddSpawner(ItemDropSpawner.Instance);
        TryAddSpawner(HPBarEnemySpawner.Instance);
    }

    private void TryAddSpawner(MonoBehaviour instance)
    {
        if (instance == null) { Debug.LogWarning("PhotonPool: một spawner singleton chưa được khởi tạo"); return; }
        Spawner spawner = instance.GetComponent<Spawner>();
        if (spawner == null) { Debug.LogWarning($"PhotonPool: {instance.name} không có component Spawner"); return; }
        spawners.Add(spawner);
    }

    public GameObject Instantiate(string prefabId, Vector3 pos, Quaternion rot)
    {
        if (spawners.Count == 0) RegisterSpawners();
        foreach (Spawner spawner in spawners)
        {
            if (!spawner.HasPrefab(prefabId)) continue;
            Transform spawned = spawner.SpawnByName(prefabId, pos, rot);
            if (spawned == null)
            {
                Debug.LogWarning("PhotonPool: pool đã đầy cho: " + prefabId);
                return null;
            }
            return spawned.gameObject;
        }
        Debug.LogWarning("PhotonPool: không tìm thấy prefab: " + prefabId);
        return null;
    }

    public void Destroy(GameObject go)
    {
        if (spawners.Count == 0) RegisterSpawners();
        foreach (Spawner spawner in spawners)
        {
            if (!spawner.IsOwner(go.transform)) continue;
            spawner.Despawn(go.transform);
            return;
        }
        Debug.LogWarning("PhotonPool: không tìm thấy owner của: " + go.name);
    }
}
