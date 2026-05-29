using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossAnimation : SaiMonoBehaviour
{
    [SerializeField] protected BossCtrl _bossCtrl;
    [SerializeField] protected Animator _animator;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBossCtrl();
        this.LoadAnimator();
    }

    private void LoadBossCtrl()
    {
        if (_bossCtrl != null) return;
        _bossCtrl = GetComponentInParent<BossCtrl>();
    }

    private void LoadAnimator()
    {
        if (_animator != null) return;
        _animator = GetComponent<Animator>();
    }
}
