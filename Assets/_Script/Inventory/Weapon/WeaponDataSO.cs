using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "SO/WeaponDataSO")]
public class WeaponDataSO : EquippableDataSO
{
    public WeaponLevelData[] _levels;
    public WeaponSkillSO[] _skills;
}
