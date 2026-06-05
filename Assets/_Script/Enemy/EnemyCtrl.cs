using System;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class EnemyCtrl : DamageableCtrl
{
    public EnemyDamageReceiver DamageReceiver => _damageReceiver;
    [SerializeField] protected EnemyDamageReceiver _damageReceiver;
    public EnemyDamageSender EnemyDamageSender => _enemyDamageSender;
    [SerializeField] protected EnemyDamageSender _enemyDamageSender;
    public EnemyAnimation EnemyAnimation => _enemyAnimation;
    [SerializeField] protected EnemyAnimation _enemyAnimation;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    public EnemyDespawn EnemyDespawn => _enemyDespawn;
    [SerializeField] protected EnemyDespawn _enemyDespawn;
    [SerializeField] protected EnemyStatsSO _enemyStatsSO;
    public EnemyStatsSO EnemyStatsSO => _enemyStatsSO;
    public EnemyItemDropper EnemyItemDropper => _enemyItemDropper;
    [SerializeField] protected EnemyItemDropper _enemyItemDropper;
    public EnemyCombatBase EnemyCombat => _enemyCombat;
    [SerializeField] protected EnemyCombatBase _enemyCombat;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageReceiver();
        this.LoadEnemyAnimation();
        this.LoadRigidbody2D();
        this.LoadEnemyDespawn();
        this.LoadEnemyDamageSender();
        this.LoadEnemyStatsSO();
        this.LoadEnemyItemDropper();
        LoadEneeyCombat();
    }

    private void LoadEnemyStatsSO()
    {
        if (_enemyStatsSO != null) return;
        string path = "EnemyStats/" + transform.name;
        _enemyStatsSO = Resources.Load<EnemyStatsSO>(path);
    }


    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = GetComponentInChildren<EnemyDamageReceiver>();
    }

    private void LoadEnemyDamageSender()
    {
        if (_enemyDamageSender != null) return;
        _enemyDamageSender = GetComponentInChildren<EnemyDamageSender>();
    }

    private void LoadEnemyAnimation()
    {
        if (_enemyAnimation != null) return;
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void LoadEnemyDespawn()
    {
        if (_enemyDespawn != null) return;
        _enemyDespawn = GetComponentInChildren<EnemyDespawn>();
    }

    private void LoadEnemyItemDropper()
    {
        if (_enemyItemDropper != null) return;
        _enemyItemDropper = GetComponentInChildren<EnemyItemDropper>();
    }
    private void LoadEneeyCombat()
    {
        if (_enemyCombat != null) return;
        _enemyCombat = GetComponentInChildren<EnemyCombatBase>();
    }
    public void ApplyStatMultiplier(float multiplier)
    {
        if (Mathf.Approximately(multiplier, 1f)) return;
        _damageReceiver.ApplyMultiplier(multiplier);
        _enemyDamageSender.ApplyMultiplier(multiplier);
        // GetComponentInChildren<Movement>()?.ApplyMultiplier(multiplier);
    }

    [PunRPC]
    public override void RpcReceive(float physDamage, float magDamage, float armorPen)
    {
        _damageReceiver.Receiver(physDamage, magDamage, armorPen);
    }

    [PunRPC]
    public void RpcSetAttackTrigger() => _enemyAnimation.PlayAttackAnim();

    [PunRPC]
    public void RpcForceKill()
    {
        _damageReceiver.SetIsDead(true);
    }

    [PunRPC]
    public void RpcSpawnItemDrop(string prefabName, int amount, Vector3 pos)
    {
        _enemyItemDropper.SpawnItemDrop(prefabName, amount, pos);
    }

    [PunRPC]
    public void RpcSpawnHPBar()
    {
        Transform obj = HPBarEnemySpawner.Instance.SpawnHPBar(transform.position, transform.rotation);
        obj.GetComponentInChildren<EnemyHPBar>().SetTarget(transform);
    }
}
