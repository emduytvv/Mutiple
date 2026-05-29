using Photon.Pun;
using UnityEngine;

public abstract class Movement : SaiMonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] private PhotonView _photonView;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
    }

    protected virtual void FixedUpdate()
    {
        if (_photonView != null && !_photonView.IsMine) return;
        Move();
    }

    protected abstract void Move();

    public virtual void ApplyMultiplier(float multiplier) { }
}
