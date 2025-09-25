using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class InventoryTests
{
    private Inventory _inventory;
    private ItemConfigSO _stackableItemConfig;
    private ItemConfigSO _nonStackableItemConfig;

    private bool _inventoryFullEventFired;
    private bool _stackLimitEventFired;
    private InventoryItem _lastAddedItem;

    [SetUp]
    public void SetUp()
    {
        _inventoryFullEventFired = false;
        _stackLimitEventFired = false;
        _lastAddedItem = null;

        _inventory = new Inventory(
            () => _inventoryFullEventFired = true,
            (name) => _stackLimitEventFired = true,
            (item) => _lastAddedItem = item
        );

        _stackableItemConfig = ScriptableObject.CreateInstance<ItemConfigSO>();
        _stackableItemConfig.Initialize(1, "Potion", 10);

        _nonStackableItemConfig = ScriptableObject.CreateInstance<ItemConfigSO>();
        _nonStackableItemConfig.Initialize(-1, "Sword", 1);
    }

    [Test]
    public void AddItem_WhenInventoryIsFull_OnInventoryFullEventIsFired()
    {
        _inventory.AddItem(_nonStackableItemConfig, _inventory.MaxCapacity);

        _inventory.AddItem(_nonStackableItemConfig);

        Assert.IsTrue(_inventoryFullEventFired);
    }

    [Test]
    public void AddItem_AddsNewNonStackableItem()
    {
        _inventory.AddItem(_nonStackableItemConfig, 1);

        Assert.IsNotNull(_lastAddedItem);
        Assert.AreEqual(_nonStackableItemConfig, _lastAddedItem.Item);
        Assert.AreEqual(1, _lastAddedItem.StackCount);
        Assert.AreEqual(1, _inventory.Items.Count);
    }

    [Test]
    public void AddItem_AddsNewStackableItem()
    {
        _inventory.AddItem(out int key, _stackableItemConfig, 4);

        Assert.IsTrue(_inventory.Items.ContainsKey(key));
        Assert.AreEqual(4, _inventory.Items[key].StackCount);
    }

    [Test]
    public void AddItem_AddsToExistingStack()
    {
        _inventory.AddItem(_stackableItemConfig, 3);
        _inventory.AddItem(out int key, _stackableItemConfig, 4);

        Assert.AreEqual(1, _inventory.Items.Count);
        Assert.AreEqual(7, _inventory.Items[key].StackCount);
    }

    [Test]
    public void AddItem_WhenStackIsFull_OnItemStackLimitReachedIsFired()
    {
        _inventory.AddItem(_stackableItemConfig, 8);

        _inventory.AddItem(out int key, _stackableItemConfig, 5); // only 2 fit

        Assert.IsTrue(_stackLimitEventFired);
        Assert.AreEqual(10, _inventory.Items[key].StackCount);
    }

    [Test]
    public void AddItem_WhenAddingNewStackableItemWithQuantityOverMaxStack_OnItemStackLimitReachedIsFired()
    {
        _inventory.AddItem(out int key, _stackableItemConfig, 15); // MaxStack = 10

        Assert.IsTrue(_stackLimitEventFired);
        Assert.AreEqual(10, _inventory.Items[key].StackCount);
    }

    [Test]
    public void AddItem_DoesNotExceedInventoryMaxCapacity()
    {
        _inventory.AddItem(out int key, _stackableItemConfig, 120);

        Assert.AreEqual(10, _inventory.Items[key].StackCount);

        var anotherStackableItem = ScriptableObject.CreateInstance<ItemConfigSO>();
        anotherStackableItem.Initialize(3, "Elixir", 20);

        _inventory.AddItem(out int anotherKey, anotherStackableItem, 95);

        Assert.AreEqual(20, _inventory.Items[anotherKey].StackCount);

        int totalItems = _inventory.Items.Sum(kvp => kvp.Value.StackCount);
        Assert.AreEqual(10 + 20, totalItems);
    }
}