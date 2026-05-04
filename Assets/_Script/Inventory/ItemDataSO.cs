using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "SO/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public int _id;
    public Sprite _icon;
    public TypeItem _typeItem;
}
