public class PowerUpPhysAttackRare : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.PhysAttackRare; }
    protected override void ApplyBuff() => _player.PlayerDamageSender.AddPercentPhysicalDamage(0.3f);
    protected override void RemoveBuff() => _player.PlayerDamageSender.AddPercentPhysicalDamage(-0.3f);
}
