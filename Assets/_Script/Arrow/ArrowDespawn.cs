using Photon.Pun;
using UnityEngine;

public class ArrowDespawn : DespawnByTime
{
    [SerializeField] private PhotonView _photonView;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 3f;
    }

    public override void DespawnObject()
    {
        if (!_photonView.IsMine) return;
        PhotonNetwork.Destroy(_photonView.gameObject);
    }
}
