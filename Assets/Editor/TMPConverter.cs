using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TMPConverter : EditorWindow
{
    [MenuItem("Tools/Convert Text → TMP")]
    static void Open() => GetWindow<TMPConverter>("TMP Converter");

    void OnGUI()
    {
        GUILayout.Label("Convert legacy Text → TextMeshPro", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Convert tất cả Text component trong scene đang mở.\nGiữ nguyên: text, font size, color, alignment.", MessageType.Info);
        EditorGUILayout.Space();

        if (GUILayout.Button("Convert All Text → TMP", GUILayout.Height(40)))
        {
            int count = ConvertAll();
            EditorSceneManager.MarkAllScenesDirty();
            EditorUtility.DisplayDialog("Xong", $"Đã convert {count} Text component.", "OK");
        }
    }

    int ConvertAll()
    {
        var texts = FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int count = 0;
        foreach (var t in texts)
        {
            ConvertOne(t);
            count++;
        }
        return count;
    }

    void ConvertOne(Text text)
    {
        var go = text.gameObject;
        string content = text.text;
        float fontSize = text.fontSize;
        Color color = text.color;
        bool bold = (text.fontStyle & FontStyle.Bold) != 0;
        bool italic = (text.fontStyle & FontStyle.Italic) != 0;
        TextAnchor anchor = text.alignment;
        bool raycast = text.raycastTarget;

        Undo.RecordObject(go, "Convert Text to TMP");
        Undo.DestroyObjectImmediate(text);

        var tmp = Undo.AddComponent<TextMeshProUGUI>(go);
        tmp.text = content;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.raycastTarget = raycast;
        tmp.fontStyle = (bold ? FontStyles.Bold : FontStyles.Normal)
                      | (italic ? FontStyles.Italic : FontStyles.Normal);

        tmp.alignment = anchor switch
        {
            TextAnchor.UpperLeft    => TextAlignmentOptions.TopLeft,
            TextAnchor.UpperCenter  => TextAlignmentOptions.Top,
            TextAnchor.UpperRight   => TextAlignmentOptions.TopRight,
            TextAnchor.MiddleLeft   => TextAlignmentOptions.Left,
            TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
            TextAnchor.MiddleRight  => TextAlignmentOptions.Right,
            TextAnchor.LowerLeft    => TextAlignmentOptions.BottomLeft,
            TextAnchor.LowerCenter  => TextAlignmentOptions.Bottom,
            TextAnchor.LowerRight   => TextAlignmentOptions.BottomRight,
            _                       => TextAlignmentOptions.Center
        };

        EditorUtility.SetDirty(go);
    }
}
