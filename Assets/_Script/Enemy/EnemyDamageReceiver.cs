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
    [SerializeField] protected EnemyCtrl _enemyCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyAnimation();
    }

    private void LoadEnemyAnimation()
    {
        if (this._enemyCtrl != null) return;
        this._enemyCtrl = GetComponentInParent<EnemyCtrl>();
        Debug.Log(transform.name + ": Load EnemyAnimation", gameObject);
    }

    protected override void OnDead()
    {
        GameEvents.OnEnemyDied?.Invoke();
    }
    public override void Receiver(float damage)
    {
        if (isDead) return;
        _enemyCtrl.EnemyAnimation.OnHurt();
        Reduce(damage);
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        baseMaxHP = 2f;
    }

}
