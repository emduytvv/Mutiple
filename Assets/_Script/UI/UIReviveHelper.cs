using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIReviveHelper : SaiMonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private CanvasGroup _canvasGroup;
    private PlayerReviveHelper _reviveHelper;
    private PlayerCtrl _playerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadFillImage();
        LoadCanvasGroup();
    }

    private void LoadFillImage()
    {
        if (_fillImage != null) return;
        Transform icon = transform.Find("Icon");
        if (icon != null) _fillImage = icon.GetComponent<Image>();
    }

    private void LoadCanvasGroup()
    {
        if (_canvasGroup != null) return;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void FixedUpdate()
    {
        GetPlayerCtrl();
    }

    private void GetPlayerCtrl()
    {
        if (_reviveHelper != null) return;
        PlayerCtrl mine = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (mine == null) return;
        _playerCtrl = mine;
        _reviveHelper = mine.GetComponentInChildren<PlayerReviveHelper>();
    }
    private void Update()
    {
        if (_playerCtrl == null) return;
        UpdateAlpha();
        UpdateFill();
    }

    private void UpdateAlpha()
    {
        _canvasGroup.alpha = _reviveHelper.IsNearDeadPlayer ? 1f : 0f;
        if (_canvasGroup.alpha != 1) return;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = _reviveHelper.PlayerOther.transform.position + 2f * Vector3.up;
    }


    private void UpdateFill()
    {
        _fillImage.fillAmount = Mathf.Clamp01(_reviveHelper.Timer / _reviveHelper.ReviveTime);
    }

}
