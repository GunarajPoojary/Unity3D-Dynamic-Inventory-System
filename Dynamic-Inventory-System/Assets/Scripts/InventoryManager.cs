using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private int _weaponsCapacity = 40;
    [SerializeField] private int _consumablesCapacity = 40;
    [SerializeField] private int _armorsCapacity = 40;
    [SerializeField] private int _miscsCapacity = 40;
    [SerializeField] private UIInventory _uIInventory;
    [SerializeField] private SOInventoryItemEventChannel _removeItemEvent;

    private InventoryController _inventoryController;

    private void Awake()
    {
        Instance = this;

        _inventoryController = new InventoryController(_weaponsCapacity, _armorsCapacity, _consumablesCapacity, _miscsCapacity, _uIInventory);
        _inventoryController.AssignEventReference(_removeItemEvent);
    }

    private void OnEnable()
    {
        _inventoryController.SubscribeToEvents();
    }

    private void OnDisable()
    {
        _inventoryController.UnsubscribeFromEvents();
    }

    public AddItemResult AddItem(SOItemConfig config, int amount = 1)
    {
        AddItemResult result = _inventoryController.AddItem(config, amount);

        if (!result.Success)
        {
            Debug.Log($"Failed to add item: {config.DisplayName}. " +
                      $"Inventory full. Leftover: {result.Leftover}");
        }
        else
        {
            if (result.Leftover == 0)
            {
                Debug.Log($"✔ Added {result.Added}x {config.DisplayName}");
            }
            else
            {
                Debug.Log($"Partially added {result.Added}/{amount} of {config.DisplayName}, " +
                          $"{result.Leftover} could not fit.");
            }
        }

        return result;
    }
}