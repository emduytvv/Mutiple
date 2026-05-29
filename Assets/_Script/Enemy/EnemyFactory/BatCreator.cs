using UnityEngine;
public class BatCreator : EnemyCreator
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        _enemyType = EnemyType.Bat;
        LoadNames();
    }

    private void LoadNames()
    {
        if (_enemyNames.Count > 0) return;
        _enemyNames.Add(EnemyName.BatOrange);
        _enemyNames.Add(EnemyName.BatPink);
        _enemyNames.Add(EnemyName.BatBrown);
        _enemyNames.Add(EnemyName.BatBlack);
        _enemyNames.Add(EnemyName.BatPurple);
    }

    protected override void OnCreated(EnemyCtrl enemy)
    {
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        EnemyFlyMovement fly = enemy.GetComponentInChildren<EnemyFlyMovement>();
        if (fly == null) return;
        int index = Random.Range(0, PlayerCtrl.AllPlayers.Count);
        fly.SetTarget(PlayerCtrl.AllPlayers[index].transform);
    }
}
