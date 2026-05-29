using Photon.Pun;
using UnityEngine;

public class ArrowNormalDamageSender : ArrowDamageSender
{
    protected override void OnHitTarget(DamageableCtrl target, Collider2D collision)
    {
        _hasHit = true;
        target.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage, _armorPen);
        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
