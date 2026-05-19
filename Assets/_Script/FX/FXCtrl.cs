using Photon.Pun;
using UnityEngine;

public class FXCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }
}
