using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using _Project.Scripts.UINodeGraph.Runtime.Transitions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UINodeGraph.Runtime
{
    [CreateAssetMenu(fileName = "UIFlowAsset", menuName = "ScriptableObjects/UIFlowAsset", order = 0)]
    public class UIFlowAsset : ScriptableObject
    {
        public string initialNodeGuid;
        [SerializeReference] 
        public List<UIFlowNodeData> nodes = new List<UIFlowNodeData>();
    
        public List<UIFlowEdgeData> edges = new List<UIFlowEdgeData>();
    }
    
    [Serializable]
    public class UIFlowEdgeData {
        public string baseNodeGuid;
        public string portName;
        public string targetNodeGuid;
        [SerializeReference] 
        public UIFlowTransition transition = new FadeTransition();
    }

    [Serializable]
    public abstract class UIFlowTransition {
        public float duration = 0.3f;
        public abstract IEnumerator DoTransition(CanvasGroup from, CanvasGroup to, float duration);
    }
}