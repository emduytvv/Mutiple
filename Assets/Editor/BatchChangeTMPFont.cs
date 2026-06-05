using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class BatchChangeTMPFont : EditorWindow
{
    private TMP_FontAsset _newFont;
    private bool _includePrefabs = true;
    private bool _includeScenes = true;

    [MenuItem("Tools/Batch Change TMP Font")]
    static void Open() => GetWindow<BatchChangeTMPFont>("Batch TMP Font");

    void OnGUI()
    {
        GUILayout.Label("Đổi font tất cả TMP trong project", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        _newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Font Asset mới", _newFont, typeof(TMP_FontAsset), false);
        _includeScenes = EditorGUILayout.Toggle("Scene đang mở", _includeScenes);
        _includePrefabs = EditorGUILayout.Toggle("Tất cả Prefabs", _includePrefabs);

        EditorGUILayout.Space();
        GUI.enabled = _newFont != null;

        if (GUILayout.Button("Đổi font"))
        {
            int count = 0;
            if (_includeScenes) count += ChangeInOpenScenes();
            if (_includePrefabs) count += ChangeInPrefabs();
            EditorUtility.DisplayDialog("Xong", $"Đã đổi {count} TMP_Text component.", "OK");
        }

        GUI.enabled = true;

        if (_newFont == null)
            EditorGUILayout.HelpBox("Chọn Font Asset trước.", MessageType.Warning);
    }

    int ChangeInOpenScenes()
    {
        var all = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
        foreach (var t in all)
        {
            Undo.RecordObject(t, "Batch Change TMP Font");
            t.font = _newFont;
            EditorUtility.SetDirty(t);
        }
        EditorSceneManager.MarkAllScenesDirty();
        return all.Length;
    }

    int ChangeInPrefabs()
    {
        int count = 0;
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });

        try
        {
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                EditorUtility.DisplayProgressBar("Đang xử lý prefabs...", path, (float)i / guids.Length);

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var texts = prefab.GetComponentsInChildren<TMP_Text>(true);
                if (texts.Length == 0) continue;

                using (var scope = new PrefabUtility.EditPrefabContentsScope(path))
                {
                    foreach (var t in scope.prefabContentsRoot.GetComponentsInChildren<TMP_Text>(true))
                    {
                        t.font = _newFont;
                        count++;
                    }
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
        }

        return count;
    }
}
