using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] protected Image icon;
    [SerializeField] protected Button button;

    protected SOItemConfig _config;
    protected int _slotIndex;
    protected ItemType _type;

    public void Init(SOItemConfig config, int slotIndex, ItemType type)
    {
        _config = config;
        _slotIndex = slotIndex;
        _type = type;

        if (icon != null)
            icon.sprite = config.Icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    public void OnClicked()
    {
        
    }
}