using _Project.Scripts.Editor.Attributes;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer :  PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.conditionField);
            if (conditionProp != null && conditionProp.propertyType == SerializedPropertyType.Boolean)
            {
                if (conditionProp.boolValue)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.conditionField);

            if (conditionProp != null && conditionProp.propertyType == SerializedPropertyType.Boolean)
            {
                return conditionProp.boolValue ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
            }

            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}