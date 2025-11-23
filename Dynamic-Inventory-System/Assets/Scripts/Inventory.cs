using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Capacity")]
    [SerializeField] private int _weaponCapacity = 40;
    [SerializeField] private int _armorsCapacity = 40;
    [SerializeField] private int _consumableCapacity = 40;
    [SerializeField] private int _miscsCapacity = 40;

    private List<InventoryItem> _weapons;
    private List<InventoryItem> _armors;
    private List<InventoryItem> _consumables;
    private List<InventoryItem> _miscs;

    private int _weaponCount;
    private int _consumablesCount;

    private void Awake()
    {
        Instance = this;

        Init();
    }

    private List<InventoryItem> InitItems(int maxCapacity)
    {
        List<InventoryItem> items = new List<InventoryItem>(maxCapacity);

        for (int i = 0; i < maxCapacity; i++)
            items.Add(new InventoryItem());

        return items;
    }

    private void Init()
    {
        _weapons = InitItems(_weaponCapacity);
        _armors = InitItems(_armorsCapacity);
        _consumables = InitItems(_consumableCapacity);
        _miscs = InitItems(_miscsCapacity);
    }

    public void AddItem(SOItemConfig config, int amount = 1)
    {
        if (config == null)
        {
            Debug.LogError("Trying to add NULL item to inventory.");
            return;
        }

        switch (config.Type)
        {
            case ItemType.Weapon:
                AddNonStackableItem(_weapons, config, amount);
                break;

            case ItemType.Armor:
                AddNonStackableItem(_armors, config, amount);
                break;

            case ItemType.Consumable:
                AddConsumable(config, amount);
                break;

            case ItemType.Misc:
                AddNonStackableItem(_miscs, config, amount);
                break;
        }
    }

    private void AddNonStackableItem(List<InventoryItem> items, SOItemConfig config, int amount)
    {
        while (amount > 0)
        {
            int emptySlot = GetFirstEmptySlot(items);
            if (emptySlot == -1)
            {
                Debug.Log("Inventory is full!");
                return;
            }

            items[emptySlot].Set(config);
            amount--;
        }
    }

    private void AddConsumable(SOItemConfig config, int amount)
    {
        if (config == null || amount <= 0)
            return;

        // 1) Try to find an existing stack of this item
        for (int i = 0; i < _consumables.Count; i++)
        {
            InventoryItem slot = _consumables[i];

            if (!slot.IsEmpty && slot.ItemConfig == config)
            {
                // Add ONLY until stack is full, ignore leftover
                slot.AddQuantity(amount);
                return;
            }
        }

        // 2) No existing stack found → place it in the first empty slot (if any)
        int emptySlot = GetFirstEmptySlot(_consumables);

        if (emptySlot == -1)
        {
            Debug.Log("No space for consumables!");
            return;
        }

        // Clamp amount to the maximum stack allowed
        int placeAmount = Mathf.Min(amount, config.MaxStack);

        _consumables[emptySlot].Set(config, placeAmount);
    }

    public int GetFirstEmptySlot(List<InventoryItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].IsEmpty)
                return i;
        }
        return -1;
    }

    public void RemoveItem(int slotIndex, ItemType type, int amount = 1)
    {
        switch (type)
        {
            case ItemType.Weapon:
                RemoveNonStackable(_weapons, slotIndex);
                break;

            case ItemType.Armor:
                RemoveNonStackable(_armors, slotIndex);
                break;

            case ItemType.Consumable:
                RemoveConsumable(slotIndex, amount);
                break;

            case ItemType.Misc:
                RemoveNonStackable(_miscs, slotIndex);
                break;
        }
    }

    private void RemoveNonStackable(List<InventoryItem> items, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count || items[slotIndex] == null)
            return;

        items[slotIndex].Reset();
    }

    private void RemoveConsumable(int slotIndex, int amount)
    {
        if (slotIndex < 0 || slotIndex >= _consumables.Count || _consumables[slotIndex] == null)
            return;

        _consumables[slotIndex].RemoveQuantity(amount);

        if (_consumables[slotIndex].Quantity <= 0)
        {
            _consumables[slotIndex].Reset();
        }
    }

    [ContextMenu("Print Items")]
    private void PrintItems()
    {
        Debug.Log("=== INVENTORY CONTENTS ===");

        PrintCategory("Weapons", _weapons);
        PrintCategory("Armors", _armors);
        PrintCategory("Consumables", _consumables, true);
        PrintCategory("Misc", _miscs);

        Debug.Log("=== END OF INVENTORY ===");
    }

    private void PrintCategory(string title, List<InventoryItem> items, bool showQuantity = false)
    {
        Debug.Log($"-- {title} --");

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];

            if (item == null || item.IsEmpty)
            {
                Debug.Log($"[{i}] (Empty)");
            }
            else
            {
                string name = item.ItemConfig.DisplayName;

                if (showQuantity)
                    Debug.Log($"[{i}] {name} x{item.Quantity}");
                else
                    Debug.Log($"[{i}] {name}");
            }
        }
    }
}