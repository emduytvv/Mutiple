using Photon.Pun;
using UnityEngine;
public class PlayerDamageReceiver : DamageReceiver
{
    protected override void Start()
    {
        GameEvents.OnPlayerRevived += OnRevived;
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        baseMaxHP = 30f;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerRevived -= OnRevived;
    }
    private void OnRevived(int viewId)
    {
        if (_photonView.ViewID != viewId) return;
        Revive();
    }

    public void Revive()
    {
        isDead = false;
        currentHp = maxHP * 0.3f;
    }

    public override void Receiver(float damage)
    {
        base.Receiver(damage);
    }

    protected override void OnDead()
    {
        GameEvents.OnPlayerDied?.Invoke(_photonView.ViewID);
    }
}
