using Photon.Pun;
using UnityEngine;

public class ArrowRicochetDamageSender : ArrowDamageSender
{
    [SerializeField] protected int _ricochetCount = 1;

    protected override void OnHitEnemy(EnemyCtrl enemy, Collider2D collision)
    {
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage, 0f);
        Ricochet(collision);
        _arrowCtrl.ArrowDespawn.DespawnObject();
    }

    private void Ricochet(Collider2D collision)
    {
        Vector3 center = collision.transform.position;
        for (int i = 0; i < _ricochetCount; i++)
        {
            Quaternion quaternion = Quaternion.Euler(0, 0, Random.Range(0, 360));
            PhotonNetwork.Instantiate("ArrowNormal", center, quaternion);
        }
    }
}
