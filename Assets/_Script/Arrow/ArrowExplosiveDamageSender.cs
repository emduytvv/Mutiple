using Photon.Pun;
using UnityEngine;

public class ArrowExplosiveDamageSender : ArrowDamageSender
{
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLayerEnemy();
    }

    private void LoadLayerEnemy()
    {
        if (_enemyLayer != 0) return;
        _enemyLayer = LayerMask.GetMask("Enemy");
        Debug.Log(transform.name + ": Load EnemyLayer", gameObject);
    }

    protected override void OnHitEnemy(EnemyCtrl enemy, Collider2D collision)
    {
        _hasHit = true;
        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage, 0f);
        Explode(collision.transform.position);
        _arrowCtrl.ArrowDespawn.DespawnObject();
    }
    protected override void SpawnFX()
    {
        PhotonNetwork.Instantiate(FXName.ImpactArrowExplosive.ToString(), transform.position, Quaternion.identity);
    }
    private void Explode(Vector3 center)
    {
        var hits = Physics2D.OverlapCircleAll(center, _radius, _enemyLayer);
        foreach (var hit in hits)
            if (hit.TryGetComponent<EnemyDamageReceiver>(out var enemy))
                enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, baseMagicalDamage, basePhysicalDamage, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
