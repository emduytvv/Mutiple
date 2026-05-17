using UnityEngine;

public class EnemyDamageReceiver : DamageReceiver
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    public override void Receiver(float physDamage, float magDamage, float armorPen = 0f)
    {
        if (isDead) return;
        _enemyCtrl.EnemyAnimation.OnHurt();
        base.Receiver(physDamage, magDamage, armorPen);
    }

    protected override void OnDead()
    {
        GameEvents.OnEnemyDied?.Invoke();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        baseMaxHP = 100f;
        physicalDefenseTotal = 0f;
        magicalDefenseTotal = 0f;
    }
}
