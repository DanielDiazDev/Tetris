using _Project.Scripts.UINodeGraph.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Editor
{
    [CustomEditor(typeof(UIFlowAsset))]
    public class UIFlowAssetInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UIFlowAsset asset = (UIFlowAsset)target;
            base.OnInspectorGUI();
            
            GUILayout.Space(10);
            GUI.backgroundColor = new Color(0.2f, 0.6f, 1f); 
            if (GUILayout.Button("OPEN UI FLOW EDITOR", GUILayout.Height(40)))
            {
                UIFlowEditorWindow.OpenWithAsset(asset);
            }
            
            GUI.backgroundColor = Color.white;
            GUILayout.Space(10);
        }
    }
}