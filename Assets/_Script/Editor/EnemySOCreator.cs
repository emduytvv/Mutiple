using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemySOCreator
{
    private struct EnemyData
    {
        public string name;
        public float hp, physDef, magDef, physAtk, magAtk, moveSpeed, cooldown, goldRate;
    }

    [MenuItem("Tools/Create Enemy Stats SOs")]
    static void CreateAll()
    {
        const string folder = "Assets/Resources/EnemyStats";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var goldDrop = Resources.Load<ItemDropSO>("ItemDrop/Gold");
        if (goldDrop == null)
            Debug.LogWarning("EnemySOCreator: Không tìm thấy Gold ItemDropSO tại Resources/ItemDrop/Gold");

        EnemyData[] enemies =
        {
            new() { name = "BatOrange",       hp = 30,  physDef = 0,  magDef = 0,  physAtk = 8,  magAtk = 0,  moveSpeed = 3.5f, cooldown = 1.5f, goldRate = 80  },
            new() { name = "BatBrown",        hp = 45,  physDef = 3,  magDef = 3,  physAtk = 10, magAtk = 0,  moveSpeed = 3.5f, cooldown = 1.3f, goldRate = 100 },
            new() { name = "BatPink",         hp = 40,  physDef = 0,  magDef = 8,  physAtk = 10, magAtk = 8,  moveSpeed = 4.0f, cooldown = 1.2f, goldRate = 100 },
            new() { name = "BatPurple",       hp = 55,  physDef = 5,  magDef = 5,  physAtk = 14, magAtk = 0,  moveSpeed = 3.5f, cooldown = 1.0f, goldRate = 150 },
            new() { name = "BatBlack",        hp = 70,  physDef = 10, magDef = 8,  physAtk = 16, magAtk = 0,  moveSpeed = 4.0f, cooldown = 0.8f, goldRate = 200 },
            new() { name = "BatExplosion",    hp = 20,  physDef = 0,  magDef = 0,  physAtk = 40, magAtk = 0,  moveSpeed = 5.0f, cooldown = 0.5f, goldRate = 120 },
            new() { name = "MagicMini_Pink",  hp = 45,  physDef = 0,  magDef = 10, physAtk = 0,  magAtk = 12, moveSpeed = 2.5f, cooldown = 2.0f, goldRate = 120 },
            new() { name = "MagicMini_Brown", hp = 60,  physDef = 0,  magDef = 12, physAtk = 0,  magAtk = 15, moveSpeed = 2.5f, cooldown = 1.8f, goldRate = 150 },
            new() { name = "MagicMini_Green", hp = 75,  physDef = 5,  magDef = 15, physAtk = 0,  magAtk = 18, moveSpeed = 3.0f, cooldown = 1.5f, goldRate = 180 },
            new() { name = "WandererMagican", hp = 60,  physDef = 5,  magDef = 10, physAtk = 0,  magAtk = 15, moveSpeed = 2.0f, cooldown = 5.0f, goldRate = 150 },
            new() { name = "Sniper",          hp = 50,  physDef = 0,  magDef = 5,  physAtk = 20, magAtk = 0,  moveSpeed = 1.5f, cooldown = 5.0f, goldRate = 180 },
            new() { name = "Satyr_Brown",     hp = 75,  physDef = 10, magDef = 5,  physAtk = 15, magAtk = 0,  moveSpeed = 2.5f, cooldown = 1.5f, goldRate = 200 },
            new() { name = "Satyr_Brown_2",   hp = 100, physDef = 15, magDef = 8,  physAtk = 20, magAtk = 0,  moveSpeed = 2.5f, cooldown = 1.2f, goldRate = 250 },
            new() { name = "Satyr_Brown_3",   hp = 130, physDef = 20, magDef = 12, physAtk = 25, magAtk = 0,  moveSpeed = 3.0f, cooldown = 1.0f, goldRate = 300 },
            new() { name = "SlimeBlue",       hp = 50,  physDef = 0,  magDef = 5,  physAtk = 0,  magAtk = 10, moveSpeed = 0.0f, cooldown = 3.0f, goldRate = 150 },
            new() { name = "SlimeCarrot",     hp = 65,  physDef = 5,  magDef = 8,  physAtk = 0,  magAtk = 13, moveSpeed = 0.0f, cooldown = 2.5f, goldRate = 200 },
            new() { name = "SlimeGreen",      hp = 80,  physDef = 8,  magDef = 10, physAtk = 0,  magAtk = 16, moveSpeed = 0.0f, cooldown = 2.0f, goldRate = 250 },
        };

        int created = 0;
        foreach (var e in enemies)
        {
            string path = $"{folder}/{e.name}.asset";
            if (File.Exists(path))
            {
                Debug.Log($"Skip {e.name} — đã tồn tại");
                continue;
            }

            var so = ScriptableObject.CreateInstance<EnemyStatsSO>();
            so._baseMaxHP = e.hp;
            so._physicalDefense = e.physDef;
            so._magicalDefense = e.magDef;
            so._physicalAttack = e.physAtk;
            so._magicalAttack = e.magAtk;
            so._moveSpeed = e.moveSpeed;
            so._cooldown = e.cooldown;

            if (goldDrop != null)
                so.itemDrops = new List<TableItemDrop> { new() { item = goldDrop, _rate = e.goldRate } };

            AssetDatabase.CreateAsset(so, path);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"EnemySOCreator: Tạo xong {created} SO mới tại {folder}");
    }
}
