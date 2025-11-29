using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIInventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _stackText;

    private int _slotIndex;
    private InventoryItem _item;

    public event Action<int, InventoryItem> OnItemSelected;

    public bool IsEmpty => _item == null;

    public void Init(int slotIndex, InventoryItem item)
    {
        _slotIndex = slotIndex;
        _item = item;

        _icon.sprite = item.Icon;
        _stackText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        _item = null;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData e)
        => OnItemSelected?.Invoke(_slotIndex, _item);
}