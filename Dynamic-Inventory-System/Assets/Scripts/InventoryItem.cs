[System.Serializable]
public class InventoryItem
{
    private SOItemConfig _itemConfig;
    private ItemStack _itemStack;

    public int Quantity => _itemStack.CurrentStackSize;
    public bool IsEmpty => _itemConfig == null;
    public int SpaceLeft => _itemStack.RemainingStackSize;
    public bool IsFull => _itemStack.IsFull;
    public SOItemConfig ItemConfig => _itemConfig;

    public InventoryItem()
    {
        _itemConfig = null;
        _itemStack = null;
    }

    public InventoryItem(SOItemConfig itemConfig, int quantity = 1)
    {
        Set(itemConfig, quantity);
    }

    public void Set(SOItemConfig itemConfig, int quantity = 1)
    {
        _itemConfig = itemConfig;
        _itemStack = _itemConfig.Type == ItemType.Consumable
                        ? new ItemStack(itemConfig.MaxStack, quantity)
                        : null;
    }

    public void Reset()
    {
        _itemConfig = null;
        _itemStack = null;
    }

    /// <summary>
    /// Adds quantity. Returns leftover that could not fit.
    /// </summary>
    public int AddQuantity(int amount) => _itemStack.AddToStack(amount);

    /// <summary>
    /// Removes quantity. Returns amount actually removed.
    /// </summary>
    public int RemoveQuantity(int amount) => _itemStack.RemoveFromStack(amount);
}