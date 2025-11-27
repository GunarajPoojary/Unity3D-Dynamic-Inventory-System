using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private UIInventoryItemSlot _slotPrefab;

    [Header("Containers")]
    [SerializeField] private RectTransform _weaponsContainer;
    [SerializeField] private RectTransform _armorsContainer;
    [SerializeField] private RectTransform _consumablesContainer;
    [SerializeField] private RectTransform _miscsContainer;

    private int _slotIndex;
    private ItemType _itemType;

    private List<UIInventoryItemSlot> _weaponSlots;
    private List<UIInventoryItemSlot> _armorSlots;
    private List<UIInventoryItemSlot> _consumableSlots;
    private List<UIInventoryItemSlot> _miscSlots;

    public void Init(int weaponCapacity, int armorsCapacity, int consumableCapacity, int miscsCapacity)
    {
        // Create lists
        _weaponSlots = CreateSlots(weaponCapacity, _weaponsContainer);
        _armorSlots = CreateSlots(armorsCapacity, _armorsContainer);
        _consumableSlots = CreateSlots(consumableCapacity, _consumablesContainer);
        _miscSlots = CreateSlots(miscsCapacity, _miscsContainer);
    }

    public void AddItemSlot(int index, InventoryItem item)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon: _weaponSlots[index].Init(item); break;
            case ItemType.Armor: _armorSlots[index].Init(item); break;
            case ItemType.Consumable: _consumableSlots[index].Init(item); break;
            case ItemType.Misc: _miscSlots[index].Init(item); break;
        }
    }

    private List<UIInventoryItemSlot> CreateSlots(int capacity, RectTransform parent)
    {
        List<UIInventoryItemSlot> slots = new(capacity);

        for (int i = 0; i < capacity; i++)
        {
            UIInventoryItemSlot slot = Instantiate(_slotPrefab, parent);
            slot.Clear();
            slots.Add(slot);
        }
        return slots;
    }

    public void RemoveSlot(int slotIndex, ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon: _weaponSlots[slotIndex].Clear(); break;
            case ItemType.Armor: _armorSlots[slotIndex].Clear(); break;
            case ItemType.Consumable: _consumableSlots[slotIndex].Clear(); break;
            case ItemType.Misc: _miscSlots[slotIndex].Clear(); break;
        }
    }
}