using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/Inventory Item Event Channel")]
public class SOItemConfigEventChannel : ScriptableObject
{
    public event Action<SOItemConfig> OnEventRaised;

    public void RaiseEvent(SOItemConfig value) => OnEventRaised?.Invoke(value);
}