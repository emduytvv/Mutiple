using UnityEngine;

public class EnemyShooterCtrl : EnemyCtrl
{
    public EnemyMovementOnPlatform ShooterMovement => _shooterMovement;
    [SerializeField] protected EnemyMovementOnPlatform _shooterMovement;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadShooterMovement();
    }

    private void LoadShooterMovement()
    {
        if (_shooterMovement != null) return;
        _shooterMovement = GetComponentInChildren<EnemyMovementOnPlatform>();
    }
}
