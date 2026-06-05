using UnityEngine;
using UnityEngine.UI;

public class UIDash : SaiMonoBehaviour
{
    [SerializeField] private Image _fillImage;
    private PlayerAbilityDash _abilityDash;
    private float _elapsed;
    private float _duration;
    private bool _isFilling;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadFillImage();
    }

    private void LoadFillImage()
    {
        if (_fillImage != null) return;
        Transform icon = transform.Find("Icon");
        if (icon != null) _fillImage = icon.GetComponent<Image>();
    }

    private void OnEnable() => GameEvents.OnPlayerDashed += OnDashed;
    private void OnDisable() => GameEvents.OnPlayerDashed -= OnDashed;

    private void FixedUpdate()
    {
        if (_abilityDash != null) return;
        PlayerCtrl mine = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (mine == null) return;
        _abilityDash = mine.GetComponentInChildren<PlayerAbilityDash>();
    }

    private void Update()
    {
        if (!_isFilling) return;
        _elapsed += Time.deltaTime;
        _fillImage.fillAmount = Mathf.Clamp01(_elapsed / _duration);
        if (_elapsed >= _duration) _isFilling = false;
    }

    private void OnDashed(float _)
    {
        if (_fillImage == null || _abilityDash == null) return;
        _elapsed = 0f;
        _duration = _abilityDash.Cooldown;
        _fillImage.fillAmount = 0f;
        _isFilling = true;
    }
}
