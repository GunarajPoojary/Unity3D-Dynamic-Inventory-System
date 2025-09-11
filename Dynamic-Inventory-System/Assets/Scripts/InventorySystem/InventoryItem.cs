using UnityEngine;
using System;

/// <summary>
/// Runtime Item Data class used by Inventory.
/// </summary>
[Serializable]
public class InventoryItem
{
    private static int _nextID = 0;
    public int ID { get; }
    private ItemConfigSO _item;
    private int _stackCount = 1;

    public ItemConfigSO Item => _item;
    public int StackCount => _stackCount;
    public int RemainingStackSize => _item.MaxStack - _stackCount;

    public InventoryItem(ItemConfigSO item, int stackCount = 1)
    {
        if (item == null || stackCount < 1) throw new ArgumentNullException(
            nameof(item),
            "ItemConfigSO cannot be null and stackCount cannot be less than 1.");

        ID = ++_nextID;

        _item = item;

        _stackCount = stackCount;
    }

    /// <summary>
    /// Method to Reset the Inventory Item to orignal state(Ex: Used for pool)
    /// </summary>
    /// <param name="item"></param>
    /// <param name="stackCount"></param>
    public void ResetItem(ItemConfigSO item, int stackCount = 1)
    {
        if (item == null || stackCount < 1) throw new ArgumentNullException(nameof(item),
                                                                                    "ItemConfigSO cannot be null and stackCount cannot be less than 1.");
        _item = item;

        _stackCount = stackCount;
    }

    /// <summary>
    /// Adds to the stack
    /// </summary>
    /// <returns> Leftover amount that couldn't be added </returns>
    /// <param name="amount"></param>
    public int PushStack(int amount)
    {
        if (amount <= 0) return 0;
        int toAdd = Mathf.Min(amount, RemainingStackSize);
        _stackCount += toAdd;
        return amount - toAdd;
    }

    /// <summary>
    /// // Method to remove items from the stack
    /// </summary>
    /// <param name="amount"></param>
    public bool PopStack(int amount)
    {
        if (amount < 1) throw new ArgumentNullException(nameof(amount), "amount cannot be less than 1.");

        _stackCount = Mathf.Max(0, _stackCount - amount); // Subtract the amount while preventing negative stack counts

        return _stackCount < 0;
    }
}