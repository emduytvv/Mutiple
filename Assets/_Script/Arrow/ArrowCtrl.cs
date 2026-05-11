using Photon.Pun;
using UnityEngine;

public class ArrowCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public ArrowDespawn ArrowDespawn => _arrowDespawn;
    [SerializeField] protected ArrowDespawn _arrowDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadArrowDespawn();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }

    private void LoadArrowDespawn()
    {
        if (_arrowDespawn != null) return;
        _arrowDespawn = GetComponentInChildren<ArrowDespawn>();
    }
}
