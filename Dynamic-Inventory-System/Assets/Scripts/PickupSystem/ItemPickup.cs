using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private SOItemConfig _itemConfig;

    public SOItemConfig PickUp() => _itemConfig;

    [ContextMenu("Add Item To Inventory")]
    private void AddItemToInventory()
    {
        Inventory.Instance.AddItem(_itemConfig);
    }
}