using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor.Views
{
    public class BackNodeView : BaseNodeView
    {
        public bool closeIfLast;
        public override void Initialize(UIFlowNodeData data)
        {
            base.Initialize(data);
            var panelData = data as BackNodeData;
            closeIfLast = panelData?.closeIfLast ?? false;
            title = "BACK ACTION";
            AddToClassList("ui-node");
            AddToClassList("back-node");
            titleContainer.style.backgroundColor = new Color(0.6f, 0.1f, 0.1f);
            inputPort.portName = "Trigger";
            var toggle = new Toggle("Close If Last") { value = closeIfLast };
            toggle.RegisterValueChangedCallback(evt => closeIfLast = evt.newValue);
            extensionContainer.Add(toggle);
        }

        public override UIFlowNodeData GetData() => new BackNodeData
        {
            guid = GUID,
            position = GetPosition().position,
            closeIfLast = closeIfLast
        };
    
    }
}