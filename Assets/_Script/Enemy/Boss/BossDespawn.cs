using Photon.Pun;
using UnityEngine;

public class BossDespawn : Despawn
{
    [SerializeField] protected BossCtrl _bossCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBossCtrl();
    }

    private void LoadBossCtrl()
    {
        if (_bossCtrl != null) return;
        _bossCtrl = GetComponentInParent<BossCtrl>();
    }

    protected override bool CanDespawn() => false;

    public override void DespawnObject()
    {
        if (!_bossCtrl.PhotonView.IsMine) return;
        PhotonNetwork.Destroy(_bossCtrl.PhotonView.gameObject);
    }
}
