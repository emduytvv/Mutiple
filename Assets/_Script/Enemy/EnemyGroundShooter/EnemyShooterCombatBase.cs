using Photon.Pun;
using UnityEngine;

public abstract class EnemyShooterCombatBase : EnemyCombat<EnemyShooterCtrl>
{
    [SerializeField] protected Transform _pointShoot;
    [SerializeField] protected float _detectionRadiusIn = 10f;
    [SerializeField] protected float _detectionRadiusExit = 12f;
    [SerializeField] protected float _coolDownTimer;
    [SerializeField] protected Transform _target;
    [SerializeField] protected bool _isPreparing;
    [SerializeField] protected bool _canShoot;
    protected string _bulletName;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPointShoot();
    }

    private void LoadPointShoot()
    {
        if (_pointShoot != null) return;
        _pointShoot = transform.Find("PointShoot");
        Debug.Log(transform.name + ": Load PointShoot", gameObject);
    }

    private void Update()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        UpdateTarget();
        HandleCombat();
    }

    private void UpdateTarget()
    {
        if (_target != null)
        {
            if (Vector2.Distance(transform.position, _target.position) > _detectionRadiusExit)
            {
                ResetCombat();
                _target = null;
            }
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _detectionRadiusIn, _playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (!hit.GetComponent<PlayerDamageReceiver>()) continue;
            _target = hit.transform.parent;
            break;
        }
    }

    protected virtual void HandleCombat()
    {
        if (_target == null) return;
        if (!_canShoot) { TickCooldown(); return; }
        if (!IsReadyToFire()) return;
        if (!_isPreparing) { StartPrepare(); return; }
    }

    protected virtual bool IsReadyToFire() => true;

    private void TickCooldown()
    {
        _coolDownTimer += Time.deltaTime;
        if (_coolDownTimer >= _coolDown) _canShoot = true;
    }

    private void StartPrepare()
    {
        _isPreparing = true;
        _enemyCtrl.ShooterMovement.SetMoving(false);
        _enemyCtrl.EnemyAnimation.SetAttackTrigger();
    }

    protected virtual void ResetCombat()
    {
        _canShoot = false;
        _coolDownTimer = 4;
        if (_isPreparing) ResumeMovement();
    }

    private void ResumeMovement()
    {
        _isPreparing = false;
        _enemyCtrl.ShooterMovement.SetMoving(true);
    }

    public override void Send()
    {
        if (_target == null) return;
        Vector2 direction = _target.position - _pointShoot.position;
        _pointShoot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        SpawnBullet(_pointShoot.rotation);
        ResetCombat();
    }

    protected void SpawnBullet(Quaternion rotation)
    {
        BulletSpawner.Instance.Spawn(_bulletName, _pointShoot.position, rotation,
            _enemyCtrl.EnemyDamageSender.PhysicalDamage, _enemyCtrl.EnemyDamageSender.MagicalDamage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusIn);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusExit);
    }
}
