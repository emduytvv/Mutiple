using TMPro;
using UnityEditor;
using UnityEngine;

public class FixTMPFont
{
    [MenuItem("Tools/Fix TMP Font (All Scenes)")]
    static void FixAllTMPFont()
    {
        var font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        if (font == null)
        {
            Debug.LogError("Không tìm thấy LiberationSans SDF trong Resources!");
            return;
        }

        var texts = Object.FindObjectsByType<TextMeshProUGUI>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);

        int count = 0;
        foreach (var t in texts)
        {
            if (t.font == null)
            {
                t.font = font;
                EditorUtility.SetDirty(t);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Đã fix {count} TMP text objects.");
    }
}
