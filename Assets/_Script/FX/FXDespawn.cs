using System;
using Photon.Pun;

public class FXDespawn : DespawnByTime
{
    protected FXCtrl _fxCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadArrowCtrl();
    }

    private void LoadArrowCtrl()
    {
        if (_fxCtrl != null) return;
        _fxCtrl = GetComponentInParent<FXCtrl>();
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 1f;
    }
    public override void DespawnObject()
    {
        if (!_fxCtrl.PhotonView.IsMine) return;
        PhotonNetwork.Destroy(_fxCtrl.PhotonView.gameObject);
    }

}
