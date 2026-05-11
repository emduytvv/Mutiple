using Photon.Pun;
using UnityEngine;

public enum PlayerState { Idle, Run, Jump, Drop, Land, Aim, Shoot, Dash, Die }

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : SaiMonoBehaviour
{
    [SerializeField] protected PlayerCtrl _playerCtrl;
    public Animator Animator => _animator;
    [SerializeField] protected Animator _animator;

    static readonly int HashIsRun = Animator.StringToHash("isRun");
    static readonly int HashJump = Animator.StringToHash("jump");
    static readonly int HashLand = Animator.StringToHash("land");
    static readonly int HashDrop = Animator.StringToHash("drop");
    static readonly int HashIsAiming = Animator.StringToHash("isAiming");
    static readonly int HashAimAngle = Animator.StringToHash("aimAngle");
    static readonly int HashShoot = Animator.StringToHash("shoot");
    static readonly int HashDash = Animator.StringToHash("dash");
    static readonly int HashDie = Animator.StringToHash("die");
    static readonly int HashRevive = Animator.StringToHash("revive");

    [SerializeField] protected PlayerState _currentState;
    public PlayerState CurrentState => _currentState;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
        this.LoadAnimator();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    private void LoadAnimator()
    {
        if (_animator != null) return;
        _animator = GetComponent<Animator>();
        Debug.Log(transform.name + ": Load Animator", gameObject);
    }

    protected override void Start()
    {
        GameEvents.OnPlayerJumped += OnJump;
        GameEvents.OnPlayerLanded += OnLand;
        GameEvents.OnPlayerDashed += OnDash;
        GameEvents.OnPlayerStartAim += OnStartAim;
        GameEvents.OnPlayerShoot += OnShoot;
        GameEvents.OnPlayerAimAngleChanged += OnAimAngleChanged;
        GameEvents.OnPlayerRevived += OnRevived;
        GameEvents.OnPlayerDied += OnDied;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerJumped -= OnJump;
        GameEvents.OnPlayerLanded -= OnLand;
        GameEvents.OnPlayerDashed -= OnDash;
        GameEvents.OnPlayerStartAim -= OnStartAim;
        GameEvents.OnPlayerShoot -= OnShoot;
        GameEvents.OnPlayerAimAngleChanged -= OnAimAngleChanged;
        GameEvents.OnPlayerRevived -= OnRevived;
        GameEvents.OnPlayerDied -= OnDied;
    }

    protected void Update()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
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
            case PlayerState.Die:
                _animator.SetTrigger(HashDie);
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
            case PlayerState.Die: HandleDie(); break;
        }
    }

    private void HandleIdle()
    {
        if (Mathf.Abs(_playerCtrl.PlayerMovement.Direction.x) > 0.01f)
            OnChangeState(PlayerState.Run);
    }

    private void HandleRun()
    {
        if (Mathf.Abs(_playerCtrl.PlayerMovement.Direction.x) <= 0.01f)
            OnChangeState(PlayerState.Idle);
    }

    private void HandleDie() { }

    private void HandleJump()
    {
        if (_playerCtrl.PlayerMovement.VerticalVelocity < 0.01f)
            OnChangeState(PlayerState.Drop);
    }

    private void HandleDrop() { }

    private void HandleLand()
    {
        if (Mathf.Abs(_playerCtrl.PlayerMovement.Direction.x) > 0.01f)
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
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_currentState == PlayerState.Die) return;
        if (_currentState == PlayerState.Dash || _currentState == PlayerState.Shoot) return;
        OnChangeState(PlayerState.Aim);
    }

    private void OnShoot()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_currentState != PlayerState.Aim) return;
        OnChangeState(PlayerState.Shoot);
    }

    private void HandleShoot()
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.8f)
        {
            if (Mathf.Abs(_playerCtrl.PlayerMovement.Direction.x) > 0.01f)
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
            if (Mathf.Abs(_playerCtrl.PlayerMovement.Direction.x) > 0.01f)
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
        if (!_playerCtrl.PhotonView.IsMine) return;
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
            dirX = _playerCtrl.PlayerMovement.Direction.x;

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
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_currentState == PlayerState.Die) return;
        if (_currentState == PlayerState.Aim || _currentState == PlayerState.Shoot) return;
        OnChangeState(PlayerState.Jump);
    }

    public void OnLand()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_currentState == PlayerState.Die) return;
        if (_currentState == PlayerState.Aim || _currentState == PlayerState.Shoot) return;
        if (_currentState == PlayerState.Jump) return;
        OnChangeState(PlayerState.Land);
    }

    private void OnDied(int viewId)
    {
        if (_playerCtrl.PhotonView.ViewID != viewId) return;
        OnChangeState(PlayerState.Die);
    }

    private void OnRevived(int viewId)
    {
        if (_playerCtrl.PhotonView.ViewID != viewId) return;
        _animator.SetTrigger(HashRevive);
        OnChangeState(PlayerState.Idle);
    }
}
