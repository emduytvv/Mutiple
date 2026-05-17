using Photon.Pun;
using UnityEngine;

public class ArrowPiercingDamageSender : ArrowDamageSender
{
    protected float _maxHit = 3;
    private float _currentHit;

    protected override void OnEnable()
    {
        base.OnEnable();
        _currentHit = _maxHit;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyCtrl enemy = collision.GetComponentInParent<EnemyCtrl>();
        if (enemy == null) return;
        if (!_arrowCtrl.PhotonView.IsMine) return;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage, 0f);
        CheckCanDespawn();
    }

    private void CheckCanDespawn()
    {
        _currentHit--;
        if (_currentHit > 0) return;
        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
