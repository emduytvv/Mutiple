using Photon.Pun;
using UnityEngine;

public class BuffHP : BaseIntrinsicSkill
{
    [SerializeField] protected float _hpBonus = 100f;
    private void Update()
    {
        if (!_isActive) return;
        Apply();
    }
    protected virtual void Apply()
    {
        _player.PhotonView.RPC("RpcAddMaxHP", RpcTarget.All, _hpBonus);
        _isActive = false;
    }
}
