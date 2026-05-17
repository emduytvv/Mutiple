using UnityEngine;
using TMPro;

public class TextDamageCtrl : SaiMonoBehaviour
{
    [SerializeField] private TextMeshPro _textPhys;
    [SerializeField] private TextMeshPro _textMagic;
    public TextMeshPro TextPhys => _textPhys;
    public TextMeshPro TextMagic => _textMagic;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_textPhys == null) _textPhys = transform.Find("TextPhys")?.GetComponent<TextMeshPro>();
        if (_textMagic == null) _textMagic = transform.Find("TextMagic")?.GetComponent<TextMeshPro>();
    }
}