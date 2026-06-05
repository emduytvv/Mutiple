using UnityEngine;

public class EnemyExplosionCtrl : EnemyCtrl
{
    public EnemyExplosionMovement ExplosionMovement => _explosionMovement;
    [SerializeField] protected EnemyExplosionMovement _explosionMovement;
    public EnemyExplosionCombat ExplosionCombat => _explosionCombat;
    [SerializeField] protected EnemyExplosionCombat _explosionCombat;
    public EnemyExplosionAnimation ExplosionAnimation => _enemyAnimation as EnemyExplosionAnimation;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosionMovement();
        this.LoadExplosionCombat();
    }

    private void LoadExplosionMovement()
    {
        if (_explosionMovement != null) return;
        _explosionMovement = GetComponentInChildren<EnemyExplosionMovement>();
    }
    private void LoadExplosionCombat()
    {
        if (_explosionCombat != null) return;
        _explosionCombat = GetComponentInChildren<EnemyExplosionCombat>();
    }
}
