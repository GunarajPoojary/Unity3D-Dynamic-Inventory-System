using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItemOverview : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _sellButton;

    [Header("Broadcasting")]
    [SerializeField] private SOInventoryItemEventChannel _removeItemEvent;
    [SerializeField] private SOInventoryItemEventChannel _selectItemEvent;

    private InventoryItem _currentSelectedItem;

    private void OnEnable() => AddListeners();
    private void OnDisable() => RemoveListeners();

    public void DisplayItemOverview(InventoryItem item)
    {
        _currentSelectedItem = item;

        _titleText.text = _currentSelectedItem.ItemConfig.DisplayName;
        _descriptionText.text = _currentSelectedItem.ItemConfig.Description;
        _itemIcon.sprite = _currentSelectedItem.ItemConfig.Icon;
    }

    private void AddListeners()
    {
        _useButton.onClick.AddListener(OnUseButtonClick);
        _sellButton.onClick.AddListener(OnSellButtonClick);
        _selectItemEvent.OnEventRaised += DisplayItemOverview;
    }

    private void RemoveListeners()
    {
        _useButton.onClick.RemoveListener(OnUseButtonClick);
        _sellButton.onClick.RemoveListener(OnSellButtonClick);
        _selectItemEvent.OnEventRaised += DisplayItemOverview;
    }

    private void OnSellButtonClick()
    {
        if (_currentSelectedItem != null)
            Debug.Log($"Sell {_currentSelectedItem.ItemConfig.DisplayName}");

        _removeItemEvent.RaiseEvent(_currentSelectedItem);

        gameObject.SetActive(false);
    }

    private void OnUseButtonClick()
    {
        if (_currentSelectedItem != null)
            Debug.Log($"Use {_currentSelectedItem.ItemConfig.DisplayName}");
    }
}