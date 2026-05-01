using Photon.Pun;
using UnityEngine;

// Player nhận damage từ Enemy
// RPC được gửi đến Owner → chỉ máy owner chạy Receiver()
// Sau đó sync HP sang máy kia để hiển thị HP bar
//
// MayA (MasterClient)         MayB (owner Player2)
//                             Receiver() chạy  ← nhận RPC từ EnemyDamageSender
//                             Giảm HP
//                             RpcSyncHP(Others)──────────────►
//                                                 MayA: currentHp = hp
//                                                 HP bar Player2 update ✓
//                             IsDead → OnDead()
//                             RpcOnDead(Others) ─────────────►
//                                                 MayA: RpcOnDead() chạy ✓

public class PlayerDamageReceiver : DamageReceiver
{
    public override void Receiver(float damage)
    {
        base.Receiver(damage);
        _photonView.RPC("RpcSyncHP", RpcTarget.Others, currentHp);
    }

    [PunRPC]
    private void RpcSyncHP(float hp)
    {
        currentHp = hp;
    }

    // Owner phát hiện dead → broadcast sang máy kia
    protected override void OnDead()
    {
        _photonView.RPC("RpcOnDead", RpcTarget.Others);
    }

    // Chạy trên máy kia khi nhận RpcOnDead — không gọi lại RPC nữa
    protected override void RpcOnDead()
    {
        isDead = true;
    }
}
