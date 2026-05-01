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

public class ArrowDamageSender : DamageSender
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDamageReceiver enemy = collision.GetComponent<EnemyDamageReceiver>();
        if (enemy == null) return;
        if (!_photonView.IsMine) return;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, maxDamage);
    }
}
