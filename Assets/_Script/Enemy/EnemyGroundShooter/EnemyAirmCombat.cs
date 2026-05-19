using Photon.Pun;
using UnityEngine;

public class EnemyAirmCombat : EnemyCombat<EnemyShooterCtrl>
{
    [SerializeField] private Transform _pointShoot;

    [SerializeField] private float _detectionRadiusIn = 10f;
    [SerializeField] private float _detectionRadiusExit = 12f;
    [SerializeField] private float _prepareDuration = 0.7f;
    [SerializeField] private float _prepareTimer;
    [SerializeField] private float _coolDown = 5f;
    [SerializeField] private float _coolDownTimer;

    [SerializeField] private Transform _target;
    [SerializeField] private bool _isAirm;
    [SerializeField] private float _airmDuration = 5f;
    [SerializeField] private float _airmTimer;
    [SerializeField] private bool _isPreparing;
    [SerializeField] private bool _canShoot;

    [SerializeField] private LineRenderer _lineRenderer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPointShoot();
        this.LoadLineRenderer();
    }

    private void LoadLineRenderer()
    {
        if (this._lineRenderer != null) return;
        this._lineRenderer = GetComponent<LineRenderer>();
        Debug.Log(transform.name + ": Load LineRenderer", gameObject);
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
        if (!_isAirm) { HandleAirm(); return; }
        if (!_isPreparing) { StartPrepare(); return; }
        TickPrepare();
    }

    private void TickCooldown()
    {
        _coolDownTimer += Time.deltaTime;
        if (_coolDownTimer >= _coolDown) _canShoot = true;
    }

    private void HandleAirm()
    {
        TickAirm();
    }

    private void TickAirm()
    {
        DrawLineAirm();
        _airmTimer += Time.deltaTime;
        if (_airmTimer < _airmDuration) return;
        _lineRenderer.enabled = false;
        _isAirm = true;
    }

    private void DrawLineAirm()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _target.position + Vector3.up * 0.5f);
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
        _lineRenderer.enabled = false;
        _canShoot = false;
        _coolDownTimer = 4;
        _isAirm = false;
        _airmTimer = 0;
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
        PhotonNetwork.Instantiate(NameBullet.Bullet_Sniper.ToString(), _pointShoot.position, _pointShoot.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusIn);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusExit);
    }
}
