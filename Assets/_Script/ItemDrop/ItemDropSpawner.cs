
using UnityEngine;
public class ItemDropSpawner : Spawner
{
    private static ItemDropSpawner _instance;
    public static ItemDropSpawner Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 200;
    }
    public void SpawnByCall(string prefabName, Vector3 pos, Quaternion rot)
    {
        Transform obj = SpawnByName(prefabName, pos, rot);
        obj.gameObject.SetActive(true);
    }

}
