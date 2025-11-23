using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory; 
    [SerializeField] private GameObject slotPrefab; 

    [Header("Containers")]
    [SerializeField] private RectTransform weaponsContainer;
    [SerializeField] private RectTransform armorsContainer;
    [SerializeField] private RectTransform consumablesContainer;
    [SerializeField] private RectTransform miscsContainer;

    [Header("UI Settings")]
    [SerializeField] private bool clearBeforePopulate = true;

    // Keep track of created slots so we can update/destroy them
    private List<GameObject> _activeSlots = new List<GameObject>();

    private void Start()
    {
        // RefreshAll();
    }

    /// <summary>Call this anytime inventory changed to refresh UI.</summary>
    // public void RefreshAll()
    // {
    //     ClearAllSlots();

    //     PopulateCategory(ItemType.Weapon, weaponsContainer);
    //     PopulateCategory(ItemType.Armor, armorsContainer);
    //     PopulateConsumables(consumablesContainer);
    //     PopulateCategory(ItemType.Misc, miscsContainer);
    // }

    private void ClearAllSlots()
    {
        if (!clearBeforePopulate) return;

        foreach (var go in _activeSlots)
            Destroy(go);

        _activeSlots.Clear();
    }

    // private void PopulateCategory(ItemType type, RectTransform container)
    // {
    //     var items = inventory.GetItems(type);
    //     for (int i = 0; i < items.Count; i++)
    //     {
    //         var config = items[i];
    //         var slotGO = Instantiate(slotPrefab, container);
    //         _activeSlots.Add(slotGO);

    //         var slotUI = slotGO.GetComponent<UIInventorySlot>();
    //         if (slotUI != null)
    //         {
    //             slotUI.Setup(config, 0, i, type, this, tooltip); // non-stackables use count 0/1
    //         }
    //     }
    // }

    // private void PopulateConsumables(RectTransform container)
    // {
    //     var consumableList = inventory.GetItems(ItemType.Consumable);
    //     var stacks = inventory.GetConsumableStacks();

    //     for (int i = 0; i < consumableList.Count; i++)
    //     {
    //         var config = consumableList[i];
    //         int stack = 0;
    //         if (stacks.TryGetValue(config, out int s))
    //             stack = s;

    //         var slotGO = Instantiate(slotPrefab, container);
    //         _activeSlots.Add(slotGO);

    //         var slotUI = slotGO.GetComponent<UIInventorySlot>();
    //         if (slotUI != null)
    //         {
    //             slotUI.Setup(config, stack, i, ItemType.Consumable, this, tooltip);
    //         }
    //     }
    // }

    // Example callback from slot when clicked
    // You can expand this to equip, use, remove, etc.
    public void OnSlotClicked(SOItemConfig config, int slotIndex, ItemType type)
    {
        Debug.Log($"Slot clicked: {config.DisplayName} (index {slotIndex}) type {type}");
        // Example: for consumable, call inventory.RemoveItem(slotIndex, ItemType.Consumable)
        // Then call RefreshAll() to update UI.
    }
}