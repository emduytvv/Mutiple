using UnityEngine;
using TMPro;

public class TextDefaultCtrl : SaiMonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_text != null) return;
        _text = transform.GetComponentInChildren<TextMeshPro>();
    }
    public void SetText(string content)
    {
        _text.text = content;
    }
}