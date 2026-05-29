using UnityEngine;

public class EnemyExplosionDamageReceiver : EnemyDamageReceiver
{
    private EnemyExplosionCtrl ExplosionCtrl => _enemyCtrl as EnemyExplosionCtrl;

    protected override void OnHurt()
    {
        base.OnHurt();
    }
    protected override void OnDead()
    {
        base.OnDead();
        Debug.Log(transform.name + ": OnDead", gameObject);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ExplosionSFX);
    }

}
