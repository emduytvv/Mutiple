using UnityEngine;

public class BuffArmorPenetration : BaseIntrinsicSkill
{
    [SerializeField] protected float _armorPenetrationBonus = 0.1f;
    private void Update()
    {
        if (!_isActive) return;
        Apply();
    }
    protected virtual void Apply()
    {
        _player.PlayerDamageSender.AddArmorPenetration(_armorPenetrationBonus);
        _isActive = false;
    }
}
