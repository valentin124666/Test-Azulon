

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.IO;

public static class ValidateLevelAddressables
{
    [MenuItem("Tools/Validate Level Addressables")]
    public static void ValidateLevels()
    {
        // var settings = AddressableAssetSettingsDefaultObject.Settings;
        // if (settings == null)
        // {
        //     Debug.LogError("Addressable settings not found.");
        //     return;
        // }
        //
        // var group = settings.FindGroup("Levels");
        // if (group == null)
        // {
        //     Debug.LogError("Group 'Level' not found.");
        //     return;
        // }
        //
        // var errors = new List<string>();
        //
        // foreach (var entry in group.entries)
        // {
        //     var assetPath = AssetDatabase.GUIDToAssetPath(entry.guid);
        //     var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        //
        //     if (prefab == null)
        //     {
        //         errors.Add($"❌ Cannot load prefab at: {assetPath}");
        //         continue;
        //     }
        //
        //     if (!prefab.TryGetComponent<LevelBehaviour>(out var _))
        //     {
        //         errors.Add($"❌ Prefab '{prefab.name}' does not have LevelBehaviour on root.");
        //         continue;
        //     }
        //
        //     // Optional: check for correct address name format
        //     if (string.IsNullOrWhiteSpace(entry.address))
        //     {
        //         errors.Add($"⚠️ Prefab '{prefab.name}' has no address assigned.");
        //     }
        // }
        //
        // if (errors.Count == 0)
        // {
        //     Debug.Log("✅ All level prefabs are valid.");
        // }
        // else
        // {
        //     Debug.LogWarning("⚠️ Issues found with Level addressables:\n" + string.Join("\n", errors));
        // }
    }
}
#endif

