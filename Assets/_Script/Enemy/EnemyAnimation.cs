using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAnimation : SaiMonoBehaviour
{
    [SerializeField] protected PhotonView _photonView;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected EnemyDamageReceiver _damageReceiver;
    [SerializeField] protected Rigidbody2D _rigidbody2D;

    static readonly int HashDie = Animator.StringToHash("die");
    static readonly int HashHurt = Animator.StringToHash("isHurt");
    private bool _dieTriggered;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadAnimator();
        this.LoadDamageReceiver();
        this.LoadRigidbody2D();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
    }

    private void LoadAnimator()
    {
        if (_animator != null) return;
        _animator = GetComponent<Animator>();
        Debug.Log(transform.name + ": Load Animator", gameObject);
    }

    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = GetComponentInChildren<EnemyDamageReceiver>();
        Debug.Log(transform.name + ": Load DamageReceiver", gameObject);
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Debug.Log(transform.name + ": Load Rigidbody2D", gameObject);
    }

    private void Update()
    {
        if (!_photonView.IsMine) return;

        HandleDeadAnim();
        UpdateFlip();
    }

    public void OnHurt()
    {
        if (!_photonView.IsMine) return;
        Debug.Log(transform.name + ": OnHurt");
        _animator.SetTrigger(HashHurt);
    }

    private void HandleDeadAnim()
    {
        if (_dieTriggered || !_damageReceiver.isDead) return;
        _dieTriggered = true;
        _animator.SetTrigger(HashDie);
    }

    private void UpdateFlip()
    {
        float velX = _rigidbody2D.linearVelocity.x;
        if (velX > 0.01f) SetFacing(true);
        else if (velX < -0.01f) SetFacing(false);
    }

    private void SetFacing(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
