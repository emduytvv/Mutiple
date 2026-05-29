using Photon.Pun;
using UnityEngine;

public class DevilExplosionDamageSender : DamageSender
{
    [SerializeField] private float _radius = 2f;
    [SerializeField] private LayerMask _playerLayer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerLayer();
    }

    private void LoadPlayerLayer()
    {
        if (_playerLayer != 0) return;
        _playerLayer = LayerMask.GetMask("Player");
    }
    public void Send()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius, _playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerDamageReceiver player = hit.GetComponent<PlayerDamageReceiver>();
            if (player == null) continue;
            PlayerCtrl playerCtrl = hit.GetComponentInParent<PlayerCtrl>();
            playerCtrl.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
