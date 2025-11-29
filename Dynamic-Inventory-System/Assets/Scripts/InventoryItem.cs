using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    private SOItemConfig _config;
    private ItemStack _stack;

    public bool IsEmpty => _config == null;
    public SOItemConfig ItemConfig => _config;

    public int Quantity => _stack?.CurrentStackSize ?? 0;
    public int SpaceLeft => _stack?.RemainingStackSize ?? 0;
    public bool IsFull => _stack?.IsFull ?? true;

    public ItemType ItemType => _config.Type;
    public Sprite Icon => _config.Icon;
    public string DisplayName => _config.DisplayName;
    public string Description => _config.Description;

    public InventoryItem(SOItemConfig config, int quantity = 1)
    {
        Set(config, quantity);
    }

    public void Set(SOItemConfig config, int quantity = 1)
    {
        _config = config;

        if (config != null && config.IsStackable)
            _stack = new ItemStack(config.MaxStack, quantity);
        else
            _stack = null;
    }

    public int AddQuantity(int amount) => _stack?.AddToStack(amount) ?? amount;

    public int RemoveQuantity(int amount) => _stack?.RemoveFromStack(amount) ?? 0;

    public void Clear()
    {
        _config = null;
        _stack = null;
    }
}