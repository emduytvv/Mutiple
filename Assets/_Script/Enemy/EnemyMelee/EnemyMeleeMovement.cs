using UnityEngine;

public class EnemyMeleeMovement : EnemyMovementToTarget<EnemyMeleeCtrl>
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _rayLength = 1f;
    private float _moveDir = 1f;
    private float _rateSpeedOnTarget = 2f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGroundLayer();
    }
    private void LoadGroundLayer()
    {
        if (this._groundLayer != 0) return;
        this._groundLayer = LayerMask.GetMask("Ground");
        Debug.Log(transform.name + ": Load GroundLayer", gameObject);
    }
    protected override void Move()
    {
        if (_target != null)
        {
            MoveToTarget();
            return;
        }
        MoveNotTarget();
    }
    private void GetDistance()
    {
        _currentDistance = Vector2.Distance(transform.position, _target.position);
    }
    private bool CanMove()
    {
        return _currentDistance > _minDistanceToStop;
    }
    private void MoveToTarget()
    {
        GetDistance();
        if (!CanMove())
        {
            _enemyCtrl.Rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }
        _direction = _target.position - transform.position;
        _moveDir = _direction.x > 0 ? 1f : -1f;
        _enemyCtrl.Rigidbody2D.linearVelocity = new Vector2(_moveDir * maxSpeed * _rateSpeedOnTarget, _enemyCtrl.Rigidbody2D.linearVelocity.y);
    }
    private void MoveNotTarget()
    {
        if (!HasGroundAhead()) _moveDir *= -1f;
        else if (HasWallAhead()) _moveDir *= -1f;
        _enemyCtrl.Rigidbody2D.linearVelocity = new Vector2(_moveDir * maxSpeed, _enemyCtrl.Rigidbody2D.linearVelocity.y);
    }


    private bool HasGroundAhead()
    {
        Vector2 origin = transform.position;
        return Physics2D.Raycast(origin, Vector2.down, _rayLength, _groundLayer);
    }
    private bool HasWallAhead()
    {
        Vector2 origin = transform.position + Vector3.up * 0.5f;
        return Physics2D.Raycast(origin, new Vector2(_moveDir, 0), _rayLength, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = transform.position + Vector3.right * 0.1f;
        Gizmos.DrawLine(origin, origin + Vector2.down * _rayLength);

        Gizmos.color = Color.green;
        origin = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawLine(origin, origin + new Vector2(_moveDir, 0) * _rayLength);
    }
}
