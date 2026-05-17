using UnityEngine;

public class BuffDamageMagical : BaseIntrinsicSkill
{
    [SerializeField] protected float _magicalDamageBonus = 10f;
    private void Update()
    {
        if (!_isActive) return;
        Apply();
    }
    protected virtual void Apply()
    {
        _player.PlayerDamageSender.AddMagicDamage(_magicalDamageBonus);
        _isActive = false;
    }
}
