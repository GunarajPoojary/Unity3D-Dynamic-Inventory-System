using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Capacity")]
    [SerializeField] private int _weaponCapacity = 40;
    [SerializeField] private int _armorsCapacity = 40;
    [SerializeField] private int _consumableCapacity = 40;
    [SerializeField] private int _miscsCapacity = 40;

    private List<SOItemConfig> _weapons;
    private List<SOItemConfig> _armors;

    private List<SOItemConfig> _consumables;

    private Dictionary<SOItemConfig, int> _consumablesStackMap;

    private List<SOItemConfig> _miscs;

    private void Awake()
    {
        _weapons = new List<SOItemConfig>(_weaponCapacity);
        _armors = new List<SOItemConfig>(_armorsCapacity);

        _consumables = new List<SOItemConfig>(_consumableCapacity);
        _consumablesStackMap = new Dictionary<SOItemConfig, int>(_consumableCapacity);

        _miscs = new List<SOItemConfig>(_miscsCapacity);
    }

    public IReadOnlyList<SOItemConfig> GetItems(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return _weapons.AsReadOnly();
            case ItemType.Armor:
                return _armors.AsReadOnly();
            case ItemType.Consumable:
                return _consumables.AsReadOnly();
            case ItemType.Misc:
                return _miscs.AsReadOnly();
            default:
                return new List<SOItemConfig>().AsReadOnly();
        }
    }

    public IReadOnlyDictionary<SOItemConfig, int> GetConsumableStacks()
    {
        return new ReadOnlyDictionary<SOItemConfig, int>(_consumablesStackMap);
    }

    public void AddItem(SOItemConfig config, int amount = 1)
    {
        if (config == null)
        {
            Debug.LogError("Trying to add NULL item to inventory.");
            return;
        }

        switch (config.Type)
        {
            case ItemType.Weapon:
                AddNonStackableItem(_weapons, config, amount, _weaponCapacity);
                break;

            case ItemType.Armor:
                AddNonStackableItem(_armors, config, amount, _armorsCapacity);
                break;

            case ItemType.Consumable:
                AddConsumable(config, amount);
                break;

            case ItemType.Misc:
                AddNonStackableItem(_miscs, config, amount, _miscsCapacity);
                break;
        }
    }

    private void AddNonStackableItem(List<SOItemConfig> list, SOItemConfig config, int amount, int capacity)
    {
        int freeSpace = capacity - list.Count;
        int amountToAdd = Mathf.Min(amount, freeSpace);

        for (int i = 0; i < amountToAdd; i++)
            list.Add(config);

        if (amountToAdd < amount)
            Debug.LogWarning($"Inventory full for {config.Type}. Added {amountToAdd}/{amount}.");
    }

    private void AddConsumable(SOItemConfig config, int amount)
    {
        if (_consumablesStackMap.TryGetValue(config, out int currentStack))
        {
            int maxStack = config.MaxStack;
            int canAdd = Mathf.Min(amount, maxStack - currentStack);

            _consumablesStackMap[config] = currentStack + canAdd;

            if (canAdd < amount)
                Debug.LogWarning($"Consumable full for {config.DisplayName}. Added {canAdd}/{amount}.");

            return;
        }

        if (_consumables.Count >= _consumableCapacity)
        {
            Debug.LogWarning("Consumables inventory full!");
            return;
        }

        int maxStackNew = config.MaxStack;
        int addAmount = Mathf.Min(amount, maxStackNew);

        _consumablesStackMap[config] = addAmount;

        if (addAmount < amount)
            Debug.LogWarning($"Consumable full for {config.DisplayName}. Added {addAmount}/{amount}.");
    }

    public void RemoveItem(int slotIndex, ItemType type, int amount = 1)
    {
        switch (type)
        {
            case ItemType.Weapon:
                RemoveNonStackable(_weapons, slotIndex);
                break;

            case ItemType.Armor:
                RemoveNonStackable(_armors, slotIndex);
                break;

            case ItemType.Consumable:
                RemoveConsumable(slotIndex, amount);
                break;

            case ItemType.Misc:
                RemoveNonStackable(_miscs, slotIndex);
                break;
        }
    }

    private void RemoveNonStackable(List<SOItemConfig> list, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= list.Count)
        {
            Debug.LogWarning($"Invalid slotIndex {slotIndex}");
            return;
        }

        list.RemoveAt(slotIndex);
    }

    private void RemoveConsumable(int slotIndex, int amount)
    {
        if (slotIndex < 0 || slotIndex >= _consumables.Count)
        {
            Debug.LogWarning($"Invalid consumable slotIndex {slotIndex}");
            return;
        }

        SOItemConfig itemConfig = _consumables[slotIndex];

        if (!_consumablesStackMap.TryGetValue(itemConfig, out int currentStack))
            return;

        int toRemove = Mathf.Min(amount, currentStack);

        currentStack -= toRemove;

        if (currentStack <= 0)
        {
            _consumablesStackMap.Remove(itemConfig);
            _consumables.RemoveAt(slotIndex);
        }
        else
        {
            _consumablesStackMap[itemConfig] = currentStack;
        }
    }

    [ContextMenu("Print Items")]
    private void PrintItems()
    {
        Debug.Log("=== INVENTORY CONTENTS ===");

        Debug.Log("-- Weapons --");
        for (int i = 0; i < _weapons.Count; i++)
            Debug.Log($"[{i}] {_weapons[i].DisplayName}");

        Debug.Log("-- Armors --");
        for (int i = 0; i < _armors.Count; i++)
            Debug.Log($"[{i}] {_armors[i].DisplayName}");

        Debug.Log("-- Consumables --");
        for (int i = 0; i < _consumables.Count; i++)
        {
            var item = _consumables[i];
            int stack = _consumablesStackMap[item];
            Debug.Log($"[{i}] {item.DisplayName} x{stack}");
        }

        Debug.Log("-- Misc --");
        for (int i = 0; i < _miscs.Count; i++)
            Debug.Log($"[{i}] {_miscs[i].DisplayName}");

        Debug.Log("=== END OF INVENTORY ===");
    }
}