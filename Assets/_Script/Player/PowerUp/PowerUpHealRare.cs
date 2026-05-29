public class PowerUpHealRare : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.HealRare; }
    protected override void ApplyBuff() => _player.PhotonView.RPC("RpcBuffPercentHP", Photon.Pun.RpcTarget.All, 0.3f);
    protected override void RemoveBuff() { }
}
