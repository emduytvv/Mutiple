using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SaiMonoBehaviour
{
    private const int MaxSlot = 9;
    [SerializeField] protected List<ItemInventoryBase> _items = new List<ItemInventoryBase>();
    public List<ItemInventoryBase> Items => _items;

    public void Swap(int fromIndex, int toIndex)
    {
        while (_items.Count <= Mathf.Max(fromIndex, toIndex))
            _items.Add(null);

        (_items[fromIndex], _items[toIndex]) = (_items[toIndex], _items[fromIndex]);
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= _items.Count) return;
        _items[index] = null;
    }
    public bool AddItem(ItemInventoryBase item)
    {
        for (int i = 0; i < MaxSlot; i++)
        {
            if (i >= _items.Count) { _items.Add(item); return true; }
            if (_items[i] == null || _items[i]._info == null) { _items[i] = item; return true; }
        }
        return false;
    }
}
