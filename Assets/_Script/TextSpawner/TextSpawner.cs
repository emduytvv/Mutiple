using UnityEngine;

public class TextSpawner : Spawner
{
    private static TextSpawner _instance;
    public static TextSpawner Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 100;
    }

    public void SpawnText(Vector3 position, float physDamage, float magDamage)
    {
        Transform text = SpawnByName("TextDamage", position, Quaternion.identity);
        text.gameObject.SetActive(true);
        TextDamageCtrl ctrl = text.GetComponent<TextDamageCtrl>();

        ctrl.TextPhys.gameObject.SetActive(physDamage > 0);
        ctrl.TextMagic.gameObject.SetActive(magDamage > 0);

        if (physDamage > 0) ctrl.TextPhys.text = physDamage.ToString();
        if (magDamage > 0) ctrl.TextMagic.text = magDamage.ToString();
    }
    public Transform SpawnTextDefault(Vector3 position)
    {
        Transform text = SpawnByName("TextDefault", position, Quaternion.identity);
        text.gameObject.SetActive(true);
        return text;
    }
}
