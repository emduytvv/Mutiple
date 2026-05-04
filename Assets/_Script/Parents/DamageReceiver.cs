using Photon.Pun;
using UnityEngine;

public abstract class DamageReceiver : SaiMonoBehaviour
{
    [Header("DamageReceiver")]

    [SerializeField] protected PhotonView _photonView;
    public PhotonView PhotonView => _photonView;
    [SerializeField] public float baseMaxHP = 10f;
    [SerializeField] public float percentHP = 0;
    [SerializeField] public float maxHP = 2f;
    [SerializeField] public float currentHp;
    public float CurrentHp => currentHp;
    public bool isDead = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
    }

    protected virtual void OnEnable()
    {
        maxHP = baseMaxHP;
        currentHp = maxHP;
        isDead = false;
    }
    public virtual void Receiver(float damage)
    {
        if (isDead) return;
        // FXSpawner.Instance.SpawnTextReduce("TextReduce", transform.position, damage.ToString());
        Reduce(damage);
    }

    public virtual bool IsDead()
    {
        if (isDead) return true;
        if (currentHp <= 0f)
        {
            isDead = true;
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

    public virtual void AddPercentHP(float amount)
    {
        percentHP += amount;
        SetTotalMaxHP();
    }

    private void SetTotalMaxHP()
    {
        maxHP = baseMaxHP * (percentHP / 100 + 1);
    }

    public virtual void Buff(float buff)
    {
        if (currentHp == maxHP) return;
        currentHp += buff;
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
    protected abstract void OnDead();
}
