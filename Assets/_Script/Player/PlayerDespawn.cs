using Photon.Pun;
using UnityEngine;

public class PlayerDespawn : Despawn
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private PlayerDamageReceiver _damageReceiver;
    [SerializeField] private float _timeToDespawn = 40f;
    [SerializeField] private float _timer = 0f;


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
        _damageReceiver = transform.parent.GetComponentInChildren<PlayerDamageReceiver>();
    }

    protected override bool CanDespawn()
    {
        if (!_damageReceiver.isDead) return false;
        _timer += Time.fixedDeltaTime;
        if (_timer < _timeToDespawn) return false;
        _timer = 0f;
        return true;
    }

    public override void DespawnObject()
    {
        if (!_photonView.IsMine) return;
        PhotonNetwork.Destroy(_photonView.gameObject);
    }
}
