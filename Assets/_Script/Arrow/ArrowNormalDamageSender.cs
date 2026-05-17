using Photon.Pun;
using UnityEngine;

public class ArrowNormalDamageSender : ArrowDamageSender
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasHit) return;
        EnemyCtrl enemy = collision.GetComponentInParent<EnemyCtrl>();
        if (enemy == null) return;

        if (!_arrowCtrl.PhotonView.IsMine) return;
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage, _armorPen);

        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
