using Photon.Pun;
using UnityEngine;

public class EnemyCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    [SerializeField] protected EnemyDamageReceiver _damageReceiver;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadDamageReceiver();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }

    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = GetComponentInChildren<EnemyDamageReceiver>();
    }

[PunRPC]
    public void RpcReceive(float damage)
    {
        _damageReceiver.Receiver(damage);
    }
}
