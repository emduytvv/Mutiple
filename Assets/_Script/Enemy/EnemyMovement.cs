using UnityEngine;

public abstract class EnemyMovement<TCtrl> : Movement where TCtrl : EnemyCtrl
{
    [SerializeField] protected TCtrl _enemyCtrl;
    [SerializeField] protected float _moveSpeed = 2f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<TCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    protected override void FixedUpdate()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        Move();
    }
}
