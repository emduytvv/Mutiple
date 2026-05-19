public class AirmCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNames();
        LoadType();
    }
    private void LoadType()
    {
        _enemyType = EnemyType.Airm;
    }
    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.Sniper);
    }

}
