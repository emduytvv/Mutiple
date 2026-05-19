using UnityEngine;

public class BatCombat : EnemyCombat<EnemyBatCtrl>
{
    [SerializeField] private Transform _pointAttack;
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
    }

    private void LoadPointAttack()
    {
        if (_pointAttack != null) return;
        _pointAttack = transform.Find("PointAttack");
        Debug.Log(transform.name + ": Load PointAttack", gameObject);
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
        _target = _enemyCtrl.FlyMovement.Target;
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
            _enemyCtrl.EnemyDamageSender.Send(player);
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
