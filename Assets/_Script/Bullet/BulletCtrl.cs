using Photon.Pun;
using UnityEngine;

public class BulletCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public BulletDespawn BulletDespawn => _bulletDespawn;
    [SerializeField] protected BulletDespawn _bulletDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadBulletDespawn();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }

    private void LoadBulletDespawn()
    {
        if (_bulletDespawn != null) return;
        _bulletDespawn = GetComponentInChildren<BulletDespawn>();
    }
}
