using UnityEngine;

public class ItemStack
{
    public int MaxStack { get; private set; }
    public int CurrentStackSize { get; private set; } = 1;
    public int RemainingStackSize => MaxStack - CurrentStackSize;
    public bool IsFull => CurrentStackSize == MaxStack; 

    public ItemStack(int maxStack, int startingAmount = 1)
    {
        MaxStack = Mathf.Max(1, maxStack);

        CurrentStackSize = Mathf.Clamp(startingAmount, 1, MaxStack);
    }

    /// <summary>
    /// Attempts to add amount to the stack.
    /// Returns the leftover amount that could not fit.
    /// </summary>
    public int AddToStack(int amount)
    {
        if (amount <= 0)
            return 0;

        int spaceLeft = MaxStack - CurrentStackSize;
        int amountToAdd = Mathf.Min(amount, spaceLeft);

        CurrentStackSize += amountToAdd;
        return amount - amountToAdd; // leftover
    }

    /// <summary>
    /// Removes from stack, returns amount actually removed.
    /// </summary>
    public int RemoveFromStack(int amount)
    {
        if (amount <= 0)
            return 0;

        int amountToRemove = Mathf.Min(amount, CurrentStackSize);
        CurrentStackSize -= amountToRemove;

        return amountToRemove;
    }
}