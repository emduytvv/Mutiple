using UnityEngine;

public class EnemyHPBar : BaseSlider
{
    protected EnemyDamageReceiver _damageReceiver;
    [SerializeField] private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
        _damageReceiver = _target.GetComponentInChildren<EnemyDamageReceiver>();
    }

    private void LateUpdate()
    {
        if (_target == null) { Despawn(); return; }
        Follow();
        GetHp();
    }

    private void GetHp()
    {
        if (_damageReceiver == null) return;
        slider.value = _damageReceiver.CurrentHp / _damageReceiver.maxHP;
        if (slider.value <= 0) Despawn();
    }

    private void Follow()
    {
        transform.position = _target.position + Vector3.up * 2.5f;
    }

    private void Despawn()
    {
        _target = null;
        _damageReceiver = null;
        HPBarEnemySpawner.Instance.Despawn(transform.parent);
    }
}
