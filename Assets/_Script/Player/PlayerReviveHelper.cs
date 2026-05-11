using Photon.Pun;
using UnityEngine;

public class PlayerReviveHelper : SaiMonoBehaviour
{
    [SerializeField] private PlayerCtrl _playerCtrl;
    [SerializeField] private float _reviveTime = 5f;
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
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        PlayerCtrl other = collision.GetComponentInParent<PlayerCtrl>();
        if (other == null) return;
        if (!other.PlayerDamageReceiver.isDead) return;
        if (other.PhotonView == _playerCtrl.PhotonView) return;

        Help(other);
    }

    private void Help(PlayerCtrl other)
    {
        if (!CanHelp()) return;
        other.PhotonView.RPC("RpcRevive", RpcTarget.All);
    }

    private bool CanHelp()
    {
        if (!InputManager.Instance.GetMouseReviveHelper())
        {
            _timer = 0f;
            return false;
        }
        if (_timer < _reviveTime)
        {
            _timer += Time.fixedDeltaTime;
            return false;
        }
        _timer = 0f;
        return true;
    }
}
