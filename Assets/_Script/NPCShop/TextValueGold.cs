using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class TextValueGold : BaseText
{
    protected PlayerGold _playerGold;
    private void LoadPlayerGold()
    {
        var player = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (player == null) return;                                    // ← trả về, thử lại frame sau
        _playerGold = player.GetComponentInChildren<PlayerGold>();
    }

    protected override void ShowText()
    {
        if (_playerGold == null) LoadPlayerGold();
        if (_playerGold == null) return;
        string gold = _playerGold.CurrentGold.ToString();
        _text.text = gold;
    }

}
