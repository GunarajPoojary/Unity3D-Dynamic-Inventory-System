using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private readonly List<InventoryItem> _weapons;
    private readonly List<InventoryItem> _armors;
    private readonly List<InventoryItem> _resources;

    public event Action<int, InventoryItem> OnItemAdded;
    public event Action<int, ItemType> OnItemRemoved;

    // New events → Presenter will react to these
    public event Action<SOItemConfig, int> OnItemAddFailed;
    public event Action<SOItemConfig, int> OnItemPartiallyAdded;
    public event Action<SOItemConfig> OnStackLimitReached;

    public Inventory(int weapons, int armors, int resources)
    {
        _weapons = InitItems(weapons);
        _armors = InitItems(armors);
        _resources = InitItems(resources);
    }

    private List<InventoryItem> InitItems(int capacity)
    {
        var list = new List<InventoryItem>(capacity);
        for (int i = 0; i < capacity; i++)
            list.Add(new InventoryItem(null));
        return list;
    }

    public AddItemResult AddItem(SOItemConfig config, int amount = 1)
    {
        return config.Type switch
        {
            ItemType.Weapon => AddNonStackable(_weapons, config, amount),
            ItemType.Armor => AddNonStackable(_armors, config, amount),
            ItemType.Resource => AddStackable(_resources, config, amount),
            _ => new AddItemResult { Success = false, Added = 0, Leftover = amount }
        };
    }

    private AddItemResult AddNonStackable(List<InventoryItem> items, SOItemConfig config, int amount)
    {
        int added = 0;

        while (amount > 0)
        {
            int empty = GetFirstEmptySlot(items);
            if (empty == -1)
            {
                OnItemAddFailed?.Invoke(config, amount);
                return new AddItemResult { Success = added > 0, Added = added, Leftover = amount };
            }

            items[empty].Set(config);
            OnItemAdded?.Invoke(empty, items[empty]);

            amount--;
            added++;
        }

        return new AddItemResult { Success = true, Added = added, Leftover = 0 };
    }

    private AddItemResult AddStackable(List<InventoryItem> items, SOItemConfig config, int amount)
    {
        int original = amount;
        bool foundStack = false;

        // Fill existing stacks
        for (int i = 0; i < items.Count && amount > 0; i++)
        {
            InventoryItem item = items[i];
            if (!item.IsEmpty && item.ItemConfig == config)
            {
                foundStack = true;

                int leftover = item.AddQuantity(amount);
                OnItemAdded?.Invoke(i, item);

                if (leftover > 0)
                {
                    OnStackLimitReached?.Invoke(config);
                }

                amount = leftover;
            }
        }

        if (foundStack && amount > 0)
        {
            OnItemPartiallyAdded?.Invoke(config, amount);
            return new AddItemResult
            {
                Success = original != amount,
                Added = original - amount,
                Leftover = amount
            };
        }

        // Create new stack
        int emptySlot = GetFirstEmptySlot(items);
        if (emptySlot == -1)
        {
            OnItemAddFailed?.Invoke(config, amount);
            return new AddItemResult { Success = false, Added = 0, Leftover = amount };
        }

        int toPlace = Mathf.Min(amount, config.MaxStack);
        items[emptySlot].Set(config, toPlace);
        OnItemAdded?.Invoke(emptySlot, items[emptySlot]);

        int leftoverNew = amount - toPlace;
        if (leftoverNew > 0)
            OnItemPartiallyAdded?.Invoke(config, leftoverNew);

        return new AddItemResult { Success = true, Added = toPlace, Leftover = leftoverNew };
    }

    private int GetFirstEmptySlot(List<InventoryItem> items)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].IsEmpty) return i;
        return -1;
    }

    public void RemoveItem(int slot, ItemType type)
    {
        List<InventoryItem> list = type switch
        {
            ItemType.Weapon => _weapons,
            ItemType.Armor => _armors,
            ItemType.Resource => _resources,
            _ => null
        };

        if (list == null) return;

        list[slot].Clear();
        OnItemRemoved?.Invoke(slot, type);
    }
}

public struct AddItemResult
{
    public bool Success;   // True if at least some items were added
    public int Added;      // How many items were added
    public int Leftover;   // Items that couldn't be added
}