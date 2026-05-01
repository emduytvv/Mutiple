using Photon.Pun;
using UnityEngine;

// Enemy đánh Player
// Chỉ MasterClient mới gửi (vì enemy do MasterClient điều khiển)
// Gửi đến playerView.Owner vì chỉ owner mới xử lý HP player
// Owner xử lý xong → tự sync HP sang máy kia
//
// MayA (MasterClient)         MayB (owner của Player2)
// Send() chạy
// RPC → Player2.Owner ──────► RpcReceive chạy
//                              Receiver() chạy
//                              Giảm HP
//                              RpcSyncHP(Others) ──────────────►
//                                                  MayA nhận HP mới
//                                                  cập nhật HP bar ✓

public class EnemyDamageSender : DamageSender
{
    public void Send(PlayerDamageReceiver player, float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        player.PhotonView.RPC("RpcReceive", player.PhotonView.Owner, damage);
    }
}
