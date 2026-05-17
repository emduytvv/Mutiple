using System;
using UnityEngine;

public class DashHeal : BaseIntrinsicSkill
{
    [SerializeField] private float _dashHealPercent = 0.03f;
    protected void OnEnable()
    {
        GameEvents.OnPlayerDashEnded += OnDashEnded;
    }
    protected void OnDisable()
    {
        GameEvents.OnPlayerDashEnded -= OnDashEnded;
    }
    private void OnDashEnded()
    {
        if (!_isActive) return;
        _player.PlayerDamageReceiver.BuffPercentHP(_dashHealPercent);
    }



}
