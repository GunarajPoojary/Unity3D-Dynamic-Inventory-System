using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Item Config SO Event Channel")]
public class ItemConfigSOEventChannelSO : DescriptionBaseSO
{
    public UnityAction<ItemConfigSO, int> OnEventRaised;

    public void RaiseEvent(ItemConfigSO item, int quantity)
    {
        OnEventRaised?.Invoke(item, quantity);
    }
}