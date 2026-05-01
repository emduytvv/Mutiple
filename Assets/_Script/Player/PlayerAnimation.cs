using Photon.Pun;
using UnityEngine;

public enum PlayerState { Idle, Run, Jump, Drop, Land, Aim, Shoot, Dash }

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public Animator Animator => _animator;
    [SerializeField] protected Animator _animator;
    public PlayerMovement PlayerMovement => _playerMovement;
    [SerializeField] protected PlayerMovement _playerMovement;

    static readonly int HashIsRun = Animator.StringToHash("isRun");
    static readonly int HashJump = Animator.StringToHash("jump");
    static readonly int HashLand = Animator.StringToHash("land");
    static readonly int HashDrop = Animator.StringToHash("drop");
    static readonly int HashIsAiming = Animator.StringToHash("isAiming");
    static readonly int HashAimAngle = Animator.StringToHash("aimAngle");
    static readonly int HashShoot = Animator.StringToHash("shoot");
    static readonly int HashDash = Animator.StringToHash("dash");

    [SerializeField] protected PlayerState _currentState;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadAnimator();
        this.LoadPlayerMovement();
    }

    private void LoadPhotonView()
    {
        if (this._photonView != null) return;
        this._photonView = GetComponentInParent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
    }

    private void LoadAnimator()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
        Debug.Log(transform.name + ": Load Animator", gameObject);
    }

    private void LoadPlayerMovement()
    {
        if (this._playerMovement != null) return;
        this._playerMovement = transform.parent.GetComponentInChildren<PlayerMovement>();
        Debug.Log(transform.name + ": Load PlayerMovement", gameObject);
    }

    protected override void Start()
    {
        GameEvents.OnPlayerJumped += OnJump;
        GameEvents.OnPlayerLanded += OnLand;
        GameEvents.OnPlayerDashed += OnDash;
        GameEvents.OnPlayerStartAim += OnStartAim;
        GameEvents.OnPlayerShoot += OnShoot;
        GameEvents.OnPlayerAimAngleChanged += OnAimAngleChanged;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerJumped -= OnJump;
        GameEvents.OnPlayerLanded -= OnLand;
        GameEvents.OnPlayerDashed -= OnDash;
        GameEvents.OnPlayerStartAim -= OnStartAim;
        GameEvents.OnPlayerShoot -= OnShoot;
        GameEvents.OnPlayerAimAngleChanged -= OnAimAngleChanged;
    }

    protected void Update()
    {
        if (!_photonView.IsMine) return;
        CheckState();
        UpdateFlip();
    }

    private void OnChangeState(PlayerState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;

        switch (_currentState)
        {
            case PlayerState.Idle:
                _animator.SetBool(HashIsRun, false);
                break;
            case PlayerState.Run:
                _animator.SetBool(HashIsRun, true);
                break;
            case PlayerState.Jump:
                _animator.ResetTrigger(HashLand);
                _animator.SetTrigger(HashJump);
                break;
            case PlayerState.Drop:
                _animator.SetTrigger(HashDrop);
                break;
            case PlayerState.Land:
                _animator.SetTrigger(HashLand);
                break;
            case PlayerState.Aim:
                SetIsAiming(true);
                break;
            case PlayerState.Shoot:
                PlayShoot();
                break;
            case PlayerState.Dash:
                _animator.SetTrigger(HashDash);
                break;
        }

        GameEvents.OnAnimStateChanged?.Invoke(newState);
    }

    private void CheckState()
    {
        switch (_currentState)
        {
            case PlayerState.Idle: HandleIdle(); break;
            case PlayerState.Run: HandleRun(); break;
            case PlayerState.Jump: HandleJump(); break;
            case PlayerState.Drop: HandleDrop(); break;
            case PlayerState.Land: HandleLand(); break;
            case PlayerState.Aim: HandleAim(); break;
            case PlayerState.Shoot: HandleShoot(); break;
            case PlayerState.Dash: HandleDash(); break;
        }
    }

    private void HandleIdle()
    {
        if (Mathf.Abs(_playerMovement.Direction.x) > 0.01f)
            OnChangeState(PlayerState.Run);
    }

    private void HandleRun()
    {
        if (Mathf.Abs(_playerMovement.Direction.x) <= 0.01f)
            OnChangeState(PlayerState.Idle);
    }

    private void HandleJump()
    {
        if (_playerMovement.VerticalVelocity < -0.1f)
            OnChangeState(PlayerState.Drop);
    }

    private void HandleDrop() { }

    private void HandleLand()
    {
        if (Mathf.Abs(_playerMovement.Direction.x) > 0.01f)
        {
            OnChangeState(PlayerState.Run);
            return;
        }

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.8f)
            OnChangeState(PlayerState.Idle);
    }

    private void HandleAim() { }

    private void OnStartAim()
    {
        if (!_photonView.IsMine) return;
        if (_currentState == PlayerState.Dash || _currentState == PlayerState.Shoot) return;
        OnChangeState(PlayerState.Aim);
    }

    private void OnShoot()
    {
        if (!_photonView.IsMine) return;
        if (_currentState != PlayerState.Aim) return;
        OnChangeState(PlayerState.Shoot);
    }

    private void HandleShoot()
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.8f)
        {
            if (Mathf.Abs(_playerMovement.Direction.x) > 0.01f)
                OnChangeState(PlayerState.Run);
            else
                OnChangeState(PlayerState.Idle);
        }
    }

    private void HandleDash()
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.8f)
        {
            if (Mathf.Abs(_playerMovement.Direction.x) > 0.01f)
                OnChangeState(PlayerState.Run);
            else
                OnChangeState(PlayerState.Idle);
        }
    }

    public void OnDash(float dashForce)
    {
        OnChangeState(PlayerState.Dash);
    }

    private void OnAimAngleChanged(float angle)
    {
        if (!_photonView.IsMine) return;
        _animator.SetFloat(HashAimAngle, angle);
    }

    private bool _facingRight = true;

    private void UpdateFlip()
    {
        float dirX;
        if ((_currentState == PlayerState.Aim || _currentState == PlayerState.Shoot)
            && InputManager.Instance != null)
            dirX = InputManager.Instance.MousePosition.x - transform.parent.position.x;
        else
            dirX = _playerMovement.Direction.x;

        if (dirX > 0.01f && !_facingRight) SetFacing(true);
        else if (dirX < -0.01f && _facingRight) SetFacing(false);
    }

    private void SetFacing(bool facingRight)
    {
        _facingRight = facingRight;
        ApplyFacing(facingRight);
        GameEvents.OnPlayerFacingChanged?.Invoke(facingRight);
    }

    private void ApplyFacing(bool facingRight)
    {
        float x = facingRight ? 0.17f : -0.17f;
        transform.localScale = new Vector3(x, 0.17f, 0.17f);
    }

    public void ApplyFacingRpc(bool facingRight)
    {
        _facingRight = facingRight;
        ApplyFacing(facingRight);
    }

    public void SetIsAiming(bool isAiming)
    {
        _animator.SetBool(HashIsAiming, isAiming);
    }

    public void PlayShoot()
    {
        _animator.SetBool(HashIsAiming, false);
        _animator.SetTrigger(HashShoot);
    }


    public void OnJump()
    {
        if (!_photonView.IsMine) return;
        if (_currentState == PlayerState.Aim || _currentState == PlayerState.Shoot) return;
        OnChangeState(PlayerState.Jump);
    }

    public void OnLand()
    {
        if (!_photonView.IsMine) return;
        if (_currentState == PlayerState.Aim || _currentState == PlayerState.Shoot) return;
        OnChangeState(PlayerState.Land);
    }
}
