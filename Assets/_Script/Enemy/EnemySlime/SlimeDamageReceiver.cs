using System;
using UnityEngine;

public class SlimeDamageReceiver : EnemyDamageReceiver
{
    private SlimeCtrl _slimeCtrl => _enemyCtrl as SlimeCtrl;
    protected override void OnHurt()
    {
        base.OnHurt();
        if (_slimeCtrl == null)
        {
            Debug.LogError("SlimeCtrl is null");
            return;
        }
        _slimeCtrl.SlimeCombat.Implement();
    }


}
