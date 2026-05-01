using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonPool : SaiMonoBehaviour, IPunPrefabPool
{
    [SerializeField] private List<Spawner> spawners;

    protected override void Awake()
    {
        base.Awake();
        PhotonNetwork.PrefabPool = this;
    }
    protected override void Start()
    {
        base.Start();
        spawners.Add(EnemySpawner.Instance.GetComponent<Spawner>());
        spawners.Add(PlayerSpawner.Instance.GetComponent<Spawner>());
        spawners.Add(ArrowSpawner.Instance.GetComponent<Spawner>());
    }
    public GameObject Instantiate(string prefabId, Vector3 pos, Quaternion rot)
    {
        foreach (Spawner spawner in spawners)
        {
            if (!spawner.HasPrefab(prefabId)) continue;
            Transform spawned = spawner.SpawnByName(prefabId, pos, rot);
            if (spawned == null) continue;
            return spawned.gameObject;
        }
        Debug.LogWarning("PhotonPool: không tìm thấy prefab: " + prefabId);
        return null;
    }

    public void Destroy(GameObject go)
    {
        foreach (Spawner spawner in spawners)
        {
            if (!spawner.IsOwner(go.transform)) continue;
            spawner.Despawn(go.transform);
            return;
        }
        Debug.LogWarning("PhotonPool: không tìm thấy owner của: " + go.name);
    }
}
