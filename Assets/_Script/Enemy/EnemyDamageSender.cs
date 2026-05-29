using Photon.Pun;
using UnityEngine;

public class EnemyDamageSender : DamageSender
{
    [SerializeField] private EnemyCtrl _enemyCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
    }

    public void ApplyMultiplier(float multiplier)
    {
        basePhysicalDamage *= multiplier;
        baseMagicalDamage  *= multiplier;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        if (_enemyCtrl?.EnemyStatsSO == null) return;
        basePhysicalDamage = _enemyCtrl.EnemyStatsSO._physicalAttack;
        baseMagicalDamage = _enemyCtrl.EnemyStatsSO._magicalAttack;
    }

    public void Send(PlayerCtrl player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        player.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage);
    }
}
