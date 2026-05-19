using Photon.Pun;
using UnityEngine;

public class ArrowNormalDamageSender : ArrowDamageSender
{
    protected override void OnHitEnemy(EnemyCtrl enemy, Collider2D collision)
    {
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage, _armorPen);
        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
