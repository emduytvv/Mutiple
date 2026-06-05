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

    private bool _isDespawning;

    protected virtual void OnEnable()
    {
        _isDespawning = false;
    }

    protected override bool CanDespawn() => false;

    public override void DespawnObject()
    {
        if (_isDespawning) return;
        if (!_bossCtrl.PhotonView.IsMine) return;
        _isDespawning = true;
        PhotonNetwork.Destroy(_bossCtrl.PhotonView.gameObject);
    }
}
