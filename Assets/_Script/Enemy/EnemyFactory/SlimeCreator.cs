public class SlimeCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.Slime;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.SlimeCarrot);
        _enemyNames.Add(EnemyName.SlimeGreen);
        _enemyNames.Add(EnemyName.SlimeBlue);
    }
}
