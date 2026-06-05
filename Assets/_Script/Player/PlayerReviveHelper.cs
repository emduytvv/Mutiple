using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerReviveHelper : SaiMonoBehaviour
{
    [SerializeField] private PlayerCtrl _playerCtrl;
    [SerializeField] private float _reviveTime = 5f;
    [SerializeField] private float _reviveRadius = 4f;
    [SerializeField] private float _timer = 0f;
    public float Timer => _timer;
    public float ReviveTime => _reviveTime;
    [SerializeField] public bool IsNearDeadPlayer = false;
    protected PlayerCtrl _playerOther;
    public PlayerCtrl PlayerOther => _playerOther;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }

    private void Update()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        foreach (PlayerCtrl other in PlayerCtrl.AllPlayers)
        {
            if (other.PhotonView == _playerCtrl.PhotonView) continue;
            if (!other.PlayerDamageReceiver.isDead) continue;
            if (Vector2.Distance(transform.position, other.transform.position) > _reviveRadius) continue;
            _playerOther = other;
            IsNearDeadPlayer = true;
            Help(other);
            return;
        }
        IsNearDeadPlayer = false;
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
