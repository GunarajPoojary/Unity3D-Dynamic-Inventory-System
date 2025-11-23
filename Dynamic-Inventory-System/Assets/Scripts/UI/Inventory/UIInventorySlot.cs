using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventorySlot : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text stackText;
    [SerializeField] private Button button;

    private SOItemConfig _config;
    private int _slotIndex;
    private ItemType _type;
    private UIInventory _inventoryUI;
    private UITooltip _tooltip;

    public void Setup(SOItemConfig config, int stackCount, int slotIndex, ItemType type, UIInventory controller, UITooltip tooltip = null)
    {
        _config = config;
        _slotIndex = slotIndex;
        _type = type;
        _inventoryUI = controller;
        _tooltip = tooltip;

        if (icon != null)
            icon.sprite = config.Icon;

        if (stackText != null)
        {
            stackText.gameObject.SetActive(config.IsStackable);
            stackText.text = config.IsStackable ? stackCount.ToString() : string.Empty;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    public void OnClicked()
    {
        if (_inventoryUI != null && _config != null)
            _inventoryUI.OnSlotClicked(_config, _slotIndex, _type);
    }

    // Called by pointer handlers to show tooltip
    public void ShowTooltip()
    {
        if (_tooltip != null && _config != null)
            _tooltip.Show(_config.DisplayName, _config.Description, transform as RectTransform);
    }

    public void HideTooltip()
    {
        if (_tooltip != null)
            _tooltip.Hide();
    }
}