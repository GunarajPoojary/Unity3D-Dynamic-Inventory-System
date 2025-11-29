public class InventoryPresenter
{
    private readonly Inventory _model;
    private readonly UIInventory _view;
    private readonly SOPopupEventChannel _popupEvent;

    public InventoryPresenter(
        int weaponCap,
        int armorCap,
        int consumableCap,
        UIInventory view,
        SOPopupEventChannel popupEvent)
    {
        _view = view;
        _popupEvent = popupEvent;

        _model = new Inventory(weaponCap, armorCap, consumableCap);
        _view.Init(weaponCap, armorCap, consumableCap);
    }

    public void Subscribe()
    {
        _model.OnItemAdded += HandleItemAdded;
        _model.OnItemRemoved += HandleItemRemoved;

        _model.OnItemAddFailed += HandleAddFailed;
        _model.OnItemPartiallyAdded += HandlePartialAdd;
        _model.OnStackLimitReached += HandleStackLimit;

        _view.OnRemoveItemRequested += RequestItemRemoval;
    }

    public void Unsubscribe()
    {
        _model.OnItemAdded -= HandleItemAdded;
        _model.OnItemRemoved -= HandleItemRemoved;

        _model.OnItemAddFailed -= HandleAddFailed;
        _model.OnItemPartiallyAdded -= HandlePartialAdd;
        _model.OnStackLimitReached -= HandleStackLimit;

        _view.OnRemoveItemRequested -= RequestItemRemoval;
    }

    public AddItemResult AddItem(SOItemConfig config, int amount = 1)
        => _model.AddItem(config, amount);

    private void HandleItemAdded(int index, InventoryItem item)
        => _view.AddItemSlot(index, item);

    private void HandleItemRemoved(int index, ItemType type)
        => _view.RemoveSlot(index, type);

    // POPUP HANDLERS ↓↓↓

    private void HandleAddFailed(SOItemConfig config, int leftover)
    {
        _popupEvent.RaisePopup(
            $"{config.DisplayName} could not be added.\nInventory is full.",
            PopupType.Error
        );
    }

    private void HandlePartialAdd(SOItemConfig config, int leftover)
    {
        _popupEvent.RaisePopup(
            $"Only partially added {config.DisplayName}. Leftover: {leftover}.",
            PopupType.Warning
        );
    }

    private void HandleStackLimit(SOItemConfig config)
    {
        _popupEvent.RaisePopup(
            $"{config.DisplayName} stack is full.",
            PopupType.Info
        );
    }

    private void RequestItemRemoval(int slot, ItemType type)
        => _model.RemoveItem(slot, type);
}