using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PlayerShootChargeBar : BaseSlider
{
    private float _maxChargeTime = 0.4f;
    private float _chargeTimer;
    private bool _isAiming;
    private CanvasGroup _canvasGroup;
    private Vector3 _pointPlayer = -Vector3.right * 0.8f + Vector3.up * 0.62f;
    [SerializeField] private PlayerCtrl _playerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCanvasGroup();
        this.LoadPlayerCtrl();
    }

    private void LoadCanvasGroup()
    {
        if (_canvasGroup != null) return;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        Hide();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerStartAim += Show;
        GameEvents.OnPlayerShoot += Hide;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerStartAim -= Show;
        GameEvents.OnPlayerShoot -= Hide;
    }

    protected void FixedUpdate()
    {
        FollowPlayer();
        UpdateCharge();
    }

    private void UpdateCharge()
    {
        if (!_isAiming) return;
        _chargeTimer = Mathf.Min(_chargeTimer + Time.fixedDeltaTime, _maxChargeTime);
        slider.value = _chargeTimer / _maxChargeTime;
    }

    private void FollowPlayer()
    {
        transform.position = transform.parent.parent.position + _pointPlayer;
    }

    private void Show()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        _canvasGroup.alpha = 0.5f;
        _chargeTimer = 0f;
        _isAiming = true;
        slider.value = 0f;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        _isAiming = false;
        slider.value = 0f;
    }
}
