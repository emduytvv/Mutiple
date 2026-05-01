using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : SaiMonoBehaviour
{
    [SerializeField] protected Slider slider;
    public Slider Slider => slider;

    protected override void Awake()
    {
        base.Awake();
        slider.onValueChanged.AddListener(OnValueChange);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlider();
    }

    private void LoadSlider()
    {
        if (slider != null) return;
        slider = GetComponent<Slider>();
        Debug.LogWarning(transform.name + ": LoadSlider()", gameObject);
    }

    protected virtual void OnValueChange(float value) { }
}
