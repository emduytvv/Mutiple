using Photon.Pun;
using UnityEngine;

public class DevilExplosionCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] private PhotonView _photonView;

    public DevilExplosionAnimation Animation => _animation;
    [SerializeField] private DevilExplosionAnimation _animation;

    public DevilExplosionDamageSender DamageSender => _damageSender;
    [SerializeField] private DevilExplosionDamageSender _damageSender;

    public DevilExplosionDespawn Despawn => _despawn;
    [SerializeField] private DevilExplosionDespawn _despawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadAnimation();
        this.LoadDamageSender();
        this.LoadDespawn();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }

    private void LoadAnimation()
    {
        if (_animation != null) return;
        _animation = GetComponentInChildren<DevilExplosionAnimation>();
    }

    private void LoadDamageSender()
    {
        if (_damageSender != null) return;
        _damageSender = GetComponentInChildren<DevilExplosionDamageSender>();
    }

    private void LoadDespawn()
    {
        if (_despawn != null) return;
        _despawn = GetComponentInChildren<DevilExplosionDespawn>();
    }
}
