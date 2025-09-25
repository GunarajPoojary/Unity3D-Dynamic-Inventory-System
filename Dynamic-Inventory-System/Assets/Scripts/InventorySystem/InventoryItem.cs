using UnityEngine;
using System;

/// <summary>
/// Runtime Item Data class used by Inventory.
/// </summary>
[Serializable]
public class InventoryItem
{
    private static int _nextID = 1;
    private readonly int _id = 0;
    public int ID => _id;

    private ItemConfigSO _item;
    private int _stackCount = 1;

    public ItemConfigSO Item => _item;
    public int StackCount => _stackCount;
    public int RemainingStackSize => _item.MaxStack - _stackCount;

    public InventoryItem(ItemConfigSO item)
    {
        InventoryUtility.ValidateItemConfig(item);
        _item = item;
        _id = _item.IsStackable ? _item.ID : ++_nextID;
    }

    /// <summary>
    /// Method to Reset the Inventory Item to original state (Ex: Used for pool)
    /// </summary>
    /// <param name="item"></param>
    public void ResetItem(ItemConfigSO item)
    {
        InventoryUtility.ValidateItemConfig(item);
        _item = item;
        _stackCount = 1;
    }

    /// <summary>
    /// Adds to the stack
    /// </summary>
    /// <returns> Leftover amount that couldn't be added </returns>
    /// <param name="amount"></param>
    public int PushStack(int amount)
    {
        InventoryUtility.ValidateQuantity(amount);
        int toAdd = Mathf.Min(amount, RemainingStackSize);
        _stackCount += toAdd;
        return amount - toAdd;
    }

    /// <summary>
    /// Method to remove items from the stack
    /// </summary>
    /// <param name="amount"></param>
    public bool PopStack(int amount)
    {
        InventoryUtility.ValidateQuantity(amount);
        _stackCount = Mathf.Max(0, _stackCount - amount);
        return _stackCount <= 0;
    }
}