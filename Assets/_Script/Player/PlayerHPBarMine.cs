using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBarMine : PlayerHPBar
{
    [SerializeField] private Image _avatarImage;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAvatarImage();
    }

    private void LoadAvatarImage()
    {
        if (_avatarImage != null) return;
        Transform t = transform.Find("Avatar/Mask/AvartarImage");
        if (t != null) _avatarImage = t.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (_damageReceiver != null) return;
        PlayerCtrl mine = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (mine == null) return;
        _damageReceiver = mine.GetComponentInChildren<PlayerDamageReceiver>();
        if (_avatarImage != null && mine.CharacterData != null)
            _avatarImage.sprite = mine.CharacterData.icon;
    }
}
