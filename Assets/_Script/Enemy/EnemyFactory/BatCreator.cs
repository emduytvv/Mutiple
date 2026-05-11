using UnityEngine;
public class BatCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadType();
        LoadNames();
    }

    private void LoadType()
    {
        _enemyType = EnemyType.Bat;
    }


    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.BatOrange);
    }

    protected override void OnCreated(EnemyCtrl enemy)
    {
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        EnemyFlyMovement fly = enemy.GetComponentInChildren<EnemyFlyMovement>();
        if (fly == null) return;
        fly.SetTarget(PlayerCtrl.AllPlayers[0].transform);
    }
}
