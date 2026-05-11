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
    [SerializeField] private PlayerCtrl _playerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }

    public void Send(EnemyDamageReceiver enemy, float damage)
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, damage);
    }
}
