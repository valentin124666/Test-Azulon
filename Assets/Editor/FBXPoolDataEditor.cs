using UnityEngine;
using UnityEditor;
using System;
using Data;
using FBXPool;

[CustomEditor(typeof(FBXPoolData))]
public class FBXPoolDataEditor : UnityEditor.Editor
{
    private bool[] _foldouts;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        FBXPoolData fbxData = (FBXPoolData)target;

        if (fbxData.FBXPoolPrefabs == null)
            fbxData.FBXPoolPrefabs = new System.Collections.Generic.List<FBXPoolPrefab>();

        if (_foldouts == null || _foldouts.Length != fbxData.FBXPoolPrefabs.Count)
            _foldouts = new bool[fbxData.FBXPoolPrefabs.Count];

        for (int i = 0; i < fbxData.FBXPoolPrefabs.Count; i++)
        {
            var item = fbxData.FBXPoolPrefabs[i];
            string label = string.IsNullOrEmpty(item.locationSuffix) ? "Item" : item.locationSuffix;
            _foldouts[i] = EditorGUILayout.Foldout(_foldouts[i], label, true);

            if (_foldouts[i])
            {
                EditorGUI.indentLevel++;

                // Редаговані вручну поля
                item.numberOfExamples = EditorGUILayout.IntField("Number of Examples", item.numberOfExamples);

                // Автоматично згенеровані поля
                EditorGUILayout.LabelField("View Type", item.viewType);
                EditorGUILayout.LabelField("Location Suffix", item.locationSuffix);

                // Вибір префабу
                var newPrefab = (FBXPoolItem)EditorGUILayout.ObjectField("FBX Prefab", null, typeof(FBXPoolItem), false);

                if (newPrefab != null)
                {
                    Type type = newPrefab.GetType();
                    item.viewType = type.FullName;
                    item.locationSuffix = newPrefab.gameObject.name;
                    item.FbxPrefab = newPrefab;

                    EditorUtility.SetDirty(fbxData);
                }

                // Кнопка видалення
                if (GUILayout.Button("Remove FBX"))
                {
                    fbxData.FBXPoolPrefabs.RemoveAt(i);
                    _foldouts = new bool[fbxData.FBXPoolPrefabs.Count];
                    EditorUtility.SetDirty(fbxData);
                    return;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        // Кнопка додавання нового
        if (GUILayout.Button("Add FBX"))
        {
            fbxData.FBXPoolPrefabs.Add(new FBXPoolPrefab());
            _foldouts = new bool[fbxData.FBXPoolPrefabs.Count];
            EditorUtility.SetDirty(fbxData);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
