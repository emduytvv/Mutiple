using System;
using System.Collections.Generic;
using UnityEngine;
public class BuffDamagePhysical : BaseIntrinsicSkill
{
    [SerializeField] protected float _physicalDamageBonus = 10f;
    private void Update()
    {
        if (!_isActive) return;
        Apply();
    }
    protected virtual void Apply()
    {
        _player.PlayerDamageSender.AddPhysicalDamage(_physicalDamageBonus);
        _isActive = false;
    }

}
