using UnityEngine;

public class EnemyFlyMovement : EnemyMovementToTarget<EnemyCtrl>
{
    protected override void Move()
    {
        if (_target == null) return;
        UpdateDistance();
        if (!CanMove())
        {
            _enemyCtrl.Rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }
        _enemyCtrl.Rigidbody2D.linearVelocity = _direction.normalized * _moveSpeed;
    }

    private void UpdateDistance()
    {
        _direction = _target.position - transform.position;
        _currentDistance = _direction.magnitude;
    }

    private bool CanMove()
    {
        return _currentDistance > _minDistanceToStop;
    }
}
