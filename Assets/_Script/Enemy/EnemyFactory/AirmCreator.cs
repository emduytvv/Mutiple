public class AirmCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.Airm;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.Sniper);
    }
}
