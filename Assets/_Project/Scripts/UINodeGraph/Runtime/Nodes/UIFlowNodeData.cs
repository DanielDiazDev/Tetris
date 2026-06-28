using System;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Runtime.Nodes
{
    [Serializable]
    public abstract class UIFlowNodeData
    {
        public string guid;
        public Vector2 position;
        public abstract void Execute(UINavigationManager manager);
    }
}