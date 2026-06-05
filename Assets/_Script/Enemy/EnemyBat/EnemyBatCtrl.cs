using UnityEngine;

public class EnemyBatCtrl : EnemyCtrl
{
    public EnemyFlyMovement FlyMovement => _flyMovement;
    [SerializeField] protected EnemyFlyMovement _flyMovement;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadFlyMovement();
    }

    private void LoadFlyMovement()
    {
        if (_flyMovement != null) return;
        _flyMovement = GetComponentInChildren<EnemyFlyMovement>();
    }
}
