using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemIDDatabase", menuName = "Inventory/IDDatabase")]
public class ItemIDDatabaseSO : ScriptableObject
{
    private int _lastUsedID = 0;

    public int GetNextUniqueID()
    {
        _lastUsedID++;
        EditorUtility.SetDirty(this);  
        return _lastUsedID;
    }
}
