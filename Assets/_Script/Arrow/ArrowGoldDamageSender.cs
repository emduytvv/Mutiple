using Photon.Pun;
using UnityEngine;

public class ArrowGoldDamageSender : ArrowDamageSender
{
    protected float _rateDamage = 1.5f;
    protected override void OnHitEnemy(EnemyCtrl enemy, Collider2D collision)
    {
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, _rateDamage * basePhysicalDamage, _rateDamage * baseMagicalDamage, 0f);
        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
}
