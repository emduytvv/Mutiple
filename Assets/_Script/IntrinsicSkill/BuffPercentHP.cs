using UnityEngine;

public class BuffPercentHP : BaseIntrinsicSkill
{
    [SerializeField] protected float _percentHPBonus = 0.1f;

    private void Update()
    {
        if (!_isActive) return;
        Apply();
    }
    protected virtual void Apply()
    {
        _player.PlayerDamageReceiver.AddPercentHPBonus(_percentHPBonus);
        _isActive = false;
    }
}
