using UnityEditor;
using UnityEngine;
using System;
using Data;
using Settings;
using UIElements.Pages;

[CustomEditor(typeof(UIScreenData))]
public class UIScreenDataEditor : UnityEditor.Editor
{
    private bool[] _foldouts;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        UIScreenData screenData = (UIScreenData)target;

        if (screenData.PagePrefabData == null)
            screenData.PagePrefabData = new PagePrefabData[0];

        if (_foldouts == null || _foldouts.Length != screenData.PagePrefabData.Length)
            _foldouts = new bool[screenData.PagePrefabData.Length];

        for (int i = 0; i < screenData.PagePrefabData.Length; i++)
        {
            var item = screenData.PagePrefabData[i];
            _foldouts[i] = EditorGUILayout.Foldout(_foldouts[i], $"Page {i + 1}", true);

            if (_foldouts[i])
            {
                EditorGUI.indentLevel++;

                // 1. Ручне поле — pageState
                item.pageState = (Enumerators.AppState)EditorGUILayout.EnumPopup("Page State", item.pageState);

                // 2. Автоматично згенеровані поля
                EditorGUILayout.LabelField("Presenter Type", item.presenterType);
                EditorGUILayout.LabelField("Location Suffix", item.locationSuffix);

                // 3. ВНИЗУ — вибір префабу
                var newPagePrefab = (PagePresentersView)EditorGUILayout.ObjectField("Page Prefab", null, typeof(PagePresentersView), false);

                if (newPagePrefab != null)
                {
                    // автоматичне заповнення типу й імені
                    Type type = newPagePrefab.GetType();
                    item.presenterType = type.FullName;
                    item.locationSuffix = newPagePrefab.gameObject.name;

                    // обнуляємо pagePrefab після витягування
                    item.pagePrefab = null;

                    EditorUtility.SetDirty(screenData);
                }

                // 4. Видалення
                if (GUILayout.Button("Remove Page"))
                {
                    var list = new System.Collections.Generic.List<PagePrefabData>(screenData.PagePrefabData);
                    list.RemoveAt(i);
                    screenData.PagePrefabData = list.ToArray();
                    _foldouts = new bool[screenData.PagePrefabData.Length];
                    EditorUtility.SetDirty(screenData);
                    return;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        if (GUILayout.Button("Add Page"))
        {
            var list = new System.Collections.Generic.List<PagePrefabData>(screenData.PagePrefabData ?? new PagePrefabData[0]);
            list.Add(new PagePrefabData());
            screenData.PagePrefabData = list.ToArray();
            _foldouts = new bool[screenData.PagePrefabData.Length];
            EditorUtility.SetDirty(screenData);
        }

        serializedObject.ApplyModifiedProperties();
    }
}