using UnityEngine;

// Base class — chỉ chứa field CHUNG cho mọi loại item
// Không tạo trực tiếp — dùng EquipmentDataSO hoặc PowerUpDataSO
public class EquippableDataSO : ItemDataSO
{
    public EquipType _equipType;
    public EquipmentRarity _equipmentRarity;
}
