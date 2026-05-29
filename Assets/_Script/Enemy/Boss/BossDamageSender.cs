using Photon.Pun;
using UnityEngine;

public class BossDamageSender : DamageSender
{
    public void Send(PlayerCtrl player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        player.PhotonView.RPC("RpcReceive", RpcTarget.All, basePhysicalDamage, baseMagicalDamage);
    }
}
