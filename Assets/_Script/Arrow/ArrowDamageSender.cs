using System;
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
        baseMagicalDamage = mag;
        _armorPen = pen;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        baseMagicalDamage = 1f;
        basePhysicalDamage = 1f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasHit) return;
        EnemyCtrl enemy = collision.GetComponentInParent<EnemyCtrl>();
        if (enemy == null) return;
        if (!_arrowCtrl.PhotonView.IsMine) return;
        OnHitEnemy(enemy, collision);
        SpawnFX();
    }

    protected virtual void SpawnFX()
    {
        PhotonNetwork.Instantiate(FXName.ImpactArrow.ToString(), transform.position, transform.rotation);
    }


    protected virtual void OnHitEnemy(EnemyCtrl enemy, Collider2D collision) { }

    protected virtual void OnEnable() => _hasHit = false;
}
