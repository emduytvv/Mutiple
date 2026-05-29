using UnityEngine;

public class EnemyMeleeCombat : EnemyMeleeCombatBase<EnemyMeleeCtrl>
{
    [SerializeField] private float _detectionLength = 6f;

    protected override void UpdateTarget()
    {
        Vector2 startPoint = (Vector2)transform.position + Vector2.up * 0.5f + Vector2.left * _detectionLength;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.right, _detectionLength * 2, _playerLayer);
        _target = hit.transform != null ? hit.transform : null;
        _enemyCtrl.MeleeMovement.SetTarget(_target);
    }

    protected override void DrawAttackGizmos()
    {
        base.DrawAttackGizmos();
        Gizmos.color = Color.green;
        Vector3 origin = transform.position + Vector3.up * 0.7f;
        Gizmos.DrawLine(origin + Vector3.left * _detectionLength, origin + Vector3.right * _detectionLength);
    }
}
