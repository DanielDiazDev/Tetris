using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    public class PrefabScreenshot : EditorWindow 
    {
        private Object prefab;
    
        [MenuItem("Tools/Prefab Screenshot")]
        public static void ShowWindow() 
        {
            GetWindow<PrefabScreenshot>("Prefab Screenshot");
        }
    
        void OnGUI() 
        {
            prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(Object), false);
            if (GUILayout.Button("Export PNG") && prefab != null) 
            {
                Texture2D tex = AssetPreview.GetAssetPreview(prefab);
                if (tex != null) 
                {
                    byte[] bytes = tex.EncodeToPNG();
                    File.WriteAllBytes("Assets/PrefabScreenshot.png", bytes);
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}