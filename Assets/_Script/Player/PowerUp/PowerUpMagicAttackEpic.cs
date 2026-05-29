public class PowerUpMagicAttackEpic : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.MagicAttackEpic; }
    protected override void ApplyBuff() => _player.PlayerDamageSender.AddPercentMagicalDamage(0.5f);
    protected override void RemoveBuff() => _player.PlayerDamageSender.AddPercentMagicalDamage(-0.5f);
}
