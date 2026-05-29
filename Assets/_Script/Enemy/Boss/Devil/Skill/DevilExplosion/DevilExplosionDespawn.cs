using Photon.Pun;
using UnityEngine;

public class DevilExplosionDespawn : DespawnByTime
{
    [SerializeField] private DevilExplosionCtrl _ctrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCtrl();
    }

    private void LoadCtrl()
    {
        if (_ctrl != null) return;
        _ctrl = GetComponentInParent<DevilExplosionCtrl>();
    }

    public override void DespawnObject()
    {
        if (!_ctrl.PhotonView.IsMine) return;
        PhotonNetwork.Destroy(_ctrl.PhotonView.gameObject);
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 2f;
    }

}

