using Photon.Pun;
using UnityEngine;

public class ArrowDamageSender : DamageSender
{
    [SerializeField] protected ArrowDespawn _arrowDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_arrowDespawn != null) return;
        _arrowDespawn = transform.parent.GetComponentInChildren<ArrowDespawn>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(transform.name + ": OnTriggerEnter2D", gameObject);
        EnemyCtrl enemy = collision.GetComponentInParent<EnemyCtrl>();

        if (enemy == null) return;
        if (!_photonView.IsMine) return;

        enemy.PhotonView.RPC("RpcReceive", RpcTarget.All, maxDamage);
        _arrowDespawn.DespawnObject();
    }
}
