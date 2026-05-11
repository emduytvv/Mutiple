public class ShooterOnPlatformCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNames();
        LoadType();
    }
    private void LoadType()
    {
        _enemyType = EnemyType.ShooterOnPlatform;
    }
    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.WandererMagican);
    }

}
