
using System.Collections.Generic;
using UnityEngine;

public class NPCShopData : SaiMonoBehaviour
{
    [SerializeField] protected List<ItemInventoryBase> _shop;
    public List<ItemInventoryBase> Shop => _shop;
    [SerializeField] protected List<EquipmentDataSO> _equipments;
    [SerializeField] protected List<WeaponDataSO> _weapons;
    [SerializeField] protected List<PowerUpDataSO> _powerUps;
    [SerializeField] protected List<WeaponSkillSO> _skills;
    private const int MaxSlot = 6;
    private const int MaxSkillPerWeapon = 3;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponDataSO();
        LoadEquipmentDataSO();
        LoadPowerUpDataSO();
        LoadWeaponSkillSO();
    }
    private void LoadPowerUpDataSO()
    {
        if (_powerUps.Count > 0) return;
        string path = "ItemData/PowerUpData";
        _powerUps.AddRange(Resources.LoadAll<PowerUpDataSO>(path));
        Debug.Log(transform.name + ": Load PowerUpDataSO from " + path, gameObject);
    }
    private void LoadWeaponSkillSO()
    {
        if (_skills.Count > 0) return;
        string path = "WeaponSkill";
        _skills.AddRange(Resources.LoadAll<WeaponSkillSO>(path));
        Debug.Log(transform.name + ": Load WeaponSkillSO from " + path, gameObject);
    }
    private void LoadWeaponDataSO()
    {
        if (_weapons.Count > 0) return;
        string path = "ItemData/WeaponData";
        _weapons.AddRange(Resources.LoadAll<WeaponDataSO>(path));
        Debug.Log(transform.name + ": Load WeaponDataSO from " + path, gameObject);
    }
    private void LoadEquipmentDataSO()
    {
        if (_equipments.Count > 0) return;
        string path = "ItemData/EquipmentData";
        _equipments.AddRange(Resources.LoadAll<EquipmentDataSO>(path));
        Debug.Log(transform.name + ": Load EquipmentDataSO from " + path, gameObject);
    }
    protected override void Start()
    {
        base.Start();
        AddWeapon(_weapons, 1);
        AddRandom(_equipments, 2);
        AddRandom(_powerUps, 3);
    }
    private void AddRandom<T>(List<T> pool, int count) where T : ItemDataSO
    {
        if (pool.Count == 0) return;

        var listFake = new List<T>(pool);
        Shuffle(listFake);

        for (int i = 0; i < count; i++)
        {
            if (_shop.Count >= MaxSlot) break;
            _shop.Add(new ItemInventoryBase { _info = listFake[i], _amount = 1, _currentLevel = 1 });
        }
    }
    private void AddWeapon<T>(List<T> pool, int count) where T : WeaponDataSO
    {
        if (pool.Count == 0) return;

        var listFake = new List<T>(pool);
        Shuffle(listFake);

        for (int i = 0; i < count; i++)
        {
            if (_shop.Count >= MaxSlot) break;
            var copy = ScriptableObject.Instantiate(listFake[i]);
            copy.name = listFake[i].name;
            copy._arrowType = RandomType();
            copy._skills = RandomSkills();
            _shop.Add(new ItemInventoryBase { _info = copy, _amount = 1, _currentLevel = 0 });
        }
    }
    private ArrowType RandomType()
    {
        return (ArrowType)Random.Range(0, System.Enum.GetValues(typeof(ArrowType)).Length);
    }
    private WeaponSkillSO[] RandomSkills()
    {
        List<WeaponSkillSO> skillsFake = new List<WeaponSkillSO>(_skills);
        Shuffle(skillsFake);

        List<WeaponSkillSO> skillsGet = new List<WeaponSkillSO>();
        int count = Random.Range(1, MaxSkillPerWeapon + 1);
        for (int i = 0; i < count; i++)
        {
            skillsGet.Add(skillsFake[i]);
        }
        return skillsGet.ToArray();
    }
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
