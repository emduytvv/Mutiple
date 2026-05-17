using UnityEngine;
using UnityEditor;

public class FixAnimations
{
    [MenuItem("Tools/Remove Legacy From All Clips")]
    static void Fix()
    {
        string[] guids = AssetDatabase.FindAssets("t:AnimationClip");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip != null && clip.legacy)
            {
                clip.legacy = false;
                EditorUtility.SetDirty(clip);
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Done! All legacy removed.");
    }
}