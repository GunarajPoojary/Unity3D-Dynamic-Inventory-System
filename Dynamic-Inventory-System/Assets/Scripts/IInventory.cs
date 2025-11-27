using System.Collections.Generic;

public interface IInventory
{
    int WeaponCapacity { get; }
    int ArmorsCapacity { get; }
    int ConsumableCapacity { get; }
    int MiscsCapacity { get; }

    IReadOnlyList<InventoryItem> Weapons { get; }
    IReadOnlyList<InventoryItem> Armors { get; }
    IReadOnlyList<InventoryItem> Consumables { get; }
    IReadOnlyList<InventoryItem> Miscs { get; }
}
