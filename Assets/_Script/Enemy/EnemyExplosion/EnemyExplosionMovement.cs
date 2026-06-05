using UnityEngine;

public class EnemyExplosionMovement : EnemyMovementToTarget<EnemyExplosionCtrl>
{
    protected override void Move()
    {
        if (_target == null) return;
        TryRefreshTarget();
        if (_target == null) return;
        UpdateDirection();
        _enemyCtrl.Rigidbody2D.linearVelocity = _direction.normalized * _moveSpeed;
    }

    private void TryRefreshTarget()
    {
        PlayerCtrl current = _target.GetComponent<PlayerCtrl>();
        if (current == null || !current.PlayerDamageReceiver.isDead) return;
        PlayerCtrl alive = PlayerCtrl.AllPlayers.Find(p => !p.PlayerDamageReceiver.isDead);
        _target = alive != null ? alive.transform : null;
    }

    private void UpdateDirection()
    {
        _direction = _target.position - transform.position;
    }
}
