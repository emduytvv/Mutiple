using Photon.Pun;
using UnityEngine;

public class ArrowDamageSender : DamageSender
{
    [SerializeField] protected ArrowCtrl _arrowCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadArrowCtrl();
    }

    private void LoadArrowCtrl()
    {
        if (_arrowCtrl != null) return;
        _arrowCtrl = GetComponentInParent<ArrowCtrl>();
    }

    private bool _hasHit = false;

    private void OnEnable() => _hasHit = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasHit) return;
        EnemyCtrl enemy = collision.GetComponentInParent<EnemyCtrl>();
        if (enemy == null) return;

        if (!_arrowCtrl.PhotonView.IsMine) return;
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, maxDamage);

        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
