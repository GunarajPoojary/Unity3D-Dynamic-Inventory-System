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
        _inventory.AddItem(_stackableItemConfig, 5);

        var itemKey = _stackableItemConfig.GetInstanceID();
        Assert.IsTrue(_inventory.Items.ContainsKey(itemKey));
        Assert.AreEqual(5, _inventory.Items[itemKey].StackCount);
    }
}