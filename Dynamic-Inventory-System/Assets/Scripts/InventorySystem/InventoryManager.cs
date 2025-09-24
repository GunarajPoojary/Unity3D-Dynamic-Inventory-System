using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemConfigSOEventChannelSO _itemConfigSOEventChannelSO;
    private Inventory _inventory;

    private void Awake()
    {
        _inventory = new Inventory();
    }

    private void OnEnable()
    {
        if (_itemConfigSOEventChannelSO != null)
            _itemConfigSOEventChannelSO.OnEventRaised += AddItem;
    }

    private void OnDisable()
    {
        if (_itemConfigSOEventChannelSO != null)
            _itemConfigSOEventChannelSO.OnEventRaised -= AddItem;
    }

    private void AddItem(ItemConfigSO item, int quantity)
    {
        _inventory.AddItem(item, quantity);
    }
}
