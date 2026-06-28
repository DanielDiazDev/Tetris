using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor.Views
{
    public class SceneNodeView : BaseNodeView
    {
        public string sceneName;

        public override void Initialize(UIFlowNodeData data)
        {
            base.Initialize(data);
            title = "CHANGE SCENE";
            
            titleContainer.style.backgroundColor = new Color(0.1f, 0.4f, 0.1f);

            var sData = data as SceneNodeData;
            sceneName = sData?.sceneName;

            SceneAsset sceneAsset = null;
            if (!string.IsNullOrEmpty(sceneName))
            {
                string[] guids = AssetDatabase.FindAssets(sceneName + " t:SceneAsset");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                }
            }

            var sceneField = new ObjectField("Scene Asset") 
            { 
                objectType = typeof(SceneAsset), 
                value = sceneAsset 
            };

            sceneField.RegisterValueChangedCallback(evt => {
                var newScene = evt.newValue as SceneAsset;
                sceneName = newScene != null ? newScene.name : "";
            });
            extensionContainer.Add(sceneField);
        }

        public override UIFlowNodeData GetData()
        {
            return new SceneNodeData
            {
                guid = GUID,
                position = GetPosition().position,
                sceneName = sceneName
            };
        }
    }
}