using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class InventoryManager : SaiMonoBehaviour
{
    [SerializeField] protected List<ItemBase> _items = new List<ItemBase>();
    public List<ItemBase> Items => _items;
}
