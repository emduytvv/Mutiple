using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UICharacterPanel : Singleton<UICharacterPanel>
{
    // Danh sách 3 UIEquipSlot — tự tìm qua GetComponentsInChildren, không cần gán tay
    [SerializeField] private List<UIEquipSlot> _equipSlots = new List<UIEquipSlot>();

    // Text Value của từng chỉ số — tự load từ StatsPanel hierarchy
    [SerializeField] private TextMeshProUGUI _physDamValueText;
    [SerializeField] private TextMeshProUGUI _magDamValueText;
    [SerializeField] private TextMeshProUGUI _hpValueText;
    [SerializeField] private TextMeshProUGUI _physDefValueText;
    [SerializeField] private TextMeshProUGUI _magDefValueText;
    [SerializeField] private TextMeshProUGUI _critValueText;
    [SerializeField] private TextMeshProUGUI _armorPenValueText;

    // ref tới data thực — không load trong LoadComponents vì player chưa tồn tại lúc Awake
    [SerializeField] private PlayerCtrl _playerCtrl;
    [SerializeField] private EquipmentManager _equipmentManager;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private UIInventoryManager _inventoryUI; // cần để gọi Refresh() sau khi equip/unequip

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEquipSlots();
        LoadInventoryUI();
        LoadStatValueTexts();
    }

    private void LoadEquipSlots()
    {
        if (_equipSlots.Count > 0) return;
        _equipSlots.AddRange(GetComponentsInChildren<UIEquipSlot>());
        Debug.Log(transform.name + ": Load EquipSlots " + _equipSlots.Count, gameObject);
    }

    private void LoadInventoryUI()
    {
        if (_inventoryUI != null) return;
        _inventoryUI = transform.parent.GetComponentInChildren<UIInventoryManager>();
        Debug.Log(transform.name + ": Load UIInventoryManager", gameObject);
    }

    private void LoadStatValueTexts()
    {
        if (_physDamValueText != null) return;
        Transform stats = transform.Find("StatsPanel");
        _physDamValueText = stats.Find("DamePhys/Value").GetComponent<TextMeshProUGUI>();
        _magDamValueText = stats.Find("DameMag/Value").GetComponent<TextMeshProUGUI>();
        _hpValueText = stats.Find("HP/Value").GetComponent<TextMeshProUGUI>();
        _physDefValueText = stats.Find("PhysicalDefense/Value").GetComponent<TextMeshProUGUI>();
        _magDefValueText = stats.Find("MagicalDefense/Value").GetComponent<TextMeshProUGUI>();
        _critValueText = stats.Find("Crit/Value").GetComponent<TextMeshProUGUI>();
        _armorPenValueText = stats.Find("ArmorPen/Value").GetComponent<TextMeshProUGUI>();
    }

    protected override void Start()
    {
        base.LoadComponents();
        LoadPlayerIventory();
        RefreshSlotEquip();
        GameEvents.OnWeaponUpgraded += RefreshStats;
    }

    protected void OnDestroy()
    {
        GameEvents.OnWeaponUpgraded -= RefreshStats;
    }
    // protected void OnEnable()
    // {
    //     RefreshStats();
    // }
    private void LoadPlayerIventory()
    {
        if (_equipmentManager != null) return;
        _playerCtrl = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (_playerCtrl == null) return;
        _equipmentManager = _playerCtrl.EquipmentManager;
        _inventoryManager = _playerCtrl.GetComponentInChildren<InventoryManager>();
        this.RefreshSlotEquip();
    }
    public void RefreshSlotEquip()
    {
        foreach (UIEquipSlot slot in _equipSlots)
        {
            ItemInventoryBase item = _equipmentManager.GetCurrentEquip(slot.SlotType);
            slot.SetItem(item);
        }

        RefreshStats();
        _inventoryUI?.Refresh();
    }
    private void RefreshStats()
    {
        if (_playerCtrl == null) return;
        var sender = _playerCtrl.PlayerDamageSender;
        var receiver = _playerCtrl.PlayerDamageReceiver;

        _physDamValueText.text = ((int)sender.PhysicalDamageTotal).ToString();
        _magDamValueText.text = ((int)sender.MagicalDamageTotal).ToString();
        _hpValueText.text = ((int)receiver.maxHP).ToString();
        _physDefValueText.text = ((int)receiver.PhysicalDefenseTotal).ToString();
        _magDefValueText.text = ((int)receiver.MagicalDefenseTotal).ToString();
        _critValueText.text = Mathf.RoundToInt(sender.CritTotal * 100f) + "%";
        _armorPenValueText.text = Mathf.RoundToInt(sender.ArmorPenTotal * 100f) + "%";
    }
    public void TryEquip(int inventoryIndex)
    {
        ItemInventoryBase item = _inventoryManager.Items[inventoryIndex];
        if (item?._info._typeItem != TypeItem.Equipment) return;

        EquipType equipType = ((EquippableDataSO)item._info)._equipType;
        ItemInventoryBase itemPrevious = _equipmentManager.Equip(equipType, item);

        _inventoryManager.Items[inventoryIndex] = itemPrevious;
        this.RefreshSlotEquip();
    }
    // public void TryUnequip(EquipType slot)
    // {
    //     // LoadPlayerIventory();
    //     if (_equipmentManager == null || _inventoryManager == null) return;

    //     ItemBase item = _equipmentManager.GetCurrentEquip(slot);
    //     if (item == null) return; // slot đang trống thì không làm gì

    //     _equipmentManager.Unequip(slot);          // tháo ra (= Equip null)
    //     _inventoryManager.FindSlotFirstEmpty(item); // trả item về ô trống đầu tiên trong inventory
    //     _inventoryUI?.Refresh();
    //     this.Refresh();
    // }
}
