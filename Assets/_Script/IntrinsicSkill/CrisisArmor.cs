using Photon.Pun;
using UnityEngine;

public class CrisisArmor : BaseIntrinsicSkill
{
    private bool _buffActive = false;
    [SerializeField] private float _threshold = 0.2f;
    [SerializeField] private float _attackMultiplier = 0.5f;
    protected void Update()
    {
        OnHPSmall();
    }
    private void OnHPSmall()
    {
        if (!_isActive) return;
        if (!_player.PhotonView.IsMine) return;

        if (CanImplement() && !_buffActive)
        {
            _buffActive = true;
            _player.PhotonView.RPC("RpcAddDefense", RpcTarget.All, _attackMultiplier);
        }
        else if (!CanImplement() && _buffActive)
        {
            _buffActive = false;
            _player.PhotonView.RPC("RpcAddDefense", RpcTarget.All, -_attackMultiplier);
        }
    }
    private bool CanImplement()
    {
        return _player.PlayerDamageReceiver.GetCurrrentHPPercent() < _threshold;
    }
}
