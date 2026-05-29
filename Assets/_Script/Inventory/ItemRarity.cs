using UnityEngine;

public enum ItemRarity { Common = 0, Rare = 1, Epic = 2, Legend = 3 }

public static class ItemRarityExtension
{
    public static Color ToColor(this ItemRarity r) => r switch
    {
        ItemRarity.Common => Color.green,
        ItemRarity.Rare   => Color.blue,
        ItemRarity.Epic   => Color.magenta,
        ItemRarity.Legend => Color.yellow,
        _                 => Color.white
    };
}
