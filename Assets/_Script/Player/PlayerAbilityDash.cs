using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityDash : SaiMonoBehaviour
{
    [SerializeField] protected PlayerCtrl _playerCtrl;
    [SerializeField] protected InputAction _dashAction;
    [SerializeField] protected float _cooldown = 1f;
    [SerializeField] protected float _dashForce = 15f;
    [SerializeField] protected float _dashDuration = 0.2f;
    private float _cooldownTimer;
    private float _dashTimer;
    private bool _isDashing;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        _dashAction = new InputAction("Dash", binding: "<Keyboard>/leftShift");
    }

    protected override void Start()
    {
        _dashAction.Enable();
        _dashAction.performed += _ => OnDash();
    }

    private void Update()
    {
        if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
            {
                _isDashing = false;
                GameEvents.OnPlayerDashEnded?.Invoke();
            }
        }
    }

    private void OnDash()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_playerCtrl.PlayerAnimation.CurrentState == PlayerState.Die) return;
        if (_cooldownTimer > 0f) return;
        _cooldownTimer = _cooldown;
        _isDashing = true;
        _dashTimer = _dashDuration;
        GameEvents.OnPlayerDashed?.Invoke(_dashForce);
    }
}
