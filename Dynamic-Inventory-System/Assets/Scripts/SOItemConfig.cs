using UnityEngine;

public enum ItemType
{
    None,
    Weapon,
    Armor,
    Consumable,
    Misc
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Configuration")]
public class SOItemConfig : ScriptableObject
{
    public string ItemID { get; set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [TextArea][field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField, Range(1, 99)] public int MaxStack { get; private set; } = 1;

    public bool IsStackable => Type == ItemType.Consumable;
    public bool IsEquipable => Type == ItemType.Weapon || Type == ItemType.Armor;

    private void OnValidate()
    {
        if (Type == ItemType.Weapon || Type == ItemType.Armor || Type == ItemType.Misc)
        {
            MaxStack = 1;
        }
    }
}