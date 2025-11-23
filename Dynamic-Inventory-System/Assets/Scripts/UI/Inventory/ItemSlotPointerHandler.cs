using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIInventorySlot))]
public class ItemSlotPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private UIInventorySlot _slotUI;

    private void Awake()
    {
        _slotUI = GetComponent<UIInventorySlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _slotUI?.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _slotUI?.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Let InventorySlotUI handle click logic
        _slotUI?.OnClicked();
    }
}