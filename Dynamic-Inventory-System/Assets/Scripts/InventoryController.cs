public class InventoryController
{
    private readonly UIInventory _uIInventory;
    private SOInventoryItemEventChannel _removeItemEvent;

    private Inventory _inventory;

    public InventoryController(int weaponCapacity, int armorsCapacity, int consumableCapacity, int miscsCapacity, UIInventory uIInventory)
    {
        _uIInventory = uIInventory;

        _inventory = new Inventory(weaponCapacity, armorsCapacity, consumableCapacity, miscsCapacity);
        _uIInventory.Init(weaponCapacity, armorsCapacity, consumableCapacity, miscsCapacity);
    }

    public void SubscribeToEvents()
    {
        _inventory.OnItemUpdated += AddItemSlot;
        _removeItemEvent.OnEventRaised += HandleItemRemoval;
    }

    public void UnsubscribeFromEvents()
    {
        _inventory.OnItemUpdated -= AddItemSlot;
        _removeItemEvent.OnEventRaised -= HandleItemRemoval;
    }

    private void AddItemSlot(int slotIndex, InventoryItem item) =>
        _uIInventory.AddItemSlot(slotIndex, item);

    public AddItemResult AddItem(SOItemConfig config, int amount = 1) => _inventory.AddItem(config, amount);

    public void RemoveItem(int slotIndex, ItemType itemType, int amount = 1)
    {
        _inventory.RemoveItem(slotIndex, itemType, amount);
        _uIInventory.RemoveSlot(slotIndex, itemType);
    }

    public void AssignEventReference(SOInventoryItemEventChannel removeItemEvent)
    {
        _removeItemEvent = removeItemEvent;
    }

    private void HandleItemRemoval(InventoryItem item)
    {
        RemoveItem(item.IndexID, item.ItemType);
    }
}