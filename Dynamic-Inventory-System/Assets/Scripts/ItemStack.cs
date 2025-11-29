using UnityEngine;

public class ItemStack
{
    public int MaxStack { get; private set; }
    public int CurrentStackSize { get; private set; } = 1;

    public int RemainingStackSize => MaxStack - CurrentStackSize;
    public bool IsFull => CurrentStackSize >= MaxStack;

    public ItemStack(int maxStack, int amount = 1)
    {
        MaxStack = Mathf.Max(1, maxStack);
        CurrentStackSize = Mathf.Clamp(amount, 1, MaxStack);
    }

    public int AddToStack(int amount)
    {
        if (amount <= 0) return 0;

        int spaceLeft = MaxStack - CurrentStackSize;
        int addAmount = Mathf.Min(amount, spaceLeft);

        CurrentStackSize += addAmount;

        return amount - addAmount;
    }

    public int RemoveFromStack(int amount)
    {
        if (amount <= 0) return 0;

        int removed = Mathf.Min(amount, CurrentStackSize);
        CurrentStackSize -= removed;

        return removed;
    }
}