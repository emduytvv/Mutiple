public class PowerUpMagicDefenseRare : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.MagicDefenseRare; }
    protected override void ApplyBuff() => _player.PlayerDamageReceiver.AddMagicalReduction(0.5f);
    protected override void RemoveBuff() => _player.PlayerDamageReceiver.AddMagicalReduction(-0.5f);
}
