using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private SOItemConfig _config;
    [SerializeField] private SOItemConfigEventChannel _pickupEvent;

[ContextMenu("Pickup")]
    public void PickUp()
    {
        _pickupEvent.RaiseEvent(_config);
    }
}