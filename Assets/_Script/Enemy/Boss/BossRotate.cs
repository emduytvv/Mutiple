using Unity.Mathematics;
using UnityEngine;

public class BossRotate : SaiMonoBehaviour
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

    protected void Update()
    {
        Rotate();
    }

    private void Rotate() { }
}
