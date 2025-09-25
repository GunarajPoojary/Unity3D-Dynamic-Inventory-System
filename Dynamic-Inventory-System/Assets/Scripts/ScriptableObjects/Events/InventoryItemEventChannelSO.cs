using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Inventory Item Event Channel")]
public class InventoryItemEventChannelSO : DescriptionBaseSO
{
    public UnityAction<InventoryItem> OnEventRaised;

    public void RaiseEvent(InventoryItem item)
    {
        OnEventRaised?.Invoke(item);
    }
}