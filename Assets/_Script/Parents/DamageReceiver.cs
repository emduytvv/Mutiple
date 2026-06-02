using System;
using Photon.Pun;
using UnityEngine;

public abstract class DamageReceiver : SaiMonoBehaviour
{
    [Header("DamageReceiver")]
    [SerializeField] protected PhotonView _photonView;
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected float baseMaxHP = 10f;
    [SerializeField] protected float percentHPBonus = 0;
    [SerializeField] public float maxHP = 2f;
    [SerializeField] protected float currentHp;
    public float CurrentHp => currentHp;
    public bool isDead => _isDead;
    protected bool _isDead = false;

    [Header("Defense")]
    [SerializeField] protected float physicalDefenseTotal = 0f;
    [SerializeField] protected float magicalDefenseTotal = 0f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
    }

    protected virtual void OnEnable()
    {
        SetTotalMaxHP();
        currentHp = maxHP;
        _isDead = false;
    }

    public virtual void Receiver(float physDamage, float magDamage, float armorPen = 0f)
    {
        if (_isDead) return;
        float effectivePhys = CalculateDamagePhys(physDamage, armorPen);
        float effectiveMag = CalculateDamageMagic(magDamage, armorPen);
        SpawnTextDamage(effectivePhys, effectiveMag);
        Reduce(effectivePhys + effectiveMag);
    }

    private float CalculateDamageMagic(float magDamage, float armorPen)
    {
        float effectiveMagDef = magicalDefenseTotal * (1f - armorPen);
        float effectiveMag = Mathf.Max(0f, magDamage - effectiveMagDef);
        return effectiveMag;
    }


    private float CalculateDamagePhys(float physDamage, float armorPen)
    {
        float effectivePhysDef = physicalDefenseTotal * (1f - armorPen);
        float effectivePhys = Mathf.Max(0f, physDamage - effectivePhysDef);
        return effectivePhys;
    }


    private void SpawnTextDamage(float effectivePhys, float effectiveMag)
    {
        TextSpawner.Instance.SpawnText(transform.position + Vector3.up, effectivePhys, effectiveMag);
    }
    public virtual bool IsDead()
    {
        if (_isDead) return true;
        if (currentHp <= 0f)
        {
            _isDead = true;
            OnDead();
            return true;
        }
        return false;
    }

    public virtual void AddMaxHP(float amount)
    {
        baseMaxHP += amount;
        SetTotalMaxHP();
    }

    public virtual void AddPercentHPBonus(float amount)
    {
        percentHPBonus += amount;
        SetTotalMaxHP();
    }

    protected virtual void SetTotalMaxHP()
    {
        maxHP = baseMaxHP * (percentHPBonus + 1);
    }

    public virtual void Buff(float buff)
    {
        if (currentHp == maxHP) return;
        currentHp += buff;
        CheckHp();
    }
    public virtual void BuffPercentHP(float buff)
    {
        if (currentHp == maxHP) return;
        currentHp += maxHP * buff;
        CheckHp();
    }

    protected virtual void Reduce(float damage)
    {
        currentHp -= damage;
        CheckHp();
        IsDead();
    }

    protected virtual void CheckHp()
    {
        if (currentHp > maxHP) currentHp = maxHP;
        if (currentHp < 0f) currentHp = 0f;
    }
    public void SetIsDead(bool isDead)
    {
        currentHp = 0f;
        _isDead = isDead;
        OnDead();
    }
    protected abstract void OnDead();
}
