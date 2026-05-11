using Photon.Pun;
using UnityEngine;

public class EnemyShooterCombat : EnemyCombat
{
    [SerializeField] private Transform _pointShoot;
    [SerializeField] private EnemyShooterMovement _movement;

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
        this.LoadMovement();
    }

    private void LoadPointShoot()
    {
        if (this._pointShoot != null) return;
        this._pointShoot = transform.Find("PointShoot");
        Debug.Log(transform.name + ": Load PointShoot", gameObject);
    }

    private void LoadMovement()
    {
        if (this._movement != null) return;
        this._movement = transform.parent.GetComponentInChildren<EnemyShooterMovement>();
        Debug.Log(transform.name + ": Load Movement", gameObject);
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
                ResetCombat();
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

    private void ResetCombat()
    {
        _target = null;
        _canShoot = false;
        _coolDownTimer = 0;
        if (_isPreparing) ResumeMovement();
    }

    private void HandleCombat()
    {
        if (_target == null) return;

        if (_canShoot)
        {
            _coolDownTimer += Time.deltaTime;
            if (_coolDownTimer >= _coolDown) _canShoot = false;
            return;
        }

        if (!_isPreparing)
        {
            StartPrepare();
            return;
        }

        _prepareTimer += Time.deltaTime;
        if (_prepareTimer < _prepareDuration) return;

        Shoot();
        ResumeMovement();
        _canShoot = true;
        _coolDownTimer = 0;
    }

    private void StartPrepare()
    {
        _isPreparing = true;
        _prepareTimer = 0;
        _movement.SetMoving(false);
        _enemyCtrl.EnemyAnimation.SetAttackTrigger();
    }

    private void ResumeMovement()
    {
        _isPreparing = false;
        _movement.SetMoving(true);
    }

    private void Shoot()
    {
        Vector2 direction = _target.position - _pointShoot.position;
        _pointShoot.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        PhotonNetwork.Instantiate("Bullet_WandererMagican", _pointShoot.position, _pointShoot.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusIn);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadiusExit);
    }
}
