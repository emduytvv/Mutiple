using UnityEngine;

public class EnemyAirmCombat : EnemyShooterCombatBase
{
    [SerializeField] private bool _isAirm;
    [SerializeField] private float _airmDuration = 5f;
    [SerializeField] private float _airmTimer;
    [SerializeField] private LineRenderer _lineRenderer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLineRenderer();
    }

    private void LoadLineRenderer()
    {
        if (_lineRenderer != null) return;
        _lineRenderer = GetComponent<LineRenderer>();
        Debug.Log(transform.name + ": Load LineRenderer", gameObject);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        _bulletName = NameBullet.Bullet_Sniper.ToString();
    }

    protected override bool IsReadyToFire()
    {
        if (_isAirm) return true;
        HandleAirm();
        return false;
    }

    private void HandleAirm()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _target.position + Vector3.up * 0.5f);
        _airmTimer += Time.deltaTime;
        if (_airmTimer < _airmDuration) return;
        _lineRenderer.enabled = false;
        _isAirm = true;
    }

    protected override void ResetCombat()
    {
        _lineRenderer.enabled = false;
        _isAirm = false;
        _airmTimer = 0;
        base.ResetCombat();
    }
}
