using System;
using UnityEngine;

public class PlayerHPBar : BaseSlider
{
    [SerializeField] protected PlayerDamageReceiver _damageReceiver;
    private void LateUpdate()
    {
        GetHp();
    }
    private void GetHp()
    {
        if (_damageReceiver == null) return;
        slider.value = _damageReceiver.CurrentHp / _damageReceiver.maxHP;
    }
}
