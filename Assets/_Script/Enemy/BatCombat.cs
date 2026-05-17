using UnityEngine;

public class BatCombat : EnemyCombat
{
    [SerializeField] private Transform _pointAttack;
    public EnemyMovementToTarget EnemyMovement => _enemyMovement;
    [SerializeField] protected EnemyMovementToTarget _enemyMovement;
    public EnemyDamageSender EnemyDamageSender => _enemyDamageSender;
    [SerializeField] protected EnemyDamageSender _enemyDamageSender;
    [SerializeField] protected float _distanceToAttack = 1.5f;
    [SerializeField] protected float _rangeAttack = 0.5f;
    [SerializeField] protected float _coolDown = 1f;
    [SerializeField] private float _attackTimer;
    [SerializeField] protected float _currentDistance;
    [SerializeField] protected Transform _target;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPointAttack();
        this.LoadEnemyMovement();
        this.LoadEnemyDamageSender();
    }

    private void LoadPointAttack()
    {
        if (_pointAttack != null) return;
        _pointAttack = transform.Find("PointAttack");
        Debug.Log(transform.name + ": Load PointAttack", gameObject);
    }

    private void LoadEnemyMovement()
    {
        if (this._enemyMovement != null) return;
        this._enemyMovement = transform.parent.GetComponentInChildren<EnemyMovementToTarget>();
        Debug.Log(transform.name + ": Load EnemyMovement", gameObject);
    }

    private void LoadEnemyDamageSender()
    {
        if (this._enemyDamageSender != null) return;
        this._enemyDamageSender = transform.parent.GetComponentInChildren<EnemyDamageSender>();
        Debug.Log(transform.name + ": Load EnemyDamageSender", gameObject);
    }

    private void Update()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        GetTarget();
        if (_target == null) return;
        GetDistance();
        Attack();
    }

    private void GetDistance()
    {
        _currentDistance = Vector2.Distance(transform.position, _target.position);
    }

    private void GetTarget()
    {
        _target = EnemyMovement.Target;
    }

    private void Attack()
    {
        if (!CanAttack()) return;
        _enemyCtrl.EnemyAnimation.SetAttackTrigger();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_pointAttack.position, _rangeAttack, _playerLayer);
        if (colliders.Length == 0) return;
        foreach (Collider2D collider in colliders)
        {
            if (!collider.GetComponent<PlayerDamageReceiver>()) continue;
            PlayerCtrl player = collider.GetComponentInParent<PlayerCtrl>();
            EnemyDamageSender.Send(player);
        }
    }

    private void OnDrawGizmos()
    {
        if (_pointAttack == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_pointAttack.position, _rangeAttack);
    }

    private bool CanAttack()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < _coolDown) return false;
        if (_currentDistance < _distanceToAttack)
        {
            _attackTimer = 0;
            return true;
        }
        return false;
    }
}
