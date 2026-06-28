using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UINodeGraph.Editor.Views;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor
{
    public class UIFlowSearchWindow : ScriptableObject, ISearchWindowProvider 
    {
        private UIFlowGraphView _graphView;

        public void Init(UIFlowGraphView gv) => _graphView = gv;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) 
        {
            var tree = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("Nodes"), 0) };
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p => typeof(BaseNodeView).IsAssignableFrom(p) && !p.IsAbstract);

            foreach (var t in types) tree.Add(new SearchTreeEntry(new GUIContent(t.Name.Replace("NodeView",""))) { userData = t, level = 1 });
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context) 
        {
            var pos = _graphView.contentViewContainer.WorldToLocal(context.screenMousePosition - _graphView.window.position.position);
            var node = (BaseNodeView)Activator.CreateInstance((Type)entry.userData);
            node.Initialize(node.GetData());
            node.GUID = Guid.NewGuid().ToString();
            node.SetPosition(new Rect(pos, new Vector2(200,150)));
            _graphView.AddElement(node);
            return true;
        }
    }
}