using Photon.Pun;
using UnityEngine;

public class EnemyDespawn : Despawn
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private EnemyDamageReceiver _damageReceiver;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadDamageReceiver();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
    }

    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = transform.parent.GetComponentInChildren<EnemyDamageReceiver>();
    }

    protected override bool CanDespawn() => _damageReceiver.isDead;

    public override void DespawnObject()
    {
        if (!_photonView.IsMine) return;
        PhotonNetwork.Destroy(_photonView.gameObject);
    }
}
