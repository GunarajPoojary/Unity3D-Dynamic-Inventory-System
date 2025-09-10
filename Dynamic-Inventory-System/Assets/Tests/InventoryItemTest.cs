using System;
using NUnit.Framework;
using UnityEngine;

public class InventoryItemTest
{
    private ItemConfigSO _stackableItemConfig;
    private ItemConfigSO _nonStackableItemConfig;

    [SetUp]
    public void Setup()
    {
        _stackableItemConfig = ScriptableObject.CreateInstance<ItemConfigSO>();
        _stackableItemConfig.Initialize("Potion", 10);

        _nonStackableItemConfig = ScriptableObject.CreateInstance<ItemConfigSO>();
        // For non-stackable items, MaxStack will be 1 regardless of the value passed.
        // Passing 1 for clarity.
        _nonStackableItemConfig.Initialize("Sword", 1);
    }

    [Test]
    public void PushStack_AddsToStack_WhenSpaceIsAvailable()
    {
        var item = new InventoryItem(_stackableItemConfig, 3); // MaxStack is 10
        int leftover = item.PushStack(5);

        Assert.AreEqual(8, item.StackCount);
        Assert.AreEqual(0, leftover);
    }

    [Test]
    public void PushStack_FillsStackAndReturnsLeftover()
    {
        var item = new InventoryItem(_stackableItemConfig, 7); // MaxStack is 10
        int leftover = item.PushStack(5);

        Assert.AreEqual(10, item.StackCount);
        Assert.AreEqual(2, leftover);
    }

    [Test]
    public void PushStack_DoesNotAdd_WhenStackIsFull()
    {
        var item = new InventoryItem(_stackableItemConfig, 10); // MaxStack is 10
        int leftover = item.PushStack(1);

        Assert.AreEqual(10, item.StackCount);
        Assert.AreEqual(1, leftover);
    }

    // [Test]
    // public void PushStack_WithZeroOrNegativeAmount_DoesNothing()
    // {
    //     var item = new InventoryItem(_stackableItemConfig, 5);
    //     int leftover = item.PushStack(0);
    //     Assert.Throws<ArgumentException>("bg");
    // }

    [Test]
    public void PopStack_RemovesFromStack()
    {
        var item = new InventoryItem(_stackableItemConfig, 8);
        item.PopStack(3);
        Assert.AreEqual(5, item.StackCount);
    }

    [Test]
    public void PopStack_WhenRemovingMoreThanAvailable_StackBecomesZero()
    {
        var item = new InventoryItem(_stackableItemConfig, 5);
        item.PopStack(10);
        Assert.AreEqual(0, item.StackCount);
    }

    [Test]
    public void ResetItem_WhenItemConfigIsNull_ThrowsArgumentNullException()
    {
        var item = new InventoryItem(_stackableItemConfig, 1);
        Assert.Throws<ArgumentNullException>(() => item.ResetItem(null));
    }
}