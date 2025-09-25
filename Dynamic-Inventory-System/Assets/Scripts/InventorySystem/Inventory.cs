using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory model class that manages collections of items
/// </summary>
[Serializable]
public class Inventory
{
    [Range(1, 2000)]
    [field: SerializeField]
    public int MaxCapacity { get; private set; } = 100;

    private readonly Dictionary<int, InventoryItem> _items = new();
    public IReadOnlyDictionary<int, InventoryItem> Items => _items;

    private int _currentCapacity = 0;

    private readonly Action _onInventoryFull;
    private readonly Action<string> _onItemStackLimitReached;
    private readonly Action<InventoryItem> _onItemAdded;

    public Inventory(Action onInventoryFull, Action<string> onItemStackLimitReached, Action<InventoryItem> onItemAdded)
    {
        _onInventoryFull = onInventoryFull;
        _onItemStackLimitReached = onItemStackLimitReached;
        _onItemAdded = onItemAdded;
    }

    /// <summary>
    /// Adds an item to the inventory with specified quantity
    /// Handles capacity checking, stacking logic, and appropriate event notifications
    /// </summary>
    /// <param name="worldItemConfig">The item scriptable object to add to the inventory</param>
    /// <param name="quantity">Number of items to add (default: 1)</param>
    public void AddItem(ItemConfigSO worldItemConfig, int quantity = 1)
    {
        InventoryUtility.ValidateItemConfig(worldItemConfig);
        InventoryUtility.ValidateQuantity(quantity);

        if (_currentCapacity >= MaxCapacity)
        {
            _onInventoryFull?.Invoke();
            return;
        }

        // Calculate how many items can actually be added based on available capacity
        int remaining = quantity;
        int availableCapacity = MaxCapacity - _currentCapacity;

        // Limit quantity to available capacity to prevent overflow
        remaining = Mathf.Min(remaining, availableCapacity);

        // Handle stackable items by attempting to add to existing stacks
        if (worldItemConfig.IsStackable)
        {
            if (_items.TryGetValue(worldItemConfig.ID, out InventoryItem existingItem))
            {
                // Attempt to add items to existing stack
                int leftover = existingItem.PushStack(remaining);
                int actuallyAdded = remaining - leftover;
                _currentCapacity += actuallyAdded;

                // Handle case where not all items could be stacked due to stack limits
                if (leftover > 0)
                    _onItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                    _onItemAdded?.Invoke(existingItem);
            }
            else
            {
                InventoryItem newStackableItem = new(worldItemConfig);
                int leftover = newStackableItem.PushStack(remaining - 1);
                int actuallyAdded = remaining - leftover;

                _items[newStackableItem.ID] = newStackableItem;
                _currentCapacity += actuallyAdded;

                if (leftover > 0)
                    _onItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                    _onItemAdded?.Invoke(newStackableItem);
            }

            return;
        }

        InventoryItem newNonStackableItem = null;

        for (int i = 0; i < remaining; i++)
        {
            if (_currentCapacity >= MaxCapacity)
            {
                _onInventoryFull?.Invoke();
                break;
            }

            newNonStackableItem = new(worldItemConfig);
            _items[newNonStackableItem.ID] = newNonStackableItem;
            _currentCapacity++;
        }

        if (newNonStackableItem != null)
            _onItemAdded?.Invoke(newNonStackableItem);
    }

    #region Unit Test Method
#if UNITY_EDITOR
    /// <summary>
    /// Editor-only version of AddItem that returns the ID of the added item
    /// </summary>
    /// <param name="id">Output parameter containing the ID of the added item</param>
    /// <param name="worldItemConfig">The item scriptable object to add to the inventory</param>
    /// <param name="quantity">Number of items to add (default: 1)</param>
    public void AddItem(out int id, ItemConfigSO worldItemConfig, int quantity = 1)
    {
        InventoryUtility.ValidateItemConfig(worldItemConfig);
        InventoryUtility.ValidateQuantity(quantity);

        if (_currentCapacity >= MaxCapacity)
        {
            _onInventoryFull?.Invoke();
            id = 0;
            return;
        }

        // Calculate how many items can actually be added based on available capacity
        int remaining = quantity;
        int availableCapacity = MaxCapacity - _currentCapacity;

        // Limit quantity to available capacity to prevent overflow
        remaining = Mathf.Min(remaining, availableCapacity);

        // Handle stackable items by attempting to add to existing stacks
        if (worldItemConfig.IsStackable)
        {
            if (_items.TryGetValue(worldItemConfig.ID, out InventoryItem existingItem))
            {
                // Attempt to add items to existing stack
                int leftover = existingItem.PushStack(remaining);
                int actuallyAdded = remaining - leftover;
                _currentCapacity += actuallyAdded;

                // Handle case where not all items could be stacked due to stack limits
                if (leftover > 0)
                    _onItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                {
                    _onItemAdded?.Invoke(existingItem);
                    id = existingItem.ID;
                    return;
                }
            }
            else
            {
                InventoryItem newStackableItem = new(worldItemConfig);
                int leftover = newStackableItem.PushStack(remaining - 1);
                int actuallyAdded = remaining - leftover;

                _items[newStackableItem.ID] = newStackableItem;
                _currentCapacity += actuallyAdded;

                if (leftover > 0)
                    _onItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                {
                    _onItemAdded?.Invoke(newStackableItem);
                    id = newStackableItem.ID;
                    return;
                }
            }

            id = 0;
            return;
        }

        InventoryItem newNonStackableItem = null;

        for (int i = 0; i < remaining; i++)
        {
            if (_currentCapacity >= MaxCapacity)
            {
                _onInventoryFull?.Invoke();
                break;
            }

            newNonStackableItem = new(worldItemConfig);
            _items[newNonStackableItem.ID] = newNonStackableItem;
            _currentCapacity++;
        }

        if (newNonStackableItem != null)
        {
            _onItemAdded?.Invoke(newNonStackableItem);
            id = newNonStackableItem.ID;
            return;
        }

        id = 0;
    }
#endif
    #endregion
}
public class AddItemOperationHandle
{
    public bool Success { get; private set; }
}