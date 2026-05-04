using System.Collections.Generic;
using UnityEngine;

public class Spawner : SaiMonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Transform FolderPrefabs;
    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] protected Transform Holder;
    [SerializeField] protected List<Transform> pools;
    [SerializeField] protected int currentObject;
    [SerializeField] protected int maxObject = 10;
    private HashSet<Transform> spawnedObjects = new HashSet<Transform>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadFolderPrefabs();
        this.Loadprefabs();
        this.LoadHolder();
    }

    protected virtual void LoadFolderPrefabs()
    {
        if (FolderPrefabs != null) return;
        FolderPrefabs = transform.Find("Prefabs");
        Debug.LogWarning(transform.name + ": LoadFolderPrefabs()", gameObject);
    }

    protected virtual void LoadHolder()
    {
        if (Holder != null) return;
        Holder = transform.Find("Holder");
        Debug.LogWarning(transform.name + ": LoadHolder()", gameObject);
    }


    protected virtual void Loadprefabs()
    {
        if (this.prefabs.Count > 0) return;
        foreach (Transform prefab in FolderPrefabs)
        {
            prefabs.Add(prefab);
            prefab.gameObject.SetActive(false);
        }
    }

    public virtual Transform SpawnByName(string name, Vector3 position, Quaternion rotation)
    {
        if (currentObject >= maxObject) return null;
        Transform prefab = this.GetTransformByName(name);
        return this.SpawnByTransform(prefab, position, rotation);
    }

    public virtual Transform SpawnByTransform(Transform prefab, Vector3 position, Quaternion rotation)
    {
        if (currentObject >= maxObject) return null;
        Transform obj = this.GetObjectFromPool(prefab);
        obj.SetPositionAndRotation(position, rotation);

        Vector3 pos = obj.position;
        pos.z = 0f;
        obj.position = pos;

        spawnedObjects.Add(obj);
        currentObject++;
        return obj;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform pool in pools)
        {
            if (pool.name != prefab.name) continue;
            pools.Remove(pool);
            return pool;
        }
        Transform prefabClone = Instantiate(prefab, Holder);
        prefabClone.name = prefab.name;
        return prefabClone;
    }

    protected virtual Transform GetTransformByName(string name)
    {
        foreach (Transform prefab in prefabs)
        {
            if (prefab.name == name) return prefab;
        }
        Debug.LogWarning(transform.name + ": Not found prefab name: " + name, gameObject);
        return null;
    }

    public virtual void Despawn(Transform obj)
    {
        if (pools.Contains(obj)) return;
        obj.gameObject.SetActive(false);
        spawnedObjects.Remove(obj);
        pools.Add(obj);
        currentObject--;
    }

    // ── Helper cho PhotonPool ──────────────────────────────────────────
    public bool HasPrefab(string name)
    {
        foreach (Transform prefab in prefabs)
            if (prefab.name == name) return true;
        return false;
    }
    public bool IsOwner(Transform obj) => spawnedObjects.Contains(obj);
}
