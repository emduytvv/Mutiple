public class PowerUpMagicDefenseEpic : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.MagicDefenseEpic; }
    protected override void ApplyBuff() => _player.PlayerDamageReceiver.AddMagicalReduction(0.8f);
    protected override void RemoveBuff() => _player.PlayerDamageReceiver.AddMagicalReduction(-0.8f);
}
