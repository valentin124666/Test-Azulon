using Items;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty id = serializedObject.FindProperty("_id");
            SerializedProperty name = serializedObject.FindProperty("_name");
            SerializedProperty description = serializedObject.FindProperty("_description");
            SerializedProperty icon = serializedObject.FindProperty("_icon");
            SerializedProperty interactionType = serializedObject.FindProperty("_interactionType");

            SerializedProperty consumableProps = serializedObject.FindProperty("_consumableProperties");
            SerializedProperty equippableProps = serializedObject.FindProperty("_equippableProperty");
            SerializedProperty itemEffects = serializedObject.FindProperty("_itemEffects");
            SerializedProperty equipSlotProperty = serializedObject.FindProperty("_equipSlotProperty");

            EditorGUILayout.PropertyField(id);
            EditorGUILayout.PropertyField(name);
            EditorGUILayout.PropertyField(description);
            EditorGUILayout.PropertyField(icon);
            EditorGUILayout.PropertyField(interactionType);

            ItemInteractionType type = (ItemInteractionType)interactionType.enumValueIndex;

            EditorGUILayout.Space(10);

            if (type == ItemInteractionType.Consume)
            {
                EditorGUILayout.PropertyField(consumableProps, true);
            }
            else if (type == ItemInteractionType.Equip)
            {
                // EditorGUILayout.LabelField("Equippable Properties", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(equipSlotProperty);
                EditorGUILayout.PropertyField(equippableProps, true);
            }
            
            EditorGUILayout.PropertyField(itemEffects);

            serializedObject.ApplyModifiedProperties();
        }
    }
}