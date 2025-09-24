using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [Header("Pooling Settings")]
    [SerializeField] private ShopItemUI _shopItemPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private int _itemsCount = 24;

    [Header("Shop Settings")]
    [SerializeField] private ItemConfigSO[] _availableItems;
    private List<ShopItemUI> _activeItems = new List<ShopItemUI>();

    private void Awake()
    {
        for (int i = 0; i < _itemsCount; i++)
        {
            CreateShopItem();
        }
    }

    private ShopItemUI CreateShopItem()
    {
        ShopItemUI newItem = Instantiate(_shopItemPrefab, _container);
        newItem.Initialize(_availableItems[Random.Range(0, _availableItems.Length)], Random.Range(1, 10));
        _activeItems.Add(newItem);
        return newItem;
    }

    public void Refresh()
    {
        foreach (ShopItemUI item in _activeItems)
        {
            item.Initialize(_availableItems[Random.Range(0, _availableItems.Length)], Random.Range(1, 10));
            item.gameObject.SetActive(true);
        }
    }
}