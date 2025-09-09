using UnityEngine;

/// <summary>
/// Represents the data for an inventory item.
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemConfigSO : DescriptionBaseSO
{
    [Header("General Info")]
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }

    [Header("Stacking Rules")]
    [SerializeField]
    private int _maxStack = 1;

    public int MaxStack => Mathf.Max(1, _maxStack);

    public bool IsStackable => MaxStack > 1;
}