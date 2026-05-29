using UnityEngine;
using UnityEngine.Serialization;

public class ItemDataSO : ScriptableObject
{
    public int _id;
    public string _name;
    public Sprite _icon;
    public TypeItem _typeItem;
    public int _price;
    [FormerlySerializedAs("_equipmentRarity")] public ItemRarity _rarity;
}
