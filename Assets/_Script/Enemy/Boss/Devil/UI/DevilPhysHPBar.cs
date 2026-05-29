using UnityEngine;

public class DevilPhysHPBar : BaseSlider
{
    [SerializeField] private DevilDamageReceiver _damageReceiver;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 5.5f, 0f);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_damageReceiver != null) return;
        _damageReceiver = transform.parent.parent.GetComponentInChildren<DevilDamageReceiver>();
    }

    private void LateUpdate()
    {
        GetHp();
        FollowBoss();
    }

    private void GetHp()
    {
        if (_damageReceiver == null) return;
        slider.value = _damageReceiver.PhysCurrentHP / _damageReceiver.PhysMaxHP;
    }

    private void FollowBoss()
    {
        transform.position = transform.parent.parent.position + _offset;
    }
}
