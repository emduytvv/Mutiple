#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class PackageReorganizer : EditorWindow
{
    string _folder = "";

    [MenuItem("Tools/Reorganize by Package")]
    static void Open() => GetWindow<PackageReorganizer>("Reorganize by Package");

    void OnGUI()
    {
        GUILayout.Label("Folder đã extract từ .unitypackage:");
        _folder = GUILayout.TextField(_folder, GUILayout.Height(24));

        if (GUILayout.Button("Browse", GUILayout.Height(28)))
            _folder = EditorUtility.OpenFolderPanel("Chọn folder extracted", "", "");

        GUILayout.Space(10);

        if (GUILayout.Button("Di chuyển về đúng folder", GUILayout.Height(36)))
            Run();
    }

    void Run()
    {
        if (!Directory.Exists(_folder)) { Debug.LogError("Folder không tồn tại!"); return; }

        int moved = 0, skipped = 0, missing = 0;

        AssetDatabase.StartAssetEditing();
        try
        {
            foreach (var guidDir in Directory.GetDirectories(_folder))
            {
                string guid        = Path.GetFileName(guidDir);
                string pathFile    = Path.Combine(guidDir, "pathname");
                if (!File.Exists(pathFile)) continue;

                string targetPath  = File.ReadAllText(pathFile).Trim();
                string currentPath = AssetDatabase.GUIDToAssetPath(guid);

                if (string.IsNullOrEmpty(currentPath)) { missing++; continue; }
                if (currentPath == targetPath)          { skipped++; continue; }

                // Tạo folder đích nếu chưa có
                string targetDir = Path.GetDirectoryName(targetPath).Replace('\\', '/');
                if (!AssetDatabase.IsValidFolder(targetDir))
                    Directory.CreateDirectory(Path.Combine(Application.dataPath, "..",  targetDir));

                string err = AssetDatabase.MoveAsset(currentPath, targetPath);
                if (string.IsNullOrEmpty(err)) { moved++; Debug.Log($"Moved: {currentPath}  →  {targetPath}"); }
                else                            Debug.LogWarning($"Lỗi move {currentPath}: {err}");
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }

        string msg = $"Xong!\nDi chuyển: {moved}\nĐã đúng chỗ: {skipped}\nKhông tìm thấy: {missing}";
        Debug.Log(msg);
        EditorUtility.DisplayDialog("Hoàn tất", msg, "OK");
    }
}
#endif
