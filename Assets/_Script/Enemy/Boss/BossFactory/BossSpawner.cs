public class BossSpawner : Spawner
{
    private static BossSpawner _instance;
    public static BossSpawner Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 5;
    }
}
