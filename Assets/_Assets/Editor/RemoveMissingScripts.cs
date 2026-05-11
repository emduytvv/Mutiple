using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts
{
    [MenuItem("Tools/Remove Missing Scripts (Scene)")]
    static void RemoveFromScene()
    {
        int removed = 0;
        foreach (var go in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            if (count > 0)
            {
                Debug.Log($"Removed {count} missing script(s) from: {go.name}", go);
                removed += count;
            }
        }
        Debug.Log($"Done. Total removed: {removed}");
    }

    [MenuItem("Tools/Remove Missing Scripts (Prefabs in Project)")]
    static void RemoveFromPrefabs()
    {
        int removed = 0;
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            int count = RemoveFromHierarchy(prefab);
            if (count > 0)
            {
                EditorUtility.SetDirty(prefab);
                Debug.Log($"Removed {count} from prefab: {path}");
                removed += count;
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log($"Done. Total removed from prefabs: {removed}");
    }

    static int RemoveFromHierarchy(GameObject root)
    {
        int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(root);
        foreach (Transform child in root.transform)
            count += RemoveFromHierarchy(child.gameObject);
        return count;
    }
}
