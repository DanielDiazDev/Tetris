using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UINodeGraph.Runtime.Nodes
{
    [Serializable]
    public class SceneNodeData : UIFlowNodeData
    {
        public string sceneName;

        public override void Execute(UINavigationManager manager)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("SceneNode: No se ha especificado el nombre de la escena.");
            }
        }
    }
}