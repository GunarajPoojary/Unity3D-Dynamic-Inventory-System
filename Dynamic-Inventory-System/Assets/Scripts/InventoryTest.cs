using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory _inventory;

    [Header("Test Input")]
    [SerializeField] private SOItemConfig _item;
    [SerializeField] private int _amount = 1;
    [SerializeField] private int _removeIndex = 0;
    [SerializeField] private ItemType _removeItemType;

    [ContextMenu("Add Item")]
    private void TestAddItem()
    {
        if (_inventory == null)
        {
            Debug.LogError("Inventory reference is missing!");
            return;
        }

        if (_item == null)
        {
            Debug.LogError("SOItemConfig is missing!");
            return;
        }

        _inventory.AddItem(_item, _amount);
        Debug.Log($"Added {_amount}x {_item.DisplayName}");
    }

    [ContextMenu("Remove Item")]
    private void TestRemoveItem()
    {
        if (_inventory == null)
        {
            Debug.LogError("Inventory reference is missing!");
            return;
        }

        if (_item == null)
        {
            Debug.LogError("SOItemConfig is missing!");
            return;
        }

        _inventory.RemoveItem(_removeIndex, _removeItemType);
        Debug.Log($"Removed {_amount}x {_item.DisplayName}");
    }
}