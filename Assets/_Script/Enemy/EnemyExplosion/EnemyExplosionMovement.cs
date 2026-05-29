using UnityEngine;

public class EnemyExplosionMovement : EnemyMovementToTarget<EnemyExplosionCtrl>
{
    protected override void Move()
    {

        if (_target == null) return;
        UpdateDirection();
        _enemyCtrl.Rigidbody2D.linearVelocity = _direction.normalized * _moveSpeed;
    }
    private void UpdateDirection()
    {
        _direction = _target.position - transform.position;
    }
}
