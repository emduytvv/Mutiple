using UnityEngine;

public class EnemyMovementOnPlatform : EnemyMovement<EnemyCtrl>
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _rayLength = 0.5f;
    private float _moveDir = 1f;
    [SerializeField] private bool _isMoving = true;

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

    public void SetMoving(bool moving)
    {
        _isMoving = moving;
        if (!_isMoving) _enemyCtrl.Rigidbody2D.linearVelocity = new Vector2(0f, _enemyCtrl.Rigidbody2D.linearVelocity.y);
    }

    protected override void Move()
    {
        if (!_isMoving) return;
        if (!HasGroundAhead()) _moveDir *= -1f;
        else if (HasWallAhead()) _moveDir *= -1f;
        _enemyCtrl.Rigidbody2D.linearVelocity = new Vector2(_moveDir * _moveSpeed, _enemyCtrl.Rigidbody2D.linearVelocity.y);
    }
    private bool HasWallAhead()
    {
        Vector2 origin = transform.position + Vector3.up * 0.5f;
        return Physics2D.Raycast(origin, new Vector2(_moveDir, 0), _rayLength, _groundLayer);
    }
    private bool HasGroundAhead()
    {
        Vector2 origin = transform.position;
        return Physics2D.Raycast(origin, Vector2.down, _rayLength, _groundLayer);
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
