public class PowerUpPhysDefenseRare : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.PhysDefenseRare; }
    protected override void ApplyBuff() => _player.PlayerDamageReceiver.AddPhysicalReduction(0.5f);
    protected override void RemoveBuff() => _player.PlayerDamageReceiver.AddPhysicalReduction(-0.5f);
}
