using Photon.Pun;
using UnityEngine;

public class EnemyShooterCombat : EnemyCombat<EnemyShooterCtrl>
{
    [SerializeField] private Transform _pointShoot;

    [SerializeField] private float _detectionRadiusIn = 10f;
    [SerializeField] private float _detectionRadiusExit = 12f;
    [SerializeField] private float _prepareDuration = 0.7f;
    [SerializeField] private float _prepareTimer;
    [SerializeField] private float _coolDown = 5f;
    [SerializeField] private float _coolDownTimer;

    [SerializeField] private Transform _target;
    [SerializeField] private bool _isPreparing;
    [SerializeField] private bool _canShoot;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPointShoot();
    }

    private void LoadPointShoot()
    {
        if (this._pointShoot != null) return;
        this._pointShoot = transform.Find("PointShoot");
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

    private void HandleCombat()
    {
        if (_target == null) return;
        if (!_canShoot) { TickCooldown(); return; }
        if (!_isPreparing) { StartPrepare(); return; }
        TickPrepare();
    }

    private void TickCooldown()
    {
        _coolDownTimer += Time.deltaTime;
        if (_coolDownTimer >= _coolDown) _canShoot = true;
    }

    private void StartPrepare()
    {
        _isPreparing = true;
        _prepareTimer = 0;
        _enemyCtrl.ShooterMovement.SetMoving(false);
        _enemyCtrl.EnemyAnimation.SetAttackTrigger();
    }

    private void TickPrepare()
    {
        _prepareTimer += Time.deltaTime;
        if (_prepareTimer < _prepareDuration) return;
        Shoot();
        ResetCombat();
    }

    private void ResetCombat()
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

    private void Shoot()
    {
        Vector2 direction = _target.position - _pointShoot.position;
        _pointShoot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        PhotonNetwork.Instantiate(NameBullet.Bullet_WandererMagican.ToString(), _pointShoot.position, _pointShoot.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusIn);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusExit);
    }
}
