using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChestSkillSlot : SaiMonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private IntrinsicSkillSO _skill;
    private UIChestSkillPanel _panel;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadIcon();
        LoadNameText();
        LoadDescriptionText();
    }

    private void LoadIcon()
    {
        if (_icon != null) return;
        _icon = transform.Find("Icon")?.GetComponent<Image>();
        Debug.Log(transform.name + ": Load Icon", gameObject);
    }

    private void LoadNameText()
    {
        if (_nameText != null) return;
        _nameText = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + ": Load NameText", gameObject);
    }

    private void LoadDescriptionText()
    {
        if (_descriptionText != null) return;
        _descriptionText = transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + ": Load DescriptionText", gameObject);
    }

    public void SetSkill(IntrinsicSkillSO skill, UIChestSkillPanel panel)
    {
        _skill = skill;
        _panel = panel;

        if (_icon != null) _icon.sprite = skill._icon;
        if (_nameText != null) _nameText.text = skill._name.ToString();
        if (_descriptionText != null) _descriptionText.text = skill._description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _panel?.OnSlotClicked(_skill);
    }
}
