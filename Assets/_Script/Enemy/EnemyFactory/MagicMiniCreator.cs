public class MagicMiniCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.MagicMini;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.MagicMini_Pink);
        _enemyNames.Add(EnemyName.MagicMini_Brown);
        _enemyNames.Add(EnemyName.MagicMini_Green);
    }
}
