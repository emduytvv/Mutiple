using Photon.Pun;
using UnityEngine;

public class PlayerShoot : SaiMonoBehaviour
{
    [SerializeField] protected PlayerCtrl _playerCtrl;
    [SerializeField] private float _aimAngle90;
    [SerializeField] protected float _aimAngle180;
    private bool _isAiming;
    protected Vector3 centerAim = Vector3.up * 0.85f;

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

    private void Update()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (InputManager.Instance == null) return;
        if (_playerCtrl.PlayerAnimation.CurrentState == PlayerState.Die)
        {
            _isAiming = false;
            return;
        }
        HandleAimInput();
    }

    private void HandleAimInput()
    {
        if (InputManager.Instance.RightMouseDown) StartAim();
        if (_isAiming) UpdateAimAngle();
        if (InputManager.Instance.RightMouseUp) OnShoot();
    }

    private void StartAim()
    {
        _isAiming = true;
        GameEvents.OnPlayerStartAim?.Invoke();
    }

    private void OnShoot()
    {
        _isAiming = false;
        Shoot();
        GameEvents.OnPlayerShoot?.Invoke();
    }

    private void UpdateAimAngle()
    {
        Vector2 dir = InputManager.Instance.MousePosition - (Vector2)(transform.parent.position + centerAim);
        _aimAngle90 = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;
        _aimAngle90 = Mathf.Clamp(_aimAngle90, 0f, 90f);
        GameEvents.OnPlayerAimAngleChanged?.Invoke(_aimAngle90);
    }

    private void Shoot()
    {
        UpdateAimAngle180();
        Vector3 spawnPos = GetArrowSpawnPos();
        Quaternion rotation = Quaternion.Euler(0, 0, _aimAngle180);
        PhotonNetwork.Instantiate("Arrow_Raidon", spawnPos, rotation);
    }

    private void UpdateAimAngle180()
    {
        bool facingRight = _playerCtrl.PlayerAnimation.transform.localScale.x > 0;
        _aimAngle180 = facingRight ? _aimAngle90 : 180f - _aimAngle90;
    }

    private Vector3 GetArrowSpawnPos()
    {
        float rad = _aimAngle180 * Mathf.Deg2Rad;
        Vector3 center = transform.parent.position + centerAim;
        return center + new Vector3(Mathf.Cos(rad) * 0.5f, Mathf.Sin(rad) * 0.5f, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.parent.position + centerAim, 0.5f);
    }
}
