using System;
using System.Linq;
using System.Text;
using _Project.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidDrawer : PropertyDrawer {
        static readonly string[] GuidParts = { "Part1", "Part2", "Part3", "Part4" };

        static SerializedProperty[] GetGuidParts(SerializedProperty property) {
            var values = new SerializedProperty[GuidParts.Length];
            for (int i = 0; i < GuidParts.Length; i++) {
                values[i] = property.FindPropertyRelative(GuidParts[i]);
            }
            return values;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            var guidParts = GetGuidParts(property);
            string guidString = BuildGuidString(guidParts);

            // Dividimos el rectángulo en dos: izquierda para el GUID, derecha para los botones
            Rect guidRect = new Rect(position.x, position.y, position.width - 160, position.height);
            Rect buttonRect = new Rect(position.x + position.width - 160, position.y, 160, position.height);

            EditorGUI.SelectableLabel(guidRect, guidString);

            // Dibujar botones en línea
            float buttonWidth = 50;
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y, buttonWidth, buttonRect.height), "Copy")) {
                CopyGuid(property);
            }
            if (GUI.Button(new Rect(buttonRect.x + buttonWidth + 5, buttonRect.y, buttonWidth, buttonRect.height), "Reset")) {
                ResetGuid(property);
            }
            if (GUI.Button(new Rect(buttonRect.x + 2*(buttonWidth + 5), buttonRect.y, buttonWidth, buttonRect.height), "New")) {
                RegenerateGuid(property);
            }

            EditorGUI.EndProperty();
        }

        void CopyGuid(SerializedProperty property) {
            string guid = BuildGuidString(GetGuidParts(property));
            EditorGUIUtility.systemCopyBuffer = guid;
            Debug.Log($"GUID copied to clipboard: {guid}");
        }

        void ResetGuid(SerializedProperty property) {
            foreach (var part in GetGuidParts(property)) {
                part.uintValue = 0;
            }
            property.serializedObject.ApplyModifiedProperties();
            Debug.Log("GUID has been reset.");
        }

        void RegenerateGuid(SerializedProperty property) {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            SerializedProperty[] guidParts = GetGuidParts(property);

            for (int i = 0; i < GuidParts.Length; i++) {
                guidParts[i].uintValue = BitConverter.ToUInt32(bytes, i * 4);
            }

            property.serializedObject.ApplyModifiedProperties();
            Debug.Log("GUID has been regenerated.");
        }

        static string BuildGuidString(SerializedProperty[] guidParts) {
            return new StringBuilder()
                .AppendFormat("{0:X8}", guidParts[0].uintValue)
                .AppendFormat("{0:X8}", guidParts[1].uintValue)
                .AppendFormat("{0:X8}", guidParts[2].uintValue)
                .AppendFormat("{0:X8}", guidParts[3].uintValue)
                .ToString();
        }
    }
}
