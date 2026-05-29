public class MeleeCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.Melee;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.Satyr_Brown);
        _enemyNames.Add(EnemyName.Satyr_Brown_2);
        _enemyNames.Add(EnemyName.Satyr_Brown_3);
    }
}

