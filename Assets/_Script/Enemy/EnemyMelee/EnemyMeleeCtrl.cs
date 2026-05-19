using System;
using Photon.Pun;
using UnityEngine;

public class EnemyMeleeCtrl : EnemyCtrl
{
    [SerializeField] protected EnemyMeleeCombat _meleeCombat;
    public EnemyMeleeCombat MeleeCombat => _meleeCombat;
    [SerializeField] protected EnemyMeleeMovement _meleeMovement;
    public EnemyMeleeMovement MeleeMovement => _meleeMovement;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMeleeCombat();
        this.LoadMeleeMovement();
    }

    private void LoadMeleeCombat()
    {
        if (this._meleeCombat != null) return;
        this._meleeCombat = transform.GetComponentInChildren<EnemyMeleeCombat>();
        Debug.Log(transform.name + ": Load MeleeCombat", gameObject);
    }

    private void LoadMeleeMovement()
    {
        if (this._meleeMovement != null) return;
        this._meleeMovement = transform.GetComponentInChildren<EnemyMeleeMovement>();
        Debug.Log(transform.name + ": Load MeleeMovement", gameObject);
    }

}
