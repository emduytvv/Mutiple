using Photon.Pun;
using UnityEngine;

public class DashHeal : BaseIntrinsicSkill
{
    [SerializeField] private float _dashHealPercent = 0.03f;
    protected void OnEnable()
    {
        GameEvents.OnPlayerDashEnded += OnDashEnded;
    }
    protected void OnDisable()
    {
        GameEvents.OnPlayerDashEnded -= OnDashEnded;
    }
    private void OnDashEnded()
    {
        if (!_isActive) return;
        if (!_player.PhotonView.IsMine) return;
        _player.PhotonView.RPC("RpcBuffPercentHP", RpcTarget.All, _dashHealPercent);
    }
}
