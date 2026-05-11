using UnityEngine;

public abstract class EnemyCombat : SaiMonoBehaviour
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;
    [SerializeField] protected LayerMask _playerLayer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
        this.LoadPlayerLayer();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    private void LoadPlayerLayer()
    {
        if (this._playerLayer != 0) return;
        this._playerLayer = LayerMask.GetMask("Player");
        Debug.Log(transform.name + ": Load PlayerLayer", gameObject);
    }
}
