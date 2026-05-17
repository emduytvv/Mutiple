using UnityEngine;

public class BuffCritical : BaseIntrinsicSkill
{
    [SerializeField] protected float _criticalRateBonus = 0.1f;
    private void Update()
    {
        if (!_isActive) return;
        Apply();
    }
    protected virtual void Apply()
    {
        _player.PlayerDamageSender.AddCriticalRate(_criticalRateBonus);
        _isActive = false;
    }
}
