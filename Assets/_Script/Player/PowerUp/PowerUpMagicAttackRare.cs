public class PowerUpMagicAttackRare : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.MagicAttackRare; }
    protected override void ApplyBuff() => _player.PlayerDamageSender.AddPercentMagicalDamage(0.3f);
    protected override void RemoveBuff() => _player.PlayerDamageSender.AddPercentMagicalDamage(-0.3f);
}
