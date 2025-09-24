using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemConfigSO : DescriptionBaseSO
{
    [Tooltip("Default is set 0, which means the ID is unassigned.")]
    [SerializeField] private int _id = 0; 
    public int ID => _id;

    [Header("Stacking Rules")]
    [Tooltip("Default is set 1, which means Non-Stackable")]
    [Range(1, 9)][SerializeField] private int _maxStack = 1;

    [Header("General Info")]
    [field: SerializeField] public string ItemName { get; private set; }
    [field: TextArea(2, 5)][field: SerializeField] public string ItemDescription { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }

    public int MaxStack => _maxStack;
    public bool IsStackable => _maxStack > 1;

#if UNITY_EDITOR
    /// <summary>
    /// Editor-only method to initialize item properties for testing.
    /// </summary>
    public void Initialize(int id, string itemName, int maxStack)
    {
        _id = id;
        ItemName = itemName;
        _maxStack = maxStack;
    }
#endif
}