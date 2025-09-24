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

    private const string ItemNullErrorMessage = "ItemConfigSO cannot be null.";
    private const string InvalidQuantityErrorMessage = "Quantity must be greater than zero.";

    #region Events
    public event Action OnInventoryFull;
    public event Action<string> OnItemStackLimitReached;
    public event Action<InventoryItem> OnItemAdded;
    #endregion

    #region Validation Methods
    /// <summary>
    /// Validates the ItemConfigSO parameter to ensure it is not null
    /// </summary>
    /// <param name="worldItem">The item to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when worldItem is null</exception>
    private void ValidateItemConfig(ItemConfigSO worldItem)
    {
        if (worldItem == null)
            throw new ArgumentNullException(nameof(worldItem), ItemNullErrorMessage);
    }

    /// <summary>
    /// Validates the quantity parameter to ensure it is greater than zero
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    /// <exception cref="ArgumentException">Thrown when quantity is less than 1</exception>
    private void ValidateQuantity(int quantity)
    {
        if (quantity < 1)
            throw new ArgumentException(InvalidQuantityErrorMessage, nameof(quantity));
    }
    #endregion

    /// <summary>
    /// Adds an item to the inventory with specified quantity
    /// Handles capacity checking, stacking logic, and appropriate event notifications
    /// </summary>
    /// <param name="worldItemConfig">The item scriptable object to add to the inventory</param>
    /// <param name="quantity">Number of items to add (default: 1)</param>
    public void AddItem(ItemConfigSO worldItemConfig, int quantity = 1)
    {
        ValidateItemConfig(worldItemConfig);
        ValidateQuantity(quantity);

        if (_currentCapacity >= MaxCapacity)
        {
            OnInventoryFull?.Invoke();
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
                    OnItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                    OnItemAdded?.Invoke(existingItem);
            }
            else
            {
                InventoryItem newStackableItem = new(worldItemConfig);
                int leftover = newStackableItem.PushStack(remaining - 1);
                int actuallyAdded = remaining - leftover;

                _items[newStackableItem.ID] = newStackableItem;
                _currentCapacity += actuallyAdded;

                if (leftover > 0)
                    OnItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                    OnItemAdded?.Invoke(newStackableItem);
            }

            return;
        }

        InventoryItem newNonStackableItem = null;

        for (int i = 0; i < remaining; i++)
        {
            if (_currentCapacity >= MaxCapacity)
            {
                OnInventoryFull?.Invoke();
                break;
            }

            newNonStackableItem = new(worldItemConfig);
            _items[newNonStackableItem.ID] = newNonStackableItem;
            _currentCapacity++;
        }

        if (newNonStackableItem != null)
            OnItemAdded?.Invoke(newNonStackableItem);
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
        ValidateItemConfig(worldItemConfig);
        ValidateQuantity(quantity);

        if (_currentCapacity >= MaxCapacity)
        {
            OnInventoryFull?.Invoke();
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
                    OnItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                {
                    OnItemAdded?.Invoke(existingItem);
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
                    OnItemStackLimitReached?.Invoke(worldItemConfig.ItemName);

                if (actuallyAdded > 0)
                {
                    OnItemAdded?.Invoke(newStackableItem);
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
                OnInventoryFull?.Invoke();
                break;
            }

            newNonStackableItem = new(worldItemConfig);
            _items[newNonStackableItem.ID] = newNonStackableItem;
            _currentCapacity++;
        }

        if (newNonStackableItem != null)
        {
            OnItemAdded?.Invoke(newNonStackableItem);
            id = newNonStackableItem.ID;
            return;
        }

        id = 0;
    }
#endif
    #endregion
}