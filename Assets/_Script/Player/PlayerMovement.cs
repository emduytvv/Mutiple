using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : SaiMonoBehaviour
{
    [Header("Move")]
    [SerializeField] protected InputAction inputAction;
    [SerializeField] protected PlayerCtrl _playerCtrl;
    private Vector2 direction;
    public Vector2 Direction => direction;
    public float VerticalVelocity => _playerCtrl.Rigidbody2D.linearVelocity.y;
    [SerializeField] protected float moveSpeed = 4f;
    private float _lastFacingX = 1f;
    private bool _isDashing;

    [Header("Jump")]
    [SerializeField] protected InputAction _jumpAction;
    [SerializeField] protected float jumpForce = 6.2f;
    [SerializeField] protected Transform pointGroundCheck;
    [SerializeField] protected Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected bool _isGrounded;
    [SerializeField] protected bool _wasGrounded;
    [SerializeField] protected int maxJumpCount = 1;
    [SerializeField] protected int _jumpCount;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
        this.LoadPointGroundCheck();
        this.LoadGroundLayer();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    private void LoadPointGroundCheck()
    {
        if (pointGroundCheck != null) return;
        pointGroundCheck = transform.Find("pointGroundCheck");
        Debug.Log(transform.name + ": Load PointGroundCheck", gameObject);
    }

    private void LoadGroundLayer()
    {
        if (groundLayer != 0) return;
        groundLayer = LayerMask.GetMask("Ground");
        Debug.Log(transform.name + ": Load GroundLayer", gameObject);
    }

    protected override void Start()
    {
        if (!_playerCtrl.PhotonView.IsMine)
        {
            _playerCtrl.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            return;
        }
        SetInputAction();
    }

    private void SetInputAction()
    {
        this.inputAction.Enable();
        this._jumpAction.Enable();
        this._jumpAction.performed += _ => TryJump();
        GameEvents.OnPlayerDashed += OnDash;
        GameEvents.OnPlayerDashEnded += OnDashEnded;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerDashed -= OnDash;
        GameEvents.OnPlayerDashEnded -= OnDashEnded;
    }

    protected void FixedUpdate()
    {
        Move();
        UpdateGrounded();
    }

    private void Move()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_playerCtrl.PlayerAnimation.CurrentState == PlayerState.Die) return;
        SetDirection();
        SetLastFacingX();
        if (_isDashing) return;
        _playerCtrl.Rigidbody2D.linearVelocity = new Vector2(direction.x * moveSpeed, _playerCtrl.Rigidbody2D.linearVelocity.y);
    }

    private void SetDirection()
    {
        direction = inputAction.ReadValue<Vector2>();
    }

    private void SetLastFacingX()
    {
        if (Mathf.Abs(direction.x) > 0.01f) _lastFacingX = Mathf.Sign(direction.x);
    }

    private void OnDash(float dashForce)
    {
        float dirX = Mathf.Abs(direction.x) > 0.01f ? Mathf.Sign(direction.x) : _lastFacingX;
        _playerCtrl.Rigidbody2D.linearVelocity = new Vector2(dirX * dashForce, _playerCtrl.Rigidbody2D.linearVelocity.y);
        _isDashing = true;
    }

    private void OnDashEnded()
    {
        _isDashing = false;
    }

    private void UpdateGrounded()
    {
        _wasGrounded = _isGrounded;
        _isGrounded = Physics2D.OverlapBox(pointGroundCheck.position, groundCheckSize, 0, groundLayer);

        if (_isGrounded) _jumpCount = 0;
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (!_wasGrounded && _isGrounded) GameEvents.OnPlayerLanded?.Invoke();
    }

    private void TryJump()
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        if (_playerCtrl.PlayerAnimation.CurrentState == PlayerState.Die) return;
        if (!_isGrounded && _jumpCount >= maxJumpCount) return;

        _jumpCount++;
        _playerCtrl.Rigidbody2D.linearVelocity = new Vector2(_playerCtrl.Rigidbody2D.linearVelocity.x, jumpForce);
        GameEvents.OnPlayerJumped?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (pointGroundCheck == null) return;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(pointGroundCheck.position, groundCheckSize);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        SetKeyMove();
        SetKetJump();
    }

    private void SetKetJump()
    {
        inputAction = new InputAction("Move");
        inputAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }

    private void SetKeyMove()
    {
        _jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");
    }
}
