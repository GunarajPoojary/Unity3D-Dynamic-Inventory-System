using System;
using System.Collections.Generic;
using UnityEngine;

public struct AddItemResult
{
    public bool Success;   // True if at least some items were added
    public int Added;      // How many items were added
    public int Leftover;   // Items that couldn't be added
}

public class Inventory
{
    private readonly List<InventoryItem> _weapons;
    private readonly List<InventoryItem> _armors;
    private readonly List<InventoryItem> _consumables;
    private readonly List<InventoryItem> _miscs;

    public event Action<int, InventoryItem> OnItemUpdated;

    public Inventory(int weaponCapacity, int armorsCapacity, int consumableCapacity, int miscsCapacity)
    {
        _weapons = InitItems(weaponCapacity);
        _armors = InitItems(armorsCapacity);
        _consumables = InitItems(consumableCapacity);
        _miscs = InitItems(miscsCapacity);
    }

    public AddItemResult AddItem(SOItemConfig config, int amount = 1)
    {
        if (config == null)
        {
            Debug.LogError("Trying to add NULL item to inventory.");
            return new AddItemResult { Success = false, Added = 0, Leftover = amount };
        }

        return config.Type switch
        {
            ItemType.Weapon => AddNonStackableItem(_weapons, config, amount),
            ItemType.Armor => AddNonStackableItem(_armors, config, amount),
            ItemType.Consumable => AddStackableItem(_consumables, config, amount),
            ItemType.Misc => AddNonStackableItem(_miscs, config, amount),
            _ => new AddItemResult { Success = false, Added = 0, Leftover = amount }
        };
    }

    public void RemoveItem(int slotIndex, ItemType type, int amount = 1)
    {
        switch (type)
        {
            case ItemType.Weapon:
                RemoveNonStackableItem(_weapons, slotIndex);
                break;

            case ItemType.Armor:
                RemoveNonStackableItem(_armors, slotIndex);
                break;

            case ItemType.Consumable:
                RemoveStackableItem(_consumables, slotIndex, amount);
                break;

            case ItemType.Misc:
                RemoveNonStackableItem(_miscs, slotIndex);
                break;
        }
    }

    private List<InventoryItem> InitItems(int maxCapacity)
    {
        List<InventoryItem> items = new List<InventoryItem>(maxCapacity);

        for (int i = 0; i < maxCapacity; i++)
            items.Add(new InventoryItem(i, null));

        return items;
    }

    private AddItemResult AddNonStackableItem(List<InventoryItem> items, SOItemConfig config, int amount)
    {
        int added = 0;

        while (amount > 0)
        {
            int emptySlot = GetFirstEmptySlot(items);
            if (emptySlot == -1)
            {
                return new AddItemResult
                {
                    Success = added > 0,
                    Added = added,
                    Leftover = amount
                };
            }

            items[emptySlot].Set(config);
            OnItemUpdated?.Invoke(emptySlot, items[emptySlot]);

            added++;
            amount--;
        }

        return new AddItemResult
        {
            Success = true,
            Added = added,
            Leftover = 0
        };
    }

    private AddItemResult AddStackableItem(List<InventoryItem> items, SOItemConfig config, int amount)
    {
        int originalAmount = amount;
        bool existingStackFound = false;

        // Try to fill existing stacks first
        for (int i = 0; i < items.Count && amount > 0; i++)
        {
            var item = items[i];
            if (!item.IsEmpty && item.ItemConfig == config)
            {
                existingStackFound = true;

                int leftover = item.AddQuantity(amount);
                OnItemUpdated?.Invoke(i, item);
                amount = leftover;  // leftover > 0 means stack was full
            }
        }

        // If we found existing stacks and they became full, do NOT create a new stack
        if (existingStackFound && amount > 0)
        {
            return new AddItemResult
            {
                Success = originalAmount != amount, // true if partially added
                Added = originalAmount - amount,
                Leftover = amount
            };
        }

        // No existing stack found → create new one if space available
        if (!existingStackFound)
        {
            int emptySlot = GetFirstEmptySlot(items);
            if (emptySlot == -1)
            {
                return new AddItemResult
                {
                    Success = false,
                    Added = 0,
                    Leftover = amount
                };
            }

            int placeAmount = Mathf.Min(amount, config.MaxStack);
            items[emptySlot].Set(config, placeAmount);
            OnItemUpdated?.Invoke(emptySlot, items[emptySlot]);
        }

        return new AddItemResult
        {
            Success = true,
            Added = originalAmount,
            Leftover = 0
        };
    }

    private int GetFirstEmptySlot(List<InventoryItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].IsEmpty)
                return i;
        }
        return -1;
    }

    private void RemoveNonStackableItem(List<InventoryItem> items, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count || items[slotIndex] == null)
            return;

        items[slotIndex].Reset();
    }

    private void RemoveStackableItem(List<InventoryItem> items, int slotIndex, int amount)
    {
        if (slotIndex < 0 || slotIndex >= items.Count || items[slotIndex] == null)
            return;

        items[slotIndex].RemoveQuantity(amount);

        if (items[slotIndex].Quantity <= 0)
            items[slotIndex].Reset();
    }
}