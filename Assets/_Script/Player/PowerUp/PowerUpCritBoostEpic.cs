public class PowerUpCritBoostEpic : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.CritBoostEpic; }
    protected override void ApplyBuff() => _player.PlayerDamageSender.AddCriticalRate(0.35f);
    protected override void RemoveBuff() => _player.PlayerDamageSender.AddCriticalRate(-0.35f);
}
