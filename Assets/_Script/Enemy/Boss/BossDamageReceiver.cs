using UnityEngine;

public class BossDamageReceiver : DamageReceiver
{
    [SerializeField] protected BossCtrl _bossCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBossCtrl();
    }

    private void LoadBossCtrl()
    {
        if (_bossCtrl != null) return;
        _bossCtrl = GetComponentInParent<BossCtrl>();
    }

    protected override void OnDead() { }

    protected override void ResetValue()
    {
        base.ResetValue();
        baseMaxHP = 1000f;
    }
}
