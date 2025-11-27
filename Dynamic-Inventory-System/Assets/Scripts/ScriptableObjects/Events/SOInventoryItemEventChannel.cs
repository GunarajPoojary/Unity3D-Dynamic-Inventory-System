using UnityEngine;
using System;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Inventory Item Event Channel")]
public class SOInventoryItemEventChannel : ScriptableObject
{
    public event Action<InventoryItem> OnEventRaised;

    public void RaiseEvent(InventoryItem value) => OnEventRaised?.Invoke(value);
}