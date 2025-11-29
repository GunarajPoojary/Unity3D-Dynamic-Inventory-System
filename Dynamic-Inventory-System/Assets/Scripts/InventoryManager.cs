using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private int _weaponsCap = 40;
    [SerializeField] private int _armorsCap = 40;
    [SerializeField] private int _resourceItemsCap = 40;

    [Header("UI References")]
    [SerializeField] private UIInventory _uiInventory;

    [Header("Global UI Event Channels")]
    [SerializeField] private SOPopupEventChannel _popupChannel;
    [SerializeField] private SOItemConfigEventChannel _pickupEvent;

    private InventoryPresenter _presenter;

    private void Awake()
    {
        _presenter = new InventoryPresenter(
            _weaponsCap,
            _armorsCap,
            _resourceItemsCap,
            _uiInventory,
            _popupChannel
        );
    }



    private void OnEnable()
    {
        _presenter.Subscribe();
        _pickupEvent.OnEventRaised += AddItem;
    }

    private void AddItem(SOItemConfig config)
    {
        _presenter.AddItem(config);
    }

    private void OnDisable()
    {
        _presenter.Unsubscribe();
        _pickupEvent.OnEventRaised -= AddItem;
    }
}