using UnityEngine;

public class EnemyCombat<TCtrl> : EnemyCombatBase where TCtrl : EnemyCtrl
{
    [SerializeField] protected TCtrl _enemyCtrl;
    [SerializeField] protected LayerMask _playerLayer;
    [SerializeField] protected float _coolDown = 1f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
        this.LoadPlayerLayer();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        if (_enemyCtrl?.EnemyStatsSO == null) return;
        if (_enemyCtrl.EnemyStatsSO._cooldown <= 0) return;
        _coolDown = _enemyCtrl.EnemyStatsSO._cooldown;
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<TCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    private void LoadPlayerLayer()
    {
        if (this._playerLayer != 0) return;
        this._playerLayer = LayerMask.GetMask("Player");
        Debug.Log(transform.name + ": Load PlayerLayer", gameObject);
    }
}
