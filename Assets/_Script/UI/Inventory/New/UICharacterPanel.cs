using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Panel nhân vật: hiển thị 3 ô trang bị + chỉ số Attack/HP/Defense/Speed
// Singleton vì UIEquipSlot và UIItemContextMenu cần gọi vào đây qua .Instance
public class UICharacterPanel : Singleton<UICharacterPanel>
{
    // Danh sách 3 UIEquipSlot — tự tìm qua GetComponentsInChildren, không cần gán tay
    [SerializeField] private List<UIEquipSlot> _equipSlots = new List<UIEquipSlot>();

    // 4 ô text hiển thị số chỉ số — gán trong Inspector
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _defenseText;
    [SerializeField] private TextMeshProUGUI _speedText;

    // const: giá trị cố định tại compile-time, là chỉ số gốc trước khi cộng bonus trang bị
    private const int BaseAttack = 100;
    private const int BaseHP = 100;
    private const int BaseDefense = 100;
    private const int BaseSpeed = 100;

    // 3 ref tới data thực — không load trong LoadComponents vì player chưa tồn tại lúc Awake
    [SerializeField] private EquipmentManager _equipmentManager;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private UIInventoryManager _inventoryUI; // cần để gọi Refresh() sau khi equip/unequip

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEquipSlots();
        LoadInventoryUI();
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

    protected override void Start()
    {
        base.LoadComponents();
        // Invoke(nameof(LoadPlayerIventory), 1f);
        // Invoke(nameof(RefreshSlotEquip), 1f);
        LoadPlayerIventory();
        RefreshSlotEquip();
    }
    private void LoadPlayerIventory()
    {
        if (_equipmentManager != null) return;
        var local = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (local == null) return;
        _equipmentManager = local.GetComponentInChildren<EquipmentManager>();
        _inventoryManager = local.GetComponentInChildren<InventoryManager>();
        this.RefreshSlotEquip();
    }
    // Cập nhật toàn bộ UI: 3 slot trang bị + 4 chỉ số
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
        // _attackText.text = (BaseAttack + _equipmentManager.GetBonus(EquipType.Weapon)).ToString();
        // _hpText.text = (BaseHP + _equipmentManager.GetBonus(EquipType.Pants)).ToString();
        // _defenseText.text = (BaseDefense + _equipmentManager.GetBonus(EquipType.Armor)).ToString();
        // _speedText.text = BaseSpeed.ToString();
    }
    public void TryEquip(int inventoryIndex)
    {
        // LoadPlayerIventory();

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
