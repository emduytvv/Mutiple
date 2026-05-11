using UnityEngine;

// Component gắn trên Player — lưu trạng thái 3 ô trang bị đang đeo
public class EquipmentManager : SaiMonoBehaviour
{
    [SerializeField] private ItemInventoryBase[] _equipped;
    protected override void Awake()
    {
        _equipped = new ItemInventoryBase[3];
    }
    public ItemInventoryBase GetCurrentEquip(EquipType equipType)
    {
        return _equipped[(int)equipType];//== null ? null : _equipped[(int)equipType];
    }
    public ItemInventoryBase Equip(EquipType equipType, ItemInventoryBase item)
    {
        ItemInventoryBase previous = _equipped[(int)equipType];
        _equipped[(int)equipType] = item;
        return previous;
    }
    // public ItemBase Unequip(EquipType equipType) => Equip(equipType, null);

    // Trả về bonus của 1 chỉ số cụ thể từ tất cả trang bị đang đeo
    // public int GetBonus(EquipType statType)
    // {
    //     // int total = 0;
    //     // foreach (ItemBase item in _equipped)
    //     // {
    //     //     if (item?._info is not EquipmentDataSO equip) continue;
    //     //     total += statType switch
    //     //     {
    //     //         EquipType.Weapon => equip._attackBonus,
    //     //         EquipType.Armor  => equip._defenseBonus,
    //     //         EquipType.Pants  => equip._hpBonus,
    //     //         _ => 0
    //     //     };
    //     // }
    //     // return total;
    // }
}
