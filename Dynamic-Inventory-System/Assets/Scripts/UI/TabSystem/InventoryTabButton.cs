using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTabButton : UITabButton
{
    [SerializeField] private ItemType _type;

    public event Action<ItemType> OnSelectTab;

    public override void OnPointerClick(PointerEventData e)
    {
        base.OnPointerClick(e);
        OnSelectTab?.Invoke(_type);
    }
}