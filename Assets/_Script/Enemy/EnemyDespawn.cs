using Photon.Pun;
using UnityEngine;

public class EnemyDespawn : Despawn
{
    [SerializeField] private EnemyCtrl _enemyCtrl;
    private bool _isDespawning;

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

    protected virtual void OnEnable()
    {

        _isDespawning = false;
    }

    protected override bool CanDespawn()
    {
        return false;
    }

    public override void DespawnObject()
    {
        if (_isDespawning) return;
        if (!_enemyCtrl.PhotonView.IsMine) return;
        _isDespawning = true;
        PhotonNetwork.Destroy(_enemyCtrl.PhotonView.gameObject);
    }
}
