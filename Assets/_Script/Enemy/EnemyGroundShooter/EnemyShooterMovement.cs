using UnityEngine;

public class EnemyShooterMovement : EnemyMovement
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
        _enemyCtrl.Rigidbody2D.linearVelocity = new Vector2(_moveDir * maxSpeed, _enemyCtrl.Rigidbody2D.linearVelocity.y);
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
    }
}
