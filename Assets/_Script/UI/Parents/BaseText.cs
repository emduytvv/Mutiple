using TMPro;
using UnityEngine;

public abstract class BaseText : SaiMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
    public TextMeshProUGUI TextMeshProUGUI => textMeshProUGUI;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTextMeshProUGUI();
    }

    private void LoadTextMeshProUGUI()
    {
        if (textMeshProUGUI != null) return;
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        Debug.LogWarning(transform.name + ": LoadTextMeshProUGUI()", gameObject);
    }

    protected virtual void Update()
    {
        ShowText();
    }

    protected abstract void ShowText();
}
