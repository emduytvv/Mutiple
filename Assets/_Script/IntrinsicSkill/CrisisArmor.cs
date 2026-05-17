using System;
using UnityEngine;

public class CrisisArmor : BaseIntrinsicSkill
{
    private bool _buffActive = false;
    [SerializeField] private float _threshold = 0.2f;
    [SerializeField] private float _attackMultiplier = 0.5f;
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
            AddDefense(_attackMultiplier);
        }
        else if (!CanImplement() && _buffActive)
        {
            _buffActive = false;
            AddDefense(-_attackMultiplier);
        }
    }
    protected void AddDefense(float attackMultiplier)
    {
        _player.PlayerDamageReceiver.AddMagicalDefense(attackMultiplier);
        _player.PlayerDamageReceiver.AddPhysicalDefense(attackMultiplier);

    }
    private bool CanImplement()
    {
        return _player.PlayerDamageReceiver.GetCurrrentHPPercent() < _threshold;
    }


}
