public class PowerUpPhysAttackEpic : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.PhysAttackEpic; }
    protected override void ApplyBuff() => _player.PlayerDamageSender.AddPercentPhysicalDamage(0.5f);
    protected override void RemoveBuff() => _player.PlayerDamageSender.AddPercentPhysicalDamage(-0.5f);
}
