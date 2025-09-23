using UnityEditor;

[CustomEditor(typeof(ItemConfigSO))]
public class ItemConfigSOEditor : Editor
{
    private const string ASSET_FILTER = "t:ItemConfigSO";
    private void OnEnable() => AssignUniqueIDsToAllItems();

    private void AssignUniqueIDsToAllItems()
    {
        string[] guids = AssetDatabase.FindAssets(ASSET_FILTER);

        ItemConfigSO[] items = new ItemConfigSO[guids.Length];

        for (int i = 0; i < items.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            items[i] = AssetDatabase.LoadAssetAtPath<ItemConfigSO>(path);

            ItemConfigSO item = items[i];

            if (item != null && !string.IsNullOrEmpty(item.name))
            {
                item.ID = -(i + 1);
                EditorUtility.SetDirty(item);
            }
        }

        AssetDatabase.SaveAssets();
    }
}