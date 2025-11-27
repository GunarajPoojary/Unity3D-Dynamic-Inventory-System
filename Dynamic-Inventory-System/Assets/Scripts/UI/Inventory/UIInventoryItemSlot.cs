using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIInventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _stackText;
    [SerializeField] private SOInventoryItemEventChannel _slotSelectedEvent;

    private InventoryItem _item;

    public bool IsEmpty => _item == null;

    public void Init(InventoryItem item)
    {
        _item = item;

        if (_icon != null)
            _icon.sprite = _item.ItemConfig.Icon;

        SetStackText(_item.Quantity);

        gameObject.SetActive(true);
    }

    private void SetStackText(int quantity)
    {
        _stackText.text = quantity > 0 ? quantity.ToString() : "";
    }

    public void Clear()
    {
        _item = null;

        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) => _slotSelectedEvent.RaiseEvent(_item);
}