using UnityEngine;
using System;

public enum PopupType
{
    Info,
    Warning,
    Error
}

[CreateAssetMenu(menuName = "Events/Popup Event Channel")]
public class SOPopupEventChannel : ScriptableObject
{
    public event Action<string, PopupType> OnPopupRequested;

    public void RaisePopup(string message, PopupType type)
    {
        OnPopupRequested?.Invoke(message, type);
    }
}
