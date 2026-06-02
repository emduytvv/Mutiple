using UnityEngine;

public class HPBarEnemySpawner : Spawner
{
    protected string _objectName = "HPBarEnemy";
    private static HPBarEnemySpawner _instance;
    public static HPBarEnemySpawner Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 100;
    }
    public Transform SpawnHPBar(Vector3 pos, Quaternion rot)
    {
        Transform obj = SpawnByName(_objectName, pos, rot);
        obj.gameObject.SetActive(true);
        return obj;
    }
}
