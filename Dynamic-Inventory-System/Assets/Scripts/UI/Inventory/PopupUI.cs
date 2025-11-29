using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private SOPopupEventChannel _popupChannel;

    private void OnEnable()
        => _popupChannel.OnPopupRequested += ShowPopup;

    private void OnDisable()
        => _popupChannel.OnPopupRequested -= ShowPopup;

    private void ShowPopup(string message, PopupType type)
    {
        // Implement later:
        // Display message with color-coded popup window
        Debug.Log($"POPUP [{type}]: {message}");
    }
}
