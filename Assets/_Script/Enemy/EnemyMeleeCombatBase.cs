using UnityEngine;

public abstract class EnemyMeleeCombatBase<TCtrl> : EnemyCombat<TCtrl> where TCtrl : EnemyCtrl
{
    [SerializeField] protected Transform _pointAttack;
    [SerializeField] protected float _distanceToAttack = 1.5f;
    [SerializeField] protected float _rangeAttack = 0.5f;
    [SerializeField] protected float _attackTimer;
    [SerializeField] protected float _currentDistance;
    [SerializeField] protected Transform _target;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPointAttack();
    }

    private void LoadPointAttack()
    {
        if (_pointAttack != null) return;
        _pointAttack = transform.Find("PointAttack");
    }

    private void Update()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        UpdateTarget();
        GetDistance();
        HandleCombat();
    }

    protected abstract void UpdateTarget();

    private void GetDistance()
    {
        if (_target == null) return;
        _currentDistance = Vector2.Distance(transform.position, _target.position);
    }

    private void HandleCombat()
    {
        if (_target == null) return;
        if (!CanAttack()) return;
        StartAttack();
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

    private void StartAttack()
    {
        _enemyCtrl.EnemyAnimation.SetAttackTrigger();
    }

    public override void Send()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_pointAttack.position, _rangeAttack, _playerLayer);
        if (colliders.Length == 0) return;
        foreach (Collider2D collider in colliders)
        {
            if (!collider.GetComponent<PlayerDamageReceiver>()) continue;
            PlayerCtrl player = collider.GetComponentInParent<PlayerCtrl>();
            if (player == null || player.PlayerDamageReceiver.isDead) continue;
            _enemyCtrl.EnemyDamageSender.Send(player);
        }
    }

    private void OnDrawGizmos()
    {
        DrawAttackGizmos();
    }

    protected virtual void DrawAttackGizmos()
    {
        if (_pointAttack == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_pointAttack.position, _rangeAttack);
    }
}
