using System.Collections.Generic;
using UnityEngine;

public class NPCShopData : SaiMonoBehaviour
{
    [SerializeField] protected List<EquipmentDataSO> _equipments;
    [SerializeField] protected List<WeaponDataSO> _weapons;
    [SerializeField] protected List<PowerUpDataSO> _powerUps;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadWeaponDataSO();
        LoadEquipmentDataSO();
        LoadPowerUpDataSO();
    }

    private void LoadPowerUpDataSO()
    {
        if (_powerUps.Count > 0) return;
        string path = "ItemData/PowerUpData";
        _powerUps.AddRange(Resources.LoadAll<PowerUpDataSO>(path));
        Debug.Log(transform.name + ": Load PowerUpDataSO from " + path, gameObject);
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
}