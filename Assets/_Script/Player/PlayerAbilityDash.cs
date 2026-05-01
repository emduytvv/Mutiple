using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityDash : SaiMonoBehaviour
{
    [SerializeField] protected PhotonView _photonView;
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
        this.LoadPhotonView();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
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
        if (!_photonView.IsMine) return;
        if (_cooldownTimer > 0f) return;
        _cooldownTimer = _cooldown;
        _isDashing = true;
        _dashTimer = _dashDuration;
        GameEvents.OnPlayerDashed?.Invoke(_dashForce);
    }
}
