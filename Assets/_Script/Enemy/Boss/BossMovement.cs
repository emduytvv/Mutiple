using UnityEngine;

public abstract class BossMovement<TCtrl> : Movement where TCtrl : BossCtrl
{
    [SerializeField] protected TCtrl _bossCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBossCtrl();
    }

    private void LoadBossCtrl()
    {
        if (_bossCtrl != null) return;
        _bossCtrl = GetComponentInParent<TCtrl>();
    }

    protected override void FixedUpdate()
    {
        if (!_bossCtrl.PhotonView.IsMine) return;
        if (_bossCtrl.DamageReceiver.isDead)
        {
            _bossCtrl.Rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }
        Move();
    }
}
