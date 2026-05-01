using System;
using UnityEngine;

public class PlayerHPSlider : BaseSlider
{
    [SerializeField] private PlayerDamageReceiver _damageReceiver;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_damageReceiver != null) return;
        _damageReceiver = transform.parent.parent.GetComponentInChildren<PlayerDamageReceiver>();
    }
    protected void FixedUpdate()
    {
        FollowPlayer();
    }
    private void LateUpdate()
    {
        GetHp();
    }

    private void GetHp()
    {
        if (_damageReceiver == null) return;
        slider.value = _damageReceiver.currentHp / _damageReceiver.maxHP;
    }


    private void FollowPlayer()
    {
        transform.position = transform.parent.parent.position + Vector3.up * 2.5f;
    }
}
