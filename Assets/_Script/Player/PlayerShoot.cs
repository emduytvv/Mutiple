using Photon.Pun;
using UnityEngine;

public class PlayerShoot : SaiMonoBehaviour
{
    [SerializeField] protected PhotonView _photonView;
    [SerializeField] private float _aimAngle;
    private bool _isAiming;

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

    private void Update()
    {
        if (!_photonView.IsMine) return;
        if (InputManager.Instance == null) return;

        if (InputManager.Instance.RightMouseDown)
        {
            _isAiming = true;
            GameEvents.OnPlayerStartAim?.Invoke();
        }

        if (_isAiming) UpdateAimAngle();

        if (InputManager.Instance.RightMouseUp)
        {
            _isAiming = false;
            GameEvents.OnPlayerShoot?.Invoke();
        }
    }

    private void UpdateAimAngle()
    {
        Vector2 dir = InputManager.Instance.MousePosition - (Vector2)transform.parent.position;
        _aimAngle = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;
        _aimAngle = Mathf.Clamp(_aimAngle, 0f, 90f);
        GameEvents.OnPlayerAimAngleChanged?.Invoke(_aimAngle);
    }
}
