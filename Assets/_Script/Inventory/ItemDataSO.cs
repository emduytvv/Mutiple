using UnityEngine;

// Base class — chỉ chứa field CHUNG cho mọi loại item
// Không tạo trực tiếp — dùng EquipmentDataSO hoặc PowerUpDataSO
public class ItemDataSO : ScriptableObject
{
    public int _id;
    public Sprite _icon;
    public TypeItem _typeItem;
}
