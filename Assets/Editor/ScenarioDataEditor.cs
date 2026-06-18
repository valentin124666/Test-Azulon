using System;
using System.Collections.Generic;
using Core;
using Data;
using Level;
using Modules;
using Tools;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ScenarioData))]
    public class ScenarioDataEditor : UnityEditor.Editor
    {
        private ReorderableList list;
        private Dictionary<int, bool> bindingFoldouts = new();
        private Dictionary<int, bool> playerFoldouts = new();
        private Dictionary<int, bool> NpcFoldouts = new();
        private Dictionary<int, bool> levelFoldouts = new();

        private void OnEnable()
        {
            list = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("bindings"),
                true, true, true, true
            );

            list.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "State Scene Bindings"); };

            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                float y = rect.y;
                float lineHeight = EditorGUIUtility.singleLineHeight;
                float spacing = EditorGUIUtility.standardVerticalSpacing;

                if (!bindingFoldouts.ContainsKey(index)) bindingFoldouts[index] = false;
                bindingFoldouts[index] = EditorGUI.Foldout(
                    new Rect(rect.x, y, rect.width, lineHeight),
                    bindingFoldouts[index],
                    $"Binding {index + 1}",
                    true
                );
                y += lineHeight + spacing;

                if (bindingFoldouts[index])
                {
                    SerializedProperty player = element.FindPropertyRelative("playerPrefab");
                    SerializedProperty npc = element.FindPropertyRelative("npcPrefab");
                    SerializedProperty level = element.FindPropertyRelative("levelPrefab");

                    DrawPrefabSection(index, ref y, rect, "Player", player, typeof(PlayerPresenterView), playerFoldouts, "playerPrefab", "playerReferencePrefab");
                    DrawLevelSection(index, ref y, rect, level);
                }
            };

            list.elementHeightCallback = index =>
            {
                float baseHeight = EditorGUIUtility.singleLineHeight + 6;
                float sectionHeight = EditorGUIUtility.singleLineHeight * 6 + 80;
                float height = baseHeight;

                if (bindingFoldouts.TryGetValue(index, out var open) && open)
                {
                    if (playerFoldouts.TryGetValue(index, out var open1) && open1)
                        height += sectionHeight;
                    else
                        height += EditorGUIUtility.singleLineHeight + 6;

                    if (levelFoldouts.TryGetValue(index, out var open4) && open4)
                        height += sectionHeight;
                    else
                        height += EditorGUIUtility.singleLineHeight + 6;
                }

                return height;
            };
        }

        private void DrawLevelSection(int index, ref float y, Rect rect, SerializedProperty levelProp)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            if (!levelFoldouts.ContainsKey(index)) levelFoldouts[index] = true;

            levelFoldouts[index] = EditorGUI.Foldout(
                new Rect(rect.x + 10, y, rect.width - 10, lineHeight),
                levelFoldouts[index],
                "Level Prefab",
                true
            );
            y += lineHeight + spacing;

            if (!levelFoldouts[index]) return;

            var sceneAssetProp = levelProp.FindPropertyRelative("sceneAsset");
            var scenePathProp = levelProp.FindPropertyRelative("sceneName");

            // Draw SceneAsset field
            EditorGUI.BeginChangeCheck();
            var newSceneAsset = EditorGUI.ObjectField(
                new Rect(rect.x + 10, y, rect.width - 10, lineHeight),
                "Scene Asset",
                sceneAssetProp.objectReferenceValue,
                typeof(SceneAsset),
                false
            ) as SceneAsset;
            y += lineHeight + spacing;

            if (EditorGUI.EndChangeCheck())
            {
                sceneAssetProp.objectReferenceValue = newSceneAsset;
                if (newSceneAsset != null)
                {
                    scenePathProp.stringValue = newSceneAsset.name;
                }
                else
                {
                    scenePathProp.stringValue = "";
                }

                EditorUtility.SetDirty(target);
            }

// Debug.Log(scenePathProp);
// Debug.Log(sceneAssetProp);
            // Show scene path for info
            EditorGUI.LabelField(new Rect(rect.x + 10, y, rect.width - 10, lineHeight), "Scene Name: " + scenePathProp.stringValue);
            y += lineHeight + spacing;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty requiredScenarioProp = serializedObject.FindProperty("requiredScenario");
            EditorGUILayout.PropertyField(requiredScenarioProp, new GUIContent("Required Scenario"));
            EditorGUILayout.Space();

            list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPrefabSection(int index, ref float y, Rect rect, string label, SerializedProperty containerProp, Type targetType, Dictionary<int, bool> foldoutDict, string prefabFieldName,
            string assetRefFieldName, bool isAdditional = false)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            if (!foldoutDict.ContainsKey(index)) foldoutDict[index] = true;

            foldoutDict[index] = EditorGUI.Foldout(
                new Rect(rect.x + 10, y, rect.width - 10, lineHeight),
                foldoutDict[index],
                label + " Prefab",
                true
            );
            y += lineHeight + spacing;

            if (!foldoutDict[index]) return;

            string prefix = isAdditional ? "Additional" : (label == "End Player" ? "End" : "");

            var presenterType = containerProp.FindPropertyRelative($"presenterType{prefix}");
            var viewType = containerProp.FindPropertyRelative($"viewType{prefix}");
            var locationSuffix = containerProp.FindPropertyRelative($"locationSuffix{prefix}");
            var prefabObj = containerProp.FindPropertyRelative(prefabFieldName);
            var assetRefProp = containerProp.FindPropertyRelative(assetRefFieldName);

            EditorGUI.LabelField(new Rect(rect.x + 10, y, rect.width - 10, lineHeight), "Presenter: " + presenterType.stringValue);
            y += lineHeight + spacing;
            EditorGUI.LabelField(new Rect(rect.x + 10, y, rect.width - 10, lineHeight), "View: " + viewType.stringValue);
            y += lineHeight + spacing;
            EditorGUI.LabelField(new Rect(rect.x + 10, y, rect.width - 10, lineHeight), "Location Suffix: " + locationSuffix.stringValue);
            y += lineHeight + spacing;

            var newObj = EditorGUI.ObjectField(
                new Rect(rect.x + 10, y, rect.width - 10, lineHeight),
                "Prefab Example",
                prefabObj.objectReferenceValue,
                targetType,
                false
            ) as MonoBehaviour;
            y += lineHeight + spacing;

            if (newObj != prefabObj.objectReferenceValue)
            {
                prefabObj.objectReferenceValue = newObj;

                if (newObj != null)
                {
                    var type = newObj.GetType();
                    if (GenericTypeHelper.TryExtractGenericArgumentsFromBase(type, typeof(SimplePresenterView<,>), out var types))
                    {
                        presenterType.stringValue = types[0].FullName;
                        viewType.stringValue = types[1].FullName;
                        locationSuffix.stringValue = newObj.name;
                    }
                    else
                    {
                        presenterType.stringValue = "(not valid)";
                        viewType.stringValue = "";
                        locationSuffix.stringValue = newObj.name;
                    }

                    prefabObj.objectReferenceValue = null;
                    EditorUtility.SetDirty(target);
                }
            }

            // Показати AssetReference
            EditorGUI.PropertyField(
                new Rect(rect.x + 10, y, rect.width - 10, lineHeight),
                assetRefProp,
                new GUIContent("Asset Reference")
            );
            y += lineHeight + spacing;

            // Якщо це Additional — поле для numberCopies
            if (isAdditional)
            {
                var numberCopies = containerProp.FindPropertyRelative("numberCopiesAdditionalPresenterT");
                numberCopies.intValue = EditorGUI.IntField(
                    new Rect(rect.x + 10, y, rect.width - 10, lineHeight),
                    "Number of Copies",
                    numberCopies.intValue
                );
                y += lineHeight + spacing;
            }
        }        
        

    }
}