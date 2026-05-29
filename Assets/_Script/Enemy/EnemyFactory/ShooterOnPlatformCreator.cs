public class ShooterOnPlatformCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.ShooterOnPlatform;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.WandererMagican);
    }
}
