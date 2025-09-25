using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemConfigSOEventChannelSO _addItemEvent;
    [SerializeField] private InventoryItemEventChannelSO _itemAddedEvent;
    [SerializeField] private VoidEventChannelSO _inventoryFullEvent;
    [SerializeField] private StringEventChannelSO _itemStackLimitReachedEvent;

    private Inventory _inventory;

    private void Awake()
    {
        _inventory = new Inventory(HandleInventoryFull, HandleItemStackLimitReached, HandleItemAdded);
    }

    private void OnEnable()
    {
        if (_addItemEvent != null)
            _addItemEvent.OnEventRaised += AddItem;
    }

    private void OnDisable()
    {
        if (_addItemEvent != null)
            _addItemEvent.OnEventRaised -= AddItem;
    }

    private void AddItem(ItemConfigSO item, int quantity)
    {
        _inventory.AddItem(item, quantity);
    }

    private void HandleInventoryFull()
    {
        _inventoryFullEvent.RaiseEvent();
        Debug.Log("Inventory is full");
    }

    private void HandleItemStackLimitReached(string itemName)
    {
        _itemStackLimitReachedEvent.RaiseEvent(itemName);
        Debug.Log("Stack limit reached for item: " + itemName);
    }

    private void HandleItemAdded(InventoryItem item)
    {
        _itemAddedEvent.RaiseEvent(item);
        Debug.Log("Item added: " + item);
    }
}