using System;

public static class InventoryUtility
{
    private const string ItemNullErrorMessage = "ItemConfigSO cannot be null.";
    private const string InvalidQuantityErrorMessage = "Quantity must be greater than zero.";

    #region Validation Methods
    /// <summary>
    /// Validates the ItemConfigSO parameter to ensure it is not null
    /// </summary>
    /// <param name="worldItem">The item to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when worldItem is null</exception>
    public static void ValidateItemConfig(ItemConfigSO worldItem)
    {
        if (worldItem == null)
            throw new ArgumentNullException(nameof(worldItem), ItemNullErrorMessage);
    }

    /// <summary>
    /// Validates the quantity parameter to ensure it is greater than zero
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    /// <exception cref="ArgumentException">Thrown when quantity is less than 1</exception>
    public static void ValidateQuantity(int quantity)
    {
        if (quantity < 1)
            throw new ArgumentException(InvalidQuantityErrorMessage, nameof(quantity));
    }
    #endregion
}