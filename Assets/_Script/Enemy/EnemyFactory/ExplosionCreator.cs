using UnityEngine;
public class ExplosionCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.Explosion;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.BatExplosion);
    }

    protected override void OnCreated(EnemyCtrl enemy)
    {
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        EnemyExplosionMovement fly = enemy.GetComponentInChildren<EnemyExplosionMovement>();
        if (fly == null) return;
        fly.SetTarget(PlayerCtrl.AllPlayers[0].transform);
    }
}
