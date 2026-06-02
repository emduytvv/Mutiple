using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PlayerHPBarPartner : PlayerHPBar
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void FixedUpdate()
    {
        if (_damageReceiver != null) return;
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        var partner = PlayerCtrl.AllPlayers.Find(p => !p.PhotonView.IsMine);
        if (partner == null) return;
        _damageReceiver = partner.GetComponentInChildren<PlayerDamageReceiver>();
        _nameText.text = partner.PhotonView.Owner.NickName;
        SetCanvasGroupAlpha(1f);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_nameText != null) return;
        _nameText = transform.GetComponentInChildren<TextMeshProUGUI>();

        _canvasGroup = GetComponent<CanvasGroup>();
        SetCanvasGroupAlpha(0f);
    }
    protected void SetCanvasGroupAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha;
    }
}
