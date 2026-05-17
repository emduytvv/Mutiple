using Photon.Pun;
using UnityEngine;

public class ArrowGoldDamageSender : ArrowDamageSender
{
    protected float _rateDamage = 1.5f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasHit) return;
        EnemyCtrl enemy = collision.GetComponentInParent<EnemyCtrl>();
        if (enemy == null) return;

        if (!_arrowCtrl.PhotonView.IsMine) return;
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, 1.5f * basePhysicalDamage, 1.5f * baseMagicalDamage, 0f);

        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
