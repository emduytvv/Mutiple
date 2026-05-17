using System;
using UnityEngine;

public class LastStand : BaseIntrinsicSkill
{
    private bool _buffActive = false;
    [SerializeField] private float _threshold = 0.2f;
    [SerializeField] private float _attackMultiplier = 0.4f;
    protected void Update()
    {
        OnHPSmall();
    }
    private void OnHPSmall()
    {
        if (!_isActive) return;

        if (CanImplement() && !_buffActive)
        {
            _buffActive = true;
            AddDamage(_attackMultiplier);
        }
        else if (!CanImplement() && _buffActive)
        {
            _buffActive = false;
            AddDamage(-_attackMultiplier);
        }
    }
    protected void AddDamage(float attackMultiplier)
    {
        _player.PlayerDamageSender.AddPercentDamage(attackMultiplier);
    }
    private bool CanImplement()
    {
        return _player.PlayerDamageReceiver.GetCurrrentHPPercent() < _threshold;
    }


}
