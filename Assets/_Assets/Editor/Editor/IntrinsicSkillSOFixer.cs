using UnityEditor;
using UnityEngine;

public class IntrinsicSkillSOFixer
{
    [MenuItem("Tools/Fix IntrinsicSkill SO Data")]
    public static void FixAll()
    {
        Fix("AutoShield",          1, IntrinsicSkillName.AutoShield,          "5s → tạo khiên hấp thụ 1 đòn");
        Fix("BuffArmorPenetration", 2, IntrinsicSkillName.BuffArmorPenetration, "Tăng 10% xuyên giáp");
        Fix("BuffCritical",         3, IntrinsicSkillName.BuffCritical,         "Tăng 10% tỉ lệ chí mạng");
        Fix("BuffDamageMagical",    4, IntrinsicSkillName.BuffDamageMagical,    "Tăng 10 damage phép thuật");
        Fix("BuffDamagePhysical",   5, IntrinsicSkillName.BuffDamagePhysical,   "Tăng 10 damage vật lý");
        Fix("BuffHP",               6, IntrinsicSkillName.BuffHP,               "Tăng 100 HP tối đa");
        Fix("BuffPercentHP",        7, IntrinsicSkillName.BuffPercentHP,        "Tăng 10% HP tối đa");
        Fix("DashHeal",             8, IntrinsicSkillName.DashHeal,             "Mỗi lần dash → hồi 3% HP");
        Fix("LastStand",            9, IntrinsicSkillName.LastStand,            "HP < 20% → damage +40%");
        Fix("ReviveBurst",         10, IntrinsicSkillName.ReviveBurst,          "Sau khi revive → damage +50% trong 15s");

        AssetDatabase.SaveAssets();
        Debug.Log("IntrinsicSkill SO data fixed!");
    }

    private static void Fix(string assetName, int id, IntrinsicSkillName name, string description)
    {
        var path = $"Assets/Resources/IntrinsicSkill/{assetName}.asset";
        var so = AssetDatabase.LoadAssetAtPath<IntrinsicSkillSO>(path);
        if (so == null) { Debug.LogWarning($"Not found: {path}"); return; }

        so._id = id;
        so._name = name;
        so._description = description;
        EditorUtility.SetDirty(so);
    }
}
