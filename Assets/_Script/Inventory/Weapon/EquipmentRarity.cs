using UnityEngine;

public enum EquipmentRarity
{
    Common = 0,
    Rare = 1,
    Epic = 2,
    Legend = 3
}
public static class EquipmentRarityToColor
{
    public static Color EquipmentToColor(this EquipmentRarity rarity) => rarity switch
    {
        EquipmentRarity.Common => Color.green,
        EquipmentRarity.Rare   => Color.blue,
        EquipmentRarity.Epic   => Color.magenta,
        EquipmentRarity.Legend => Color.yellow,
        _                      => Color.white
    };
}