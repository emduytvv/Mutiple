using Photon.Pun;
using UnityEngine;

public class EnemyMeleeCombat : EnemyCombat<EnemyMeleeCtrl>
{
    private Transform _pointAttack;

    private float _detectionLength = 6f;
    private float _coolDown = 1f;
    private float _coolDownTimer = 0f;

    private Transform _target;
    protected float _distanceToAttack = 1.5f;
    protected float _rangeAttack = 0.7f;
    protected float _currentDistance;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPointAttack();
    }

    private void LoadPointAttack()
    {
        if (this._pointAttack != null) return;
        this._pointAttack = transform.Find("PointAttack");
        Debug.Log(transform.name + ": Load PointAttack", gameObject);
    }

    private void Update()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        this.ProcessCombat();
    }

    private void ProcessCombat()
    {
        UpdateTarget();
        GetDistance();
        HandleCombat();
    }

    private void GetDistance()
    {
        if (_target == null) return;
        _currentDistance = Vector2.Distance(transform.position, _target.position);
    }

    private void UpdateTarget()
    {
        Vector2 startPoint = (Vector2)transform.position + Vector2.up * 0.5f + Vector2.left * _detectionLength;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.right, _detectionLength * 2, _playerLayer);
        _target = hit.transform != null ? hit.transform : null;
        _enemyCtrl.MeleeMovement.SetTarget(_target);
    }

    private void HandleCombat()
    {
        if (_target == null) return;
        if (!CanAttack()) return;
        StartAttack();
    }

    private bool CanAttack()
    {
        _coolDownTimer += Time.deltaTime;
        if (_coolDownTimer < _coolDown) return false;
        if (_currentDistance < _distanceToAttack)
        {
            _coolDownTimer = 0;
            return true;
        }
        return false;
    }

    private void StartAttack()
    {
        _enemyCtrl.EnemyAnimation.SetAttackTrigger();
    }

    public void Attack()
    {
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

        Gizmos.color = Color.green;
        Vector3 origin = transform.position + Vector3.up * 0.7f;
        Gizmos.DrawLine(origin + Vector3.left * _detectionLength, origin + Vector3.right * _detectionLength);
    }
}
