using UnityEngine;

public abstract class BossCombat<TCtrl> : SaiMonoBehaviour where TCtrl : BossCtrl
{
    [SerializeField] protected TCtrl _bossCtrl;
    [SerializeField] protected LayerMask _playerLayer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBossCtrl();
        this.LoadPlayerLayer();
    }

    private void LoadBossCtrl()
    {
        if (_bossCtrl != null) return;
        _bossCtrl = GetComponentInParent<TCtrl>();
    }

    private void LoadPlayerLayer()
    {
        if (_playerLayer != 0) return;
        _playerLayer = LayerMask.GetMask("Player");
    }
}
