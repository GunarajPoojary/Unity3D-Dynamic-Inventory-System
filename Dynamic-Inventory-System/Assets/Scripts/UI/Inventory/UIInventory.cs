using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private UIInventoryItemSlot _slotPrefab;
    [SerializeField] private GameObject _itemOverviewPanel;
    [SerializeField] private InventoryTabButton[] _itemTabs;
    [SerializeField] private TMP_Text _itemTabText;

    [Header("Containers")]
    [SerializeField] private RectTransform _weaponsContainer;
    [SerializeField] private RectTransform _armorsContainer;
    [SerializeField] private RectTransform _resourceItemsContainer;

    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _sellButton;

    public event Action<int, ItemType> OnRemoveItemRequested;

    private List<UIInventoryItemSlot> _weaponSlots;
    private List<UIInventoryItemSlot> _armorSlots;
    private List<UIInventoryItemSlot> _resourceSlots;

    private ItemType _selectedType;
    private int _selectedSlot;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(RequestRemove);

        foreach (InventoryTabButton tab in _itemTabs)
            tab.OnSelectTab += t => _itemTabText.text = t.ToString();
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(RequestRemove);
        
        foreach (InventoryTabButton tab in _itemTabs)
            tab.OnSelectTab -= t => _itemTabText.text = t.ToString();
    }

    public void Init(int wc, int ac, int cc)
    {
        _weaponSlots = CreateSlots(wc, _weaponsContainer);
        _armorSlots = CreateSlots(ac, _armorsContainer);
        _resourceSlots = CreateSlots(cc, _resourceItemsContainer);

        _itemOverviewPanel.SetActive(false);
    }

    private List<UIInventoryItemSlot> CreateSlots(int cap, RectTransform parent)
    {
        List<UIInventoryItemSlot> list = new(cap);
        for (int i = 0; i < cap; i++)
        {
            UIInventoryItemSlot slot = Instantiate(_slotPrefab, parent);
            slot.Clear();
            slot.OnItemSelected += HandleSelection;
            list.Add(slot);
        }
        return list;
    }

    private void HandleSelection(int index, InventoryItem item)
    {
        _selectedSlot = index;
        _selectedType = item.ItemType;

        _itemOverviewPanel.SetActive(true);
        _titleText.text = item.DisplayName;
        _descriptionText.text = item.Description;
        _icon.sprite = item.Icon;
    }

    private void RequestRemove()
    {
        OnRemoveItemRequested?.Invoke(_selectedSlot, _selectedType);
        _itemOverviewPanel.SetActive(false);
    }

    public void AddItemSlot(int index, InventoryItem item)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon: _weaponSlots[index].Init(index, item); break;
            case ItemType.Armor: _armorSlots[index].Init(index, item); break;
            case ItemType.Resource: _resourceSlots[index].Init(index, item); break;
        }
    }

    public void RemoveSlot(int index, ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon: _weaponSlots[index].Clear(); break;
            case ItemType.Armor: _armorSlots[index].Clear(); break;
            case ItemType.Resource: _resourceSlots[index].Clear(); break;
        }
    }
}