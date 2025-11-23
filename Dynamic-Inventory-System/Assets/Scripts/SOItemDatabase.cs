using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Database")]
public class SOItemDatabase : ScriptableObject
{
    [field: SerializeField] public List<SOItemConfig> Items { get; private set; }
}