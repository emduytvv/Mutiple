using UnityEngine;

public abstract class EnemyMovement<TCtrl> : Movement where TCtrl : EnemyCtrl
{
    [SerializeField] protected TCtrl _enemyCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<TCtrl>();
    }

    public override void ApplyMultiplier(float multiplier)
    {
        _moveSpeed *= multiplier;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        if (_enemyCtrl?.EnemyStatsSO == null) return;
        if (_enemyCtrl.EnemyStatsSO._moveSpeed <= 0) return;
        _moveSpeed = _enemyCtrl.EnemyStatsSO._moveSpeed;
    }

    protected override void FixedUpdate()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead)
        {
            _enemyCtrl.Rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }
        Move();
    }
}
