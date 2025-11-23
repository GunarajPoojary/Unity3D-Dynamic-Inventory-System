using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Database")]
public class SOItemDatabase : ScriptableObject
{
    [SerializeField] private SOItemConfig[] _items;
}