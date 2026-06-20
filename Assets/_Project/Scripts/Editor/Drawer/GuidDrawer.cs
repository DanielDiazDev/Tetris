using System;
using _Project.Scripts.Editor.Attributes;
using _Project.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(GuidAttribute))]
    public class GuidDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Obtenemos las sub-properties (Part1..Part4)
            SerializedProperty part1 = property.FindPropertyRelative("Part1");
            SerializedProperty part2 = property.FindPropertyRelative("Part2");
            SerializedProperty part3 = property.FindPropertyRelative("Part3");
            SerializedProperty part4 = property.FindPropertyRelative("Part4");

            SerializableGuid currentGuid = new SerializableGuid(
                (uint)part1.intValue,
                (uint)part2.intValue,
                (uint)part3.intValue,
                (uint)part4.intValue
            );

            string hexString = currentGuid.ToHexString();

            Rect textRect = new Rect(position.x, position.y, position.width - 80, position.height);
            Rect buttonRect = new Rect(position.x + position.width - 75, position.y, 75, position.height);

            EditorGUI.LabelField(textRect, label, new GUIContent(hexString));

            if (GUI.Button(buttonRect, "New GUID"))
            {
                SerializableGuid newGuid = SerializableGuid.NewGuid();
                part1.intValue = (int)newGuid.Part1;
                part2.intValue = (int)newGuid.Part2;
                part3.intValue = (int)newGuid.Part3;
                part4.intValue = (int)newGuid.Part4;

                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}