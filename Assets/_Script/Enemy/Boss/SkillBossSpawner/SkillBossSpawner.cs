public class SkillBossSpawner : Spawner
{
    private static SkillBossSpawner _instance;
    public static SkillBossSpawner Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 50;
    }
}
