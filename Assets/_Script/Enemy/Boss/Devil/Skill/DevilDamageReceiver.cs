using UnityEngine;

public class DevilDamageReceiver : BossDamageReceiver
{
    private DevilCtrl DevilCtrl => _bossCtrl as DevilCtrl;

    [Header("Physical HP")]
    [SerializeField] private float _basePhysMaxHP = 2000f;
    [SerializeField] private float _physMaxHP;
    [SerializeField] private float _physCurrentHP;
    public float PhysCurrentHP => _physCurrentHP;
    public float PhysMaxHP => _physMaxHP;
    public bool IsPhysDead => _physCurrentHP <= 0f;

    [Header("Magical HP")]
    [SerializeField] private float _baseMagMaxHP = 2000f;
    [SerializeField] private float _magMaxHP;
    [SerializeField] private float _magCurrentHP;
    public float MagCurrentHP => _magCurrentHP;
    public float MagMaxHP => _magMaxHP;
    public bool IsMagDead => _magCurrentHP <= 0f;

    // Phase 2 khi tổng HP còn dưới 50%
    public bool IsPhase2 => (_physCurrentHP + _magCurrentHP) < (_physMaxHP + _magMaxHP) * 0.5f;
    private bool _phase2Triggered;

    protected override void OnEnable()
    {
        base.OnEnable(); // reset _isDead = false
        _phase2Triggered = false;
        _physMaxHP = _basePhysMaxHP;
        _magMaxHP = _baseMagMaxHP;
        _physCurrentHP = _physMaxHP;
        _magCurrentHP = _magMaxHP;
    }

    public override void Receiver(float physDamage, float magDamage, float armorPen = 0f)
    {
        if (_isDead) return;

        if (physDamage > 0f)
        {
            float effective = Mathf.Max(0f, physDamage - physicalDefenseTotal * (1f - armorPen));
            _physCurrentHP = Mathf.Max(0f, _physCurrentHP - effective);
            TextSpawner.Instance.SpawnText(transform.position + Vector3.up, effective, 0f);
        }

        if (magDamage > 0f)
        {
            float effective = Mathf.Max(0f, magDamage - magicalDefenseTotal * (1f - armorPen));
            _magCurrentHP = Mathf.Max(0f, _magCurrentHP - effective);
            TextSpawner.Instance.SpawnText(transform.position + Vector3.up, 0f, effective);
        }

        OnHurt();
        IsDead();

        if (!_phase2Triggered && IsPhase2)
        {
            _phase2Triggered = true;
            DevilCtrl.DevilCombat.SetPhase2();
        }
    }

    // Boss chết khi CẢ 2 thanh đều về 0
    public override bool IsDead()
    {
        if (_isDead) return true;
        if (_physCurrentHP <= 0f && _magCurrentHP <= 0f)
        {
            _isDead = true;
            OnDead();
            return true;
        }
        return false;
    }

    protected virtual void OnHurt()
    {
        // DevilCtrl.BossAnimation.OnHurt();
    }

    protected override void OnDead()
    {

        base.OnDead();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        _basePhysMaxHP = 15000;
        _baseMagMaxHP = 15000;
    }
}
