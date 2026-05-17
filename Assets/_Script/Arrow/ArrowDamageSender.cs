using Photon.Pun;
using UnityEngine;

public class ArrowDamageSender : DamageSender
{
    [SerializeField] protected ArrowCtrl _arrowCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadArrowCtrl();
    }

    private void LoadArrowCtrl()
    {
        if (_arrowCtrl != null) return;
        _arrowCtrl = GetComponentInParent<ArrowCtrl>();
    }

    protected bool _hasHit;
    protected float _armorPen = 0f;

    public void SetDamage(float phys, float mag, float pen)
    {
        basePhysicalDamage = phys;
        baseMagicalDamage  = mag;
        _armorPen          = pen;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        baseMagicalDamage  = 1f;
        basePhysicalDamage = 1f;
    }

    protected virtual void OnEnable() => _hasHit = false;
}
