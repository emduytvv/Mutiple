using Photon.Pun;
using UnityEngine;

public class PlayerDespawn : Despawn
{
    [SerializeField] private PlayerCtrl _playerCtrl;
    [SerializeField] private float _timeToDespawn = 40f;
    [SerializeField] private float _timer = 0f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }

    protected override bool CanDespawn()
    {
        if (!_playerCtrl.PlayerDamageReceiver.isDead) return false;
        _timer += Time.fixedDeltaTime;
        if (_timer < _timeToDespawn) return false;
        _timer = 0f;
        return true;
    }

    public override void DespawnObject()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        PhotonNetwork.Destroy(_playerCtrl.PhotonView.gameObject);
    }
}
