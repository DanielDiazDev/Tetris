using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor.Views
{
    public abstract class BaseNodeView : Node
    {
        public string GUID;
        public Port inputPort;

        public virtual void Initialize(UIFlowNodeData data)
        {
            GUID = string.IsNullOrEmpty(data.guid) ? System.Guid.NewGuid().ToString() : data.guid;
            SetPosition(new Rect(data.position, new Vector2(200, 150)));
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "In";
            inputContainer.Add(inputPort);
            AddToClassList("ui-node");
            var copyGuidBtn = new Button(() =>
            {
                GUIUtility.systemCopyBuffer = GUID; Debug.Log("GUID Copied!");
            })
            {
                text = "Copy GUID"
            };
            titleContainer.Add(copyGuidBtn);
        }
        public abstract UIFlowNodeData GetData();
    }
}