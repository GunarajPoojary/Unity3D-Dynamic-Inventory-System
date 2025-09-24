using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private Button _button;
    [SerializeField] private ItemConfigSOEventChannelSO _itemConfigSOEventChannelSO;

    private ItemConfigSO _itemConfigSO;
    private int _quantity = 1;

    public void Initialize(ItemConfigSO itemConfigSO, int quantity = 1)
    {
        _itemConfigSO = itemConfigSO;
        _quantity = quantity;

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

        gameObject.SetActive(false);
    }
}