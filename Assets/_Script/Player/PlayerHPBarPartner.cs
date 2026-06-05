using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PlayerHPBarPartner : PlayerHPBar
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _avatarImage;

    private void FixedUpdate()
    {
        if (_damageReceiver != null) return;
        this.InitPartner();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNameText();
        this.LoadCanvasGroup();
        this.LoadAvatarImage();
    }

    private void InitPartner()
    {
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        PlayerCtrl partner = PlayerCtrl.AllPlayers.Find(p => !p.PhotonView.IsMine);
        if (partner == null) return;
        this.BindPartner(partner);
    }

    private void BindPartner(PlayerCtrl partner)
    {
        _damageReceiver = partner.GetComponentInChildren<PlayerDamageReceiver>();
        this.SetPartnerName(partner);
        this.SetPartnerAvatar(partner);
        SetCanvasGroupAlpha(1f);
    }

    private void SetPartnerName(PlayerCtrl partner)
    {
        _nameText.text = partner.PhotonView.Owner.NickName;
    }

    private void SetPartnerAvatar(PlayerCtrl partner)
    {
        if (_avatarImage == null || partner.CharacterData == null) return;
        _avatarImage.sprite = partner.CharacterData.icon;
    }

    private void LoadNameText()
    {
        if (_nameText != null) return;
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void LoadCanvasGroup()
    {
        if (_canvasGroup != null) return;
        _canvasGroup = GetComponent<CanvasGroup>();
        SetCanvasGroupAlpha(0f);
    }

    private void LoadAvatarImage()
    {
        if (_avatarImage != null) return;
        Transform t = transform.Find("Avatar/Mask/AvartarImage");
        if (t != null) _avatarImage = t.GetComponent<Image>();
    }

    protected void SetCanvasGroupAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }
}
