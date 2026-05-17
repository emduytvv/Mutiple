using UnityEngine;

public enum SkillWeaponRarity
{
    Common = 0,
    Rare = 1,
    Epic = 2,
    Legend = 3
}

public static class SkillRarityToColor
{
    public static Color SkillToColor(this SkillWeaponRarity rarity) => rarity switch
    {
        SkillWeaponRarity.Common => Color.green,
        SkillWeaponRarity.Rare => Color.blue,
        SkillWeaponRarity.Epic => Color.magenta,
        SkillWeaponRarity.Legend => Color.yellow,
        _ => Color.white
    };
}
