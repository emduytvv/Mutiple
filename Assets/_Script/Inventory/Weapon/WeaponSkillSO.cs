using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponSkillSO")]
public class WeaponSkillSO : ScriptableObject
{
    public string _description;
    public WeaponSkillName _name;
    public ItemRarity _rarity;
}