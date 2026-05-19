using Photon.Pun;
using UnityEngine;

public class EnemyCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public EnemyDamageReceiver DamageReceiver => _damageReceiver;
    [SerializeField] protected EnemyDamageReceiver _damageReceiver;
    public EnemyDamageSender EnemyDamageSender => _enemyDamageSender;
    [SerializeField] protected EnemyDamageSender _enemyDamageSender;
    public EnemyAnimation EnemyAnimation => _enemyAnimation;
    [SerializeField] protected EnemyAnimation _enemyAnimation;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    public EnemyDespawn EnemyDespawn => _enemyDespawn;
    [SerializeField] protected EnemyDespawn _enemyDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadDamageReceiver();
        this.LoadEnemyAnimation();
        this.LoadRigidbody2D();
        this.LoadEnemyDespawn();
        LoadEnemyDamageSender();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }

    private void LoadDamageReceiver()
    {
        if (_damageReceiver != null) return;
        _damageReceiver = GetComponentInChildren<EnemyDamageReceiver>();
    }
    private void LoadEnemyDamageSender()
    {
        if (_enemyDamageSender != null) return;
        _enemyDamageSender = GetComponentInChildren<EnemyDamageSender>();
    }

    private void LoadEnemyAnimation()
    {
        if (_enemyAnimation != null) return;
        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void LoadEnemyDespawn()
    {
        if (_enemyDespawn != null) return;
        _enemyDespawn = GetComponentInChildren<EnemyDespawn>();
    }

    [PunRPC]
    public void RpcReceive(float physDamage, float magDamage, float armorPen)
    {
        _damageReceiver.Receiver(physDamage, magDamage, armorPen);
    }
}
