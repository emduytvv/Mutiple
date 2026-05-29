public class EnemyShooterCombat : EnemyShooterCombatBase
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _bulletName = NameBullet.Bullet_WandererMagican.ToString();
    }
}
