using Photon.Pun;
using UnityEngine;

public class BulletDespawn : DespawnByTime
{
    [SerializeField] private BulletCtrl _bulletCtrl;

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

    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 2.5f;
    }

    public override void DespawnObject()
    {
        if (!_bulletCtrl.PhotonView.IsMine) return;
        PhotonNetwork.Destroy(_bulletCtrl.PhotonView.gameObject);
    }
}
