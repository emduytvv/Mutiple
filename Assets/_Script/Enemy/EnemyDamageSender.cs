using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class EnemyDamageSender : DamageSender
{
    public void Send(PlayerCtrl player, float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        player.PhotonView.RPC("RpcReceive", RpcTarget.All, damage);
    }
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     PlayerDamageReceiver player = collision.GetComponent<PlayerDamageReceiver>();
    //     if (player == null) return;
    //     if (!_photonView.IsMine) return;
    //     PlayerCtrl playerCtrl = player.GetComponentInParent<PlayerCtrl>();
    //     playerCtrl.PhotonView.RPC("RpcReceive", RpcTarget.All, 4f);
    // }


}
