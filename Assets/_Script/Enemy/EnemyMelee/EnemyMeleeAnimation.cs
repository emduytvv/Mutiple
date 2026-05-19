using UnityEngine;

public class EnemyMeleeAnimation : EnemyAnimation
{
    private EnemyMeleeCtrl MeleeCtrl => _enemyCtrl as EnemyMeleeCtrl;

    public void AttackByEvent()
    {
        MeleeCtrl.MeleeCombat.Attack();
    }
}
