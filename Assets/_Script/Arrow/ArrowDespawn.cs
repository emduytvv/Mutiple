using Photon.Pun;
using UnityEngine;

public class ArrowDespawn : DespawnByTime
{
    [SerializeField] private ArrowCtrl _arrowCtrl;

    private bool _isDespawning;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadArrowCtrl();
    }

    private void LoadArrowCtrl()
    {
        if (_arrowCtrl != null) return;
        _arrowCtrl = GetComponentInParent<ArrowCtrl>();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 3f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _isDespawning = false;
    }

    public override void DespawnObject()
    {
        if (_isDespawning) return;
        if (!_arrowCtrl.PhotonView.IsMine) return;
        _isDespawning = true;
        PhotonNetwork.Destroy(_arrowCtrl.PhotonView.gameObject);
    }
}
