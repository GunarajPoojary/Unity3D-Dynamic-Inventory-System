using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemConfigSO))]
public class ItemConfigSOEditor : Editor
{
    private const string ASSET_FILTER = "t:ItemConfigSO";

    private void OnEnable()
    {
        AssignUniqueIDsToAllItems();
    }

    private void AssignUniqueIDsToAllItems()
    {
        string[] guids = AssetDatabase.FindAssets(ASSET_FILTER);

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            ItemConfigSO item = AssetDatabase.LoadAssetAtPath<ItemConfigSO>(path);

            if (item != null)
            {
                SerializedObject serializedObject = new SerializedObject(item);
                SerializedProperty idProperty = serializedObject.FindProperty("_id");

                if (idProperty != null)
                {
                    idProperty.intValue = -(i + 1);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(item);
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Unique IDs assigned to all ItemConfigSO assets.");
    }
}