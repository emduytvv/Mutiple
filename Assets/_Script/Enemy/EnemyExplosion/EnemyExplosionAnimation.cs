using UnityEngine;

public class EnemyExplosionAnimation : EnemyAnimation
{
    private EnemyExplosionCtrl ExplosionCtrl => _enemyCtrl as EnemyExplosionCtrl;
    public void DeadAnim()
    {
        if (_dieTriggered) return;
        _dieTriggered = true;
        _animator.ResetTrigger(HashHurt);
        _animator.SetTrigger(HashDie);

    }
    public void SendDamageByEvent()
    {
        ExplosionCtrl.ExplosionCombat.BlastDamage();
    }
}
