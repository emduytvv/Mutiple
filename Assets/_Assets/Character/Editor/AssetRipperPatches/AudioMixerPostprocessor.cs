using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRipperPatches.Editor
{
	/// <summary>
	/// This script is AssetRipper's patch for exported audio effects to recover effect parameter names when Unity imports each audio mixer.
	/// Unity does not serialize the parameter names in a release asset, so it is impossible to recover them by AssetRipper.
	/// Fortunately, there is an internal function <c>AudioMixerEffectController.PreallocateGUIDs</c> in UnityEditor.dll, which can help us.
	/// This function is used by Unity Editor when creating a new audio effect. It collects a list of runtime audio effects,
	/// retrieves parameter definitions for each, and updates the parameter names and GUIDs in the caller AudioMixerEffectController.
	/// Moreover, this function won't update the GUID for a parameter if it already has a non-empty GUID,
	/// which is the case in exported audio effects, perfectly matching our needs.
	/// </summary>
	public class AudioMixerPostprocessor : AssetPostprocessor
	{
		private static readonly Type AudioMixerEffectControllerType;
		private static readonly MethodInfo PreallocateGUIDsMethod;
		static AudioMixerPostprocessor()
		{
			Assembly editorAssembly = typeof(AssetPostprocessor).Assembly;
			AudioMixerEffectControllerType = editorAssembly.GetType("UnityEditor.Audio.AudioMixerEffectController", true);
			PreallocateGUIDsMethod = AudioMixerEffectControllerType.GetMethod("PreallocateGUIDs", BindingFlags.Public | BindingFlags.Instance);
			if (PreallocateGUIDsMethod == null)
			{
				Debug.LogError("AudioMixerEffectController.PreallocateGUIDs() method is missing in this version of Unity. Audio effect parameter values will be reset to default.");
			}
		}

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			if (PreallocateGUIDsMethod == null) return;

			foreach (string importedAsset in importedAssets)
			{
				if (importedAsset.EndsWith(".mixer"))
				{
					foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath(importedAsset))
					{
						if (asset.GetType() == AudioMixerEffectControllerType)
						{
							PreallocateGUIDsMethod.Invoke(asset, new object[0]);
							EditorUtility.SetDirty(asset);
						}
					}
				}
			}
		}
	}
}