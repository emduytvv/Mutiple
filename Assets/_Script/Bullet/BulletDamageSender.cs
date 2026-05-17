using Photon.Pun;
using UnityEngine;

public class BulletDamageSender : DamageSender
{
    [SerializeField] protected BulletCtrl _bulletCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletCtrl();
    }

    private void LoadBulletCtrl()
    {
        if (_bulletCtrl != null) return;
        _bulletCtrl = GetComponentInParent<BulletCtrl>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCtrl player = collision.GetComponentInParent<PlayerCtrl>();

        if (player == null) return;
        if (!_bulletCtrl.PhotonView.IsMine) return;

        player.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage);
        _bulletCtrl.BulletDespawn.DespawnObject();
    }
}
