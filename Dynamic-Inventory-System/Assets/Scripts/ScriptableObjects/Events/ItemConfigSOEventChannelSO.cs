using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Item Config SO Event Channel")]
public class ItemConfigSOEventChannelSO : DescriptionBaseSO
{
    public UnityAction<ItemConfigSO, int> OnEventRaised;

    public void RaiseEvent(ItemConfigSO item, int quantity)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(item, quantity);
        }
        else
        {
            Debug.LogWarning("A ItemConfigSO event was requested, but nobody picked it up. " +
                "Check why there is no SceneLoader already present, " +
                "and make sure it's listening on this Load Event channel.");
        }
    }
}