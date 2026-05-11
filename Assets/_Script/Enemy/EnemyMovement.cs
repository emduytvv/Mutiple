using UnityEngine;

public class EnemyMovement : Movement
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;
    [SerializeField] protected float _moveSpeed = 2f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    protected override void FixedUpdate()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        Move();
    }

    protected override void Move()
    {
    }
}
