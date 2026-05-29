public class PowerUpHealEpic : BasePowerUpEffect
{
    protected override void ResetValue() { base.ResetValue(); _effectName = PowerUpEffectName.HealEpic; }
    protected override void ApplyBuff() => _player.PhotonView.RPC("RpcBuffPercentHP", Photon.Pun.RpcTarget.All, 0.5f);
    protected override void RemoveBuff() { }
}
