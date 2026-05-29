using Photon.Pun;
using UnityEngine;

public class EnemyExplosionCombat : EnemyCombat<EnemyExplosionCtrl>
{
    [SerializeField] private float _explodeRange = 0.6f;
    [SerializeField] private float _blastRadius = 2f;
    [SerializeField] private bool _hasExploded = false;
    protected void OnEnable()
    {
        _hasExploded = false;
    }
    private void Update()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        if (_hasExploded) return;
        CheckExplode();
    }

    private void CheckExplode()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _explodeRange, _playerLayer);
        if (hit == null) return;
        Explode();
    }

    private void Explode()
    {
        _hasExploded = true;
        _enemyCtrl.PhotonView.RPC("RpcForceKill", RpcTarget.All);
    }

    public void BlastDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerCtrl player = hit.GetComponentInParent<PlayerCtrl>();
            if (player == null) continue;
            _enemyCtrl.EnemyDamageSender.Send(player);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explodeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _blastRadius);
    }
}
