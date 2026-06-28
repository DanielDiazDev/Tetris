using _Project.Scripts.UINodeGraph.Runtime;
using _Project.Scripts.UINodeGraph.Runtime.Transitions;
using UnityEditor.Experimental.GraphView;

namespace _Project.Scripts.UINodeGraph.Editor.Edges
{
    public class UIFlowEdge : Edge
    {
        public UIFlowTransition transition = new FadeTransition();
    }
}