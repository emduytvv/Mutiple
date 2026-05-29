using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpDataSO", menuName = "SO/PowerUpDataSO")]
public class PowerUpDataSO : ItemDataSO
{
    public PowerUpEffectName _effectName;
    public float _value;
    public float _duration; // 0 = instant
    [TextArea(2, 4)] public string _description;
}
