using System;
using UnityEngine;

public class EnemyDamageReceiver : DamageReceiver
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;
    [SerializeField] private Collider2D _hitbox;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
        this.LoadHitbox();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    private void LoadHitbox()
    {
        if (_hitbox != null) return;
        _hitbox = GetComponentInParent<Collider2D>();
    }

    public void ApplyMultiplier(float multiplier)
    {
        baseMaxHP *= multiplier;
        physicalDefenseTotal *= multiplier;
        magicalDefenseTotal *= multiplier;
        SetTotalMaxHP();
        currentHp = maxHP;
    }

    protected override void OnEnable()
    {
        // _enemyCtrl.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _hitbox.enabled = true;
        LoadStatsFromSO();
        base.OnEnable();
    }

    private void LoadStatsFromSO()
    {
        if (_enemyCtrl?.EnemyStatsSO == null) return;
        baseMaxHP = _enemyCtrl.EnemyStatsSO._baseMaxHP;
        physicalDefenseTotal = _enemyCtrl.EnemyStatsSO._physicalDefense;
        magicalDefenseTotal = _enemyCtrl.EnemyStatsSO._magicalDefense;
    }

    public override void Receiver(float physDamage, float magDamage, float armorPen = 0f)
    {
        base.Receiver(physDamage, magDamage, armorPen);
        if (_isDead) return;
        this.OnHurt();
    }
    protected virtual void OnHurt()
    {
        _enemyCtrl.EnemyAnimation.SetHurtTrigger();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.EnemyHitSFX);
    }
    protected override void OnDead()
    {
        _hitbox.enabled = false;
        // _enemyCtrl.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        GameEvents.OnEnemyDied?.Invoke();
        _enemyCtrl.EnemyItemDropper?.OnEnemyDead();
        _enemyCtrl.EnemyAnimation.SetDieTrigger();
    }
}
