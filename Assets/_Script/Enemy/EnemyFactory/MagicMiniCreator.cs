public class MagicMiniCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNames();
        LoadType();
    }
    private void LoadType()
    {
        _enemyType = EnemyType.MagicMini;
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.MagicMini_Pink);
    }

}
