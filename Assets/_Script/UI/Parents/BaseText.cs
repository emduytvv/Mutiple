using TMPro;
using UnityEngine;

public abstract class BaseText : SaiMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;
    public TextMeshProUGUI Text => _text;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTextMeshProUGUI();
    }

    private void LoadTextMeshProUGUI()
    {
        if (_text != null) return;
        _text = GetComponent<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + ": LoadTextMeshProUGUI()", gameObject);
    }

    protected virtual void Update()
    {
        ShowText();
    }

    protected abstract void ShowText();
}
