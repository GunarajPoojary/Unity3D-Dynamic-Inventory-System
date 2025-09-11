using NUnit.Framework;
using UnityEngine;

public class InventoryTests
{
    private Inventory _inventory;
    private ItemConfigSO _stackableItemConfig;
    private ItemConfigSO _nonStackableItemConfig;

    [SetUp]
    public void SetUp()
    {
        _inventory = new Inventory();

        _stackableItemConfig = ScriptableObject.CreateInstance<ItemConfigSO>();
        _stackableItemConfig.Initialize("Potion", 10);

        _nonStackableItemConfig = ScriptableObject.CreateInstance<ItemConfigSO>();
        _nonStackableItemConfig.Initialize("Sword", 1);
    }

    [Test]
    public void AddItem_WhenInventoryIsFull_OnInventoryFullEventIsFired()
    {
        _inventory.AddItem(_nonStackableItemConfig, _inventory.MaxCapacity);

        bool eventFired = false;
        _inventory.OnInventoryFull += () => eventFired = true;

        _inventory.AddItem(_nonStackableItemConfig);

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void AddItem_AddsNewNonStackableItem()
    {
        InventoryItem addedItem = null;
        _inventory.OnItemAdded += (item) => addedItem = item;

        _inventory.AddItem(_nonStackableItemConfig, 1);

        Assert.IsNotNull(addedItem);
        Assert.AreEqual(_nonStackableItemConfig, addedItem.Item);
        Assert.AreEqual(1, addedItem.StackCount);
        Assert.AreEqual(1, _inventory.Items.Count);
    }

    [Test]
    public void AddItem_AddsNewStackableItem()
    {
        _inventory.AddItem(out int key, _stackableItemConfig, 5);

        Assert.IsTrue(_inventory.Items.ContainsKey(key));
        Assert.AreEqual(5, _inventory.Items[key].StackCount);
    }

    [Test]
    public void AddItem_AddsToExistingStack()
    {
        _inventory.AddItem(_stackableItemConfig, 3);
        _inventory.AddItem(out int key, _stackableItemConfig, 4);

        Assert.AreEqual(1, _inventory.Items.Count);
        Assert.AreEqual(7, _inventory.Items[key].StackCount);
    }

    // [Test]
    // public void AddItem_WhenStackIsFull_OnItemStackLimitReachedIsFired()
    // {
    //     _inventory.AddItem(_stackableItemConfig, 8);

    //     bool eventFired = false;
    //     _inventory.OnItemStackLimitReached += (name) => eventFired = true;

    //     _inventory.AddItem(_stackableItemConfig, 5); // Tries to add 5, but only 2 will fit

    //     var itemKey = _stackableItemConfig.GetInstanceID();
    //     Assert.IsTrue(eventFired);
    //     Assert.AreEqual(10, _inventory.Items[itemKey].StackCount);
    // }

    // [Test]
    // public void AddItem_WhenAddingNewStackableItemWithQuantityOverMaxStack_OnItemStackLimitReachedIsFired()
    // {
    //     bool eventFired = false;
    //     _inventory.OnItemStackLimitReached += (name) => eventFired = true;

    //     _inventory.AddItem(_stackableItemConfig, 15); // MaxStack is 10

    //     var itemKey = _stackableItemConfig.GetInstanceID();
    //     Assert.IsTrue(eventFired);
    //     Assert.AreEqual(10, _inventory.Items[itemKey].StackCount);
    // }

    // [Test]
    // public void AddItem_DoesNotExceedInventoryMaxCapacity()
    // {
    //     InventoryItem addedItem = null;
    //     _inventory.OnItemAdded += (item) => addedItem = item;

    //     // MaxCapacity is 100, try to add 120
    //     _inventory.AddItem(_stackableItemConfig, 120);

    //     var itemKey = _stackableItemConfig.GetInstanceID();
    //     // The constructor of InventoryItem will clamp this to MaxStack (10)
    //     Assert.AreEqual(10, _inventory.Items[itemKey].StackCount);

    //     // Add more items to fill up capacity
    //     var anotherStackableItem = ScriptableObject.CreateInstance<ItemConfigSO>();
    //     anotherStackableItem.Initialize("Elixir", 20, true);

    //     _inventory.AddItem(anotherStackableItem, 95); // 10 already in inventory, 90 space left

    //     var anotherItemKey = anotherStackableItem.GetInstanceID();
    //     // It should only add 20 (MaxStack) because that's all that fits in the new stack,
    //     // and the total requested (95) is more than available capacity (90).
    //     // The amount to add is min(95, 90) = 90.
    //     // The new item stack will be min(90, 20) = 20.
    //     Assert.AreEqual(20, _inventory.Items[anotherItemKey].StackCount);

    //     int totalItems = _inventory.Items.Sum(kvp => kvp.Value.StackCount);
    //     Assert.AreEqual(10 + 20, totalItems);
    // }
}