public class PowerUpPhysDefenseEpic : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.PhysDefenseEpic; }
    protected override void ApplyBuff() => _player.PlayerDamageReceiver.AddPhysicalReduction(0.8f);
    protected override void RemoveBuff() => _player.PlayerDamageReceiver.AddPhysicalReduction(-0.8f);
}
