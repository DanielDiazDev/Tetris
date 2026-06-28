using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UINodeGraph.Editor.Edges;
using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor.Views
{
    public class UIFlowGraphView : GraphView
    {
        public Action<ISelectable> OnElementSelected;
        public UIFlowEditorWindow window;
        public UIFlowGraphView(UIFlowEditorWindow window)
        {
            this.window = window;
            serializeGraphElements = OnSerializeElements;
            unserializeAndPaste = OnUnserializeAndPaste;
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());
            this.graphViewChanged = OnGraphChanged;
            this.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            schedule.Execute(() => {
                if (selection.Count > 0)
                {
                    OnElementSelected?.Invoke(selection[0]);
                }
                else
                {
                    OnElementSelected?.Invoke(null);
                }
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                for (int i = 0; i < change.edgesToCreate.Count; i++)
                {
                    var edge = change.edgesToCreate[i];
                    if (!(edge is UIFlowEdge))
                    {
                        var flowEdge = new UIFlowEdge
                        {
                            output = edge.output,
                            input = edge.input
                        };
                    
                        flowEdge.input.Connect(flowEdge);
                        flowEdge.output.Connect(flowEdge);
                        change.edgesToCreate[i] = flowEdge;
                    }
                }
            }
            return change;
        }

        private void OnUnserializeAndPaste(string operationName, string data)
        {
            var wrapper = JsonUtility.FromJson<NodeCopyWrapper>(data);
            foreach (var nodeData in wrapper.nodes)
            {
                nodeData.guid = System.Guid.NewGuid().ToString();
                nodeData.position += new Vector2(40, 40);
                string viewName = nodeData.GetType().Name.Replace("Data", "View");
                System.Type viewType = System.AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == viewName);

                if (viewType != null)
                {
                    BaseNodeView view = (BaseNodeView)System.Activator.CreateInstance(viewType);
                    view.Initialize(nodeData);
                    AddElement(view);
                }
            }
        }

        private string OnSerializeElements(IEnumerable<GraphElement> elements)
        {
            var nodesToCopy = elements.OfType<BaseNodeView>().Select(n => n.GetData()).ToList();
            return JsonUtility.ToJson(new NodeCopyWrapper { nodes = nodesToCopy });
        }
    }
    [System.Serializable]
    public class NodeCopyWrapper {
        [SerializeReference] public List<UIFlowNodeData> nodes;
    }
}