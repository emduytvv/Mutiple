using Photon.Pun;
using UnityEngine;

public class EnemyMovement : SaiMonoBehaviour
{
    [SerializeField] protected PhotonView _photonView;
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    [SerializeField] protected EnemyDamageReceiver _damageReceiver;
    [SerializeField] protected float _moveSpeed = 2f;

    private float _timer;
    private float _directionX = -1f;
    private const float _switchInterval = 1f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadRigidbody2D();
        this.LoadDamageReceiver();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
        Debug.Log(transform.name + ": Load Rigidbody2D", gameObject);
    }

    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = transform.parent.GetComponentInChildren<EnemyDamageReceiver>();
        Debug.Log(transform.name + ": Load DamageReceiver", gameObject);
    }

    protected void FixedUpdate()
    {
        if (!_photonView.IsMine) return;
        if (_damageReceiver.isDead) return;
        Move();
    }

    private void Move()
    {
        UpdateDirection();
        _rigidbody2D.linearVelocity = new Vector2(_directionX * _moveSpeed, _rigidbody2D.linearVelocity.y);
    }

    private void UpdateDirection()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer < _switchInterval) return;
        _timer = 0f;
        _directionX *= -1f;
    }
}
