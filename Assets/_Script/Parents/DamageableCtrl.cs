using Photon.Pun;
using UnityEngine;

public abstract class DamageableCtrl : SaiMonoBehaviour
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

    public virtual void RpcReceive(float physDamage, float magDamage, float armorPen) { }
}
