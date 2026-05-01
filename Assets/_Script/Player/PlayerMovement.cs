using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : SaiMonoBehaviour
{

    [Header("Move")]
    [SerializeField] protected InputAction inputAction;
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    public PlayerCtrl PlayerCtrl => _playerCtrl;
    [SerializeField] protected PlayerCtrl _playerCtrl;
    private Vector2 direction;
    public Vector2 Direction => direction;
    public float VerticalVelocity => _rigidbody2D.linearVelocity.y;
    [SerializeField] protected float moveSpeed = 4f;
    private float _lastFacingX = 1f;

    private bool _isDashing;

    [Header("Jump")]
    [SerializeField] protected InputAction _jumpAction;
    [SerializeField] protected float jumpForce = 6f;
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
        this.LoadRigidbody2D();
        this.LoadPhotonView();
        this.LoadPointGroundCheck();
        this.LoadGroundLayer();
    }

    private void LoadPhotonView()
    {
        if (this._photonView != null) return;
        this._photonView = GetComponentInParent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
    }
    private void LoadRigidbody2D()
    {
        if (this._rigidbody2D != null) return;
        this._rigidbody2D = GetComponentInParent<Rigidbody2D>();
        Debug.Log(transform.name + ": Load Rigidbody2D", gameObject);
    }
    private void LoadPlayerCtrl()
    {
        if (this._playerCtrl != null) return;
        this._playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }
    private void LoadPointGroundCheck()
    {
        if (this.pointGroundCheck != null) return;
        this.pointGroundCheck = transform.Find("pointGroundCheck");
        Debug.Log(transform.name + ": Load PointGroundCheck", gameObject);
    }

    private void LoadGroundLayer()
    {
        if (this.groundLayer != 0) return;
        this.groundLayer = LayerMask.GetMask("Ground");
        Debug.Log(transform.name + ": Load GroundLayer", gameObject);
    }
    protected override void Start()
    {
        if (!_photonView.IsMine)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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
        if (!_photonView.IsMine) return;
        SetDirection();
        SetLastFacingX();
        if (_isDashing) return;
        _rigidbody2D.linearVelocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.linearVelocity.y);
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
        _rigidbody2D.linearVelocity = new Vector2(dirX * dashForce, _rigidbody2D.linearVelocity.y);
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
        if (!_photonView.IsMine) return;
        if (!_wasGrounded && _isGrounded) GameEvents.OnPlayerLanded?.Invoke();
    }

    private void TryJump()
    {
        if (!_photonView.IsMine) return;
        if (!_isGrounded && _jumpCount >= maxJumpCount) return;

        _jumpCount++;
        _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);
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
