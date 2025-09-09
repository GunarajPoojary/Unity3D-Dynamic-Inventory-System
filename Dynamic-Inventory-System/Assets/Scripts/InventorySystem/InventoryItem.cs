using UnityEngine;
using System;

/// <summary>
/// Runtime Item Data class used by Inventory.
/// </summary>
[System.Serializable]
public class InventoryItem
{
    private ItemConfigSO _item;
    private int _stackCount;

    public ItemConfigSO Item => _item;
    public int StackCount => _stackCount;
    public int RemainingStackSize => _item.MaxStack - _stackCount;

    public InventoryItem(ItemConfigSO item, int stackCount = 1)
    {
        if (item == null) throw new ArgumentNullException(nameof(item), "ItemConfigSO cannot be null.");

        _item = item;

        _stackCount = Mathf.Clamp(stackCount, 1, item.MaxStack);
    }

    /// <summary>
    /// Method to Reset the Inventory Item to orignal state(Ex: Used for pool)
    /// </summary>
    /// <param name="item"></param>
    /// <param name="stackCount"></param>
    public void ResetItem(ItemConfigSO item, int stackCount = 1)
    {
        if (item == null) throw new ArgumentNullException(nameof(item), "ItemConfigSO cannot be null.");

        _item = item;

        _stackCount = Mathf.Clamp(stackCount, 1, item.MaxStack);
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
    public void PopStack(int amount)
    {
        if (amount <= 0) return;

        _stackCount = Mathf.Max(0, _stackCount - amount); // Subtract the amount while preventing negative stack counts
    }
}