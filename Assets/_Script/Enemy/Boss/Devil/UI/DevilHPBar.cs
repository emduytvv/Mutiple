using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DevilHPBar : Singleton<DevilHPBar>
{
    [SerializeField] private Slider _physSlider;
    [SerializeField] private Slider _magSlider;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private CanvasGroup _canvasGroup;
    private DevilDamageReceiver _damageReceiver;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlider();
        LoadCanvasGroup();
        LoadNameText();
    }

    private void LoadNameText()
    {
        if (_nameText != null) return;
        _nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
    }


    private void LoadCanvasGroup()
    {
        if (_canvasGroup != null) return;
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }


    private void LoadSlider()
    {
        if (_physSlider != null) return;
        _physSlider = transform.Find("PhysHPBar").GetComponent<Slider>();
        _magSlider = transform.Find("MagHPBar").GetComponent<Slider>();
    }


    private void LateUpdate()
    {
        UpdateHP();
    }

    public void SetOwnerBoss(DevilDamageReceiver damageReceiver)
    {
        _damageReceiver = damageReceiver;
        Show();
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
    }

    private void UpdateHP()
    {
        if (_damageReceiver == null) return;
        UpdatePhysHP();
        UpdateMagHP();
        CheckBothDead();
    }

    private void CheckBothDead()
    {
        if (_physSlider.value > 0f || _magSlider.value > 0f) return;
        Hide();
    }

    private void UpdatePhysHP()
    {
        _physSlider.value = _damageReceiver.PhysCurrentHP / _damageReceiver.PhysMaxHP;
    }

    private void UpdateMagHP()
    {
        _magSlider.value = _damageReceiver.MagCurrentHP / _damageReceiver.MagMaxHP;
    }
}
