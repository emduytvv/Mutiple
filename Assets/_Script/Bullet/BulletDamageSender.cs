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

    public void SetDamage(float physDamage, float magDamage)
    {
        basePhysicalDamage = physDamage;
        baseMagicalDamage = magDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamageReceiver player = collision.GetComponent<PlayerDamageReceiver>();
        if (player == null) return;
        if (!_bulletCtrl.PhotonView.IsMine) return;
        PlayerCtrl playerCtrl = player.GetComponentInParent<PlayerCtrl>();
        playerCtrl.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage);
        _bulletCtrl.BulletDespawn.DespawnObject();
    }
}
