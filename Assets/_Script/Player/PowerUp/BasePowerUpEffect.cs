using UnityEngine;

public abstract class BasePowerUpEffect : SaiMonoBehaviour
{
    [SerializeField] protected PowerUpEffectName _effectName;
    public PowerUpEffectName EffectName => _effectName;

    [SerializeField] protected bool _isActive = false;
    protected float _remainingTime;
    protected PowerUpDataSO _data;
    protected PlayerCtrl _player;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_player != null) return;
        _player = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    public void Activate(PowerUpDataSO so)
    {
        _data = so;
        if (_isActive)
        {
            _remainingTime = so._duration;
            return;
        }
        ApplyBuff();
        if (so._duration <= 0) return;
        _remainingTime = so._duration;
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive) return;
        _remainingTime -= Time.deltaTime;
        if (_remainingTime > 0) return;
        RemoveBuff();
        _isActive = false;
    }

    protected abstract void ApplyBuff();
    protected abstract void RemoveBuff();
}
