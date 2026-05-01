using Photon.Pun;
using UnityEngine;

// Player đánh Enemy
// Gửi RpcTarget.All vì tất cả máy cần thấy HP enemy giảm
//
// MayA (IsMine=true)          MayB (IsMine=false)
// Send() chạy ──────────────► nhận RpcReceive
//                              EnemyDamageReceiver.Receiver() chạy
// nhận RpcReceive
// EnemyDamageReceiver.Receiver() chạy
// → cả 2 máy giảm HP enemy đồng thời ✓

public class PlayerDamageSender : DamageSender
{
    public void Send(EnemyDamageReceiver enemy, float damage)
    {
        if (!_photonView.IsMine) return;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, damage);
    }
}
