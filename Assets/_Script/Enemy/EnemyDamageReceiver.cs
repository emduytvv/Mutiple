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
    public EnemyAnimation EnemyAnimation => _enemyAnimation;
    [SerializeField] protected EnemyAnimation _enemyAnimation;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyAnimation();
    }

    private void LoadEnemyAnimation()
    {
        if (this._enemyAnimation != null) return;
        this._enemyAnimation = GetComponentInParent<EnemyAnimation>();
        Debug.Log(transform.name + ": Load EnemyAnimation", gameObject);
    }

    protected override void OnDead() { }
    public override void Receiver(float damage)
    {
        if (isDead) return;
        _enemyAnimation.OnHurt();
        // FXSpawner.Instance.SpawnTextReduce("TextReduce", transform.position, damage.ToString());
        Reduce(damage);
    }
}
