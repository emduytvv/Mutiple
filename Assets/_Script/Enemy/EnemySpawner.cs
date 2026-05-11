using UnityEngine;

public class EnemySpawner : Spawner
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance => _instance;

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

}
