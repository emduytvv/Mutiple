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

public class DamageSender : SaiMonoBehaviour
{
    protected float maxDamage = 1f;
    protected float baseDamage = 1f;
    [SerializeField] protected PhotonView _photonView;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
    }
}
