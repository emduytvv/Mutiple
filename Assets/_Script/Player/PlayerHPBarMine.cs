using System;
using UnityEngine;

public class PlayerHPBarMine : PlayerHPBar
{

    private void FixedUpdate()
    {
        if (_damageReceiver != null) return;
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        _damageReceiver = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine).GetComponentInChildren<PlayerDamageReceiver>();
    }
}
