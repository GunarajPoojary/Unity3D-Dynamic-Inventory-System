using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private ItemConfigSO _itemConfigSO;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private Button _button;
    [SerializeField] private ItemConfigSOEventChannelSO _itemConfigSOEventChannelSO;
    [Range(1, 9)][SerializeField] private int _quantity = 1;

    private void Awake()
    {
        if (_itemConfigSO != null)
        {
            _itemImage.sprite = _itemConfigSO.Icon;
            _itemName.text = _itemConfigSO.ItemName;
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _itemConfigSOEventChannelSO.RaiseEvent(_itemConfigSO, _quantity);
    }
}