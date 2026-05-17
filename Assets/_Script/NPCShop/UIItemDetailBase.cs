using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemDetailBase : SaiMonoBehaviour
{
    [SerializeField] private Image _avatar;
    private Transform[] _statRows;
    private TextMeshProUGUI[] _statLabels;
    private TextMeshProUGUI[] _statValues;
    [SerializeField] protected TextMeshProUGUI _nameItem;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAvatar();
        LoadStatRows();
        LoadNameItem();
    }

    protected void LoadNameItem()
    {
        if (_nameItem != null) return;
        _nameItem = transform.Find("NameItem").GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + ": Load NameItem", gameObject);
    }
    protected void LoadAvatar()
    {
        if (_avatar != null) return;
        _avatar = transform.Find("Avatar").Find("Icon").GetComponent<Image>();
        Debug.Log(transform.name + ": Load Avatar", gameObject);
    }

    protected void LoadStatRows()
    {
        if (_statRows == null || _statRows.Length == 0)
        {
            Transform statRowsParent = transform.Find("StatRows");
            int count = statRowsParent.childCount;
            _statRows = new Transform[count];
            for (int i = 0; i < count; i++)
                _statRows[i] = statRowsParent.GetChild(i);
        }
        _statLabels = new TextMeshProUGUI[_statRows.Length];
        _statValues = new TextMeshProUGUI[_statRows.Length];
        for (int i = 0; i < _statRows.Length; i++)
        {
            _statLabels[i] = _statRows[i].Find("Label").GetComponent<TextMeshProUGUI>();
            _statValues[i] = _statRows[i].Find("Value").GetComponent<TextMeshProUGUI>();
        }
    }
    protected virtual void SetNameItem(ItemInventoryBase item)
    {

        _nameItem.text = item._info._name;
        SetColor(item);
        if (item._info is not WeaponDataSO) return;
        _nameItem.text += " +" + item._currentLevel;
    }
    protected void SetColor(ItemInventoryBase item)
    {
        if (item._info is not EquippableDataSO equip) { _nameItem.color = Color.white; return; }
        Color color = equip._equipmentRarity.EquipmentToColor();
        _nameItem.color = color;
    }
    protected void SetAvatar(ItemInventoryBase item)
    {
        _avatar.sprite = item._info._icon;
    }

    protected void ShowWeapon(WeaponDataSO weapon, int level)
    {
        WeaponLevelData _data = weapon._levels[level];

        SetRow(0, "PhysicalDamage", _data._physicalDamageBonus.ToString());
        SetRow(1, "MagicalDamage", _data._magicalDamageBonus.ToString());
        SetRow(2, "CriticalRate", $"{_data._criticalRate:P0}");
        SetRow(3, "ArmorPenetration", $"{_data._armorPenetration:P0}");
        //SetRow SpecialSkill
        int _length = weapon._skills.Length;
        for (int i = 0; i < _length; i++)
        {
            Color color = weapon._skills[i]._rarity.SkillToColor();
            SetRow(i + 4, weapon._skills[i]._description, "", color);
        }
        SetActiveRows(4 + _length);
    }
    protected void ShowEquipment(EquipmentDataSO eq)
    {
        int amount = 0;
        if (eq._physicalDefense != 0)
        {
            amount++;
            SetRow(amount - 1, "PhysicalDefense", eq._physicalDefense.ToString());
        }
        if (eq._magicalDefense != 0)
        {
            amount++;
            SetRow(amount - 1, "MagicalDefense", eq._magicalDefense.ToString());
        }
        if (eq._hp != 0)
        {
            amount++;
            SetRow(amount - 1, "HP", eq._hp.ToString());
        }
        SetActiveRows(amount);
    }
    protected void ShowPowerUp(PowerUpDataSO pu)
    {
        SetRow(0, "Value", pu._value.ToString());
        string dur = pu._duration <= 0 ? "Permanent" : $"{pu._duration}s";
        SetRow(1, "Duration", dur);
        SetActiveRows(2);
    }

    protected void SetRow(int index, string label, string value, Color? color = null)
    {
        Color _color = color == null ? Color.white : (Color)color;
        _statLabels[index].text = label;
        _statValues[index].text = value;
        _statLabels[index].color = _color;
        _statValues[index].color = _color;
    }

    protected void SetActiveRows(int activeCount)
    {
        for (int i = 0; i < _statRows.Length; i++)
            _statRows[i].gameObject.SetActive(i < activeCount);
    }
}
