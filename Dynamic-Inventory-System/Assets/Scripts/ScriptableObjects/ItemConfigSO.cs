#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemConfigSO : DescriptionBaseSO
{
    public int ID = 0; // Default is set 0, which means the ID is unassigned.

    [Header("Stacking Rules")]
    [Range(1, 99)][SerializeField] private int _maxStack = 1;

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
    public void Initialize(int id,string itemName, int maxStack)
    {
        ID = id;
        ItemName = itemName;
        _maxStack = maxStack;
    }
#endif
}