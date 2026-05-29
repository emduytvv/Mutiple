public class BatCombat : EnemyMeleeCombatBase<EnemyBatCtrl>
{
    protected override void UpdateTarget()
    {
        _target = _enemyCtrl.FlyMovement.Target;
    }
}
