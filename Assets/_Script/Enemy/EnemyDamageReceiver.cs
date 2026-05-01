using UnityEngine;

// Enemy nhận damage từ Player
// Dùng RpcTarget.All → cả 2 máy đều chạy Receiver()
// → cả 2 máy tự giảm HP → không cần sync thêm
//
// MayA                        MayB
// Receiver() chạy             Receiver() chạy
// Giảm HP enemy               Giảm HP enemy
// HP bar update ✓             HP bar update ✓
// IsDead() → OnDead() ✓       IsDead() → OnDead() ✓

public class EnemyDamageReceiver : DamageReceiver
{
    protected override void OnDead()
    {
        // Không cần RPC vì tất cả máy đã chạy đến đây rồi
        gameObject.SetActive(false);
    }
}
