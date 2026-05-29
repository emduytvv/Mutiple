public class PowerUpCritBoostRare : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.CritBoostRare; }
    protected override void ApplyBuff() => _player.PlayerDamageSender.AddCriticalRate(0.2f);
    protected override void RemoveBuff() => _player.PlayerDamageSender.AddCriticalRate(-0.2f);
}
