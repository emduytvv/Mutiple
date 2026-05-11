using Photon.Pun;
using UnityEngine;

public class EnemyDespawn : Despawn
{
    [SerializeField] private EnemyCtrl _enemyCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
    }

    protected override bool CanDespawn()
    {
        return false;
    }

    public override void DespawnObject()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        PhotonNetwork.Destroy(_enemyCtrl.PhotonView.gameObject);
    }
}
