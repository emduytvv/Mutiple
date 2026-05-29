using Photon.Pun;
using UnityEngine;

public class BulletCtrl : SaiMonoBehaviour, IPunInstantiateMagicCallback
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public BulletDespawn BulletDespawn => _bulletDespawn;
    [SerializeField] protected BulletDespawn _bulletDespawn;
    public BulletDamageSender BulletDamageSender => _bulletDamageSender;
    [SerializeField] protected BulletDamageSender _bulletDamageSender;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadBulletDespawn();
        this.LoadBulletDamageSender();
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

    private void LoadBulletDamageSender()
    {
        if (_bulletDamageSender != null) return;
        _bulletDamageSender = GetComponentInChildren<BulletDamageSender>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;
        if (data == null || data.Length < 2) return;
        _bulletDamageSender.SetDamage((float)data[0], (float)data[1]);
    }
}
