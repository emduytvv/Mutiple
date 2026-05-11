using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponSkillSO")]
public class WeaponSkillSO : ScriptableObject
{
    public string _skillName;
    public string _description;
    public WeaponSkillType _type;
    public SkillWeaponRarity _skillRarity;
}