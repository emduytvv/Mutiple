using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossCtrl : DamageableCtrl
{
    public BossDamageReceiver DamageReceiver => _damageReceiver;
    [SerializeField] protected BossDamageReceiver _damageReceiver;

    public BossDamageSender DamageSender => _damageSender;
    [SerializeField] protected BossDamageSender _damageSender;

    public BossAnimation BossAnimation => _bossAnimation;
    [SerializeField] protected BossAnimation _bossAnimation;

    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    [SerializeField] protected Rigidbody2D _rigidbody2D;

    public BossDespawn BossDespawn => _bossDespawn;
    [SerializeField] protected BossDespawn _bossDespawn;

    public BossRotate BossRotate => _bossRotate;
    [SerializeField] protected BossRotate _bossRotate;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageReceiver();
        this.LoadDamageSender();
        this.LoadBossAnimation();
        this.LoadRigidbody2D();
        this.LoadBossDespawn();
        this.LoadBossRotate();
    }

    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = GetComponentInChildren<BossDamageReceiver>();
    }

    private void LoadDamageSender()
    {
        if (_damageSender != null) return;
        _damageSender = GetComponentInChildren<BossDamageSender>();
    }

    private void LoadBossAnimation()
    {
        if (_bossAnimation != null) return;
        _bossAnimation = GetComponentInChildren<BossAnimation>();
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void LoadBossDespawn()
    {
        if (_bossDespawn != null) return;
        _bossDespawn = GetComponentInChildren<BossDespawn>();
    }

    private void LoadBossRotate()
    {
        if (_bossRotate != null) return;
        _bossRotate = GetComponentInChildren<BossRotate>();
    }

    [PunRPC]
    public override void RpcReceive(float physDamage, float magDamage, float armorPen)
    {
        _damageReceiver.Receiver(physDamage, magDamage, armorPen);
    }
}
