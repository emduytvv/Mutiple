using System;
using UnityEngine;

[Serializable]
public class ItemTransferData
{
    public string soName;
    public int typeItem;
    public int equipType;
    public int amount;
    public int currentLevel;
    public int arrowType;
    public string[] skillSoNames;

    public static string Serialize(ItemInventoryBase item)
    {
        var d = new ItemTransferData
        {
            soName = item._info.name,
            typeItem = (int)item._info._typeItem,
            equipType = item._info is EquippableDataSO eq ? (int)eq._equipType : (int)EquipType.Null,
            amount = item._amount,
            currentLevel = item._currentLevel,
        };

        if (item._info is WeaponDataSO weapon)
        {
            d.arrowType = (int)weapon._arrowType;
            d.skillSoNames = Array.ConvertAll(weapon._skills, s => s.name);
        }

        return JsonUtility.ToJson(d);
    }

    public static ItemInventoryBase Deserialize(string json)
    {
        ItemTransferData d = JsonUtility.FromJson<ItemTransferData>(json);
        ItemDataSO so = LoadSO(d);
        if (so == null)
        {
            Debug.LogError($"[ItemTransferData] Cannot find SO '{d.soName}'");
            return null;
        }
        return new ItemInventoryBase { _info = so, _amount = d.amount, _currentLevel = d.currentLevel };
    }

    private static ItemDataSO LoadSO(ItemTransferData d)
    {
        TypeItem type = (TypeItem)d.typeItem;
        EquipType equip = (EquipType)d.equipType;

        if (type == TypeItem.Equipment && equip == EquipType.Weapon)
        {
            WeaponDataSO baseWeapon = Resources.Load<WeaponDataSO>("ItemData/WeaponData/" + d.soName);
            if (baseWeapon == null) return null;
            WeaponDataSO copy = ScriptableObject.Instantiate(baseWeapon);
            copy.name = baseWeapon.name;
            copy._arrowType = (ArrowType)d.arrowType;
            copy._skills = LoadSkills(d.skillSoNames);
            return copy;
        }

        if (type == TypeItem.Equipment)
            return Resources.Load<EquipmentDataSO>("ItemData/EquipmentData/" + d.soName);

        return Resources.Load<PowerUpDataSO>("ItemData/PowerUpData/" + d.soName);
    }

    private static WeaponSkillSO[] LoadSkills(string[] names)
    {
        if (names == null || names.Length == 0) return new WeaponSkillSO[0];
        var result = new WeaponSkillSO[names.Length];
        for (int i = 0; i < names.Length; i++)
            result[i] = Resources.Load<WeaponSkillSO>("WeaponSkill/" + names[i]);
        return result;
    }
}
