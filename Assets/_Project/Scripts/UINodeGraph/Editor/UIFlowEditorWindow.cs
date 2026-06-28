using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UINodeGraph.Editor.Edges;
using _Project.Scripts.UINodeGraph.Editor.Views;
using _Project.Scripts.UINodeGraph.Runtime;
using _Project.Scripts.UINodeGraph.Runtime.Transitions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor
{
    public class UIFlowEditorWindow : EditorWindow
    {
        private UIFlowGraphView _graphView;
        private UIFlowAsset _asset;
        private ObjectField _assetField;
        private VisualElement _edgeInspector; 
        [MenuItem("Tools/UI Flow Editor")]
        public static UIFlowEditorWindow Open() => 
            GetWindow<UIFlowEditorWindow>("UI Flow");
        public static void OpenWithAsset(UIFlowAsset asset)
        {
            var window = Open();
            window.LoadAssetFromInspector(asset);
        }

        private void OnEnable()
        {
            _graphView = new UIFlowGraphView(this) { style = { flexGrow = 1 } };
        
            // Cargar Estilos
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Project/Scripts/UINodeGraph/Editor/UIFlowStyle.uss");
            if (styleSheet != null) rootVisualElement.styleSheets.Add(styleSheet);
        
            rootVisualElement.Add(_graphView);
            _graphView.OnElementSelected = (element) => {
                if (element is UIFlowEdge edge) ShowEdgeInspector(edge);
                else _edgeInspector.style.display = DisplayStyle.None;
            };
            GenerateToolbar();
            
            var miniMap = new  MiniMap(){ anchored = true };
            miniMap.style.position = Position.Absolute;
            miniMap.style.left = 15;
            miniMap.style.bottom = 15; // O top si lo prefieres
            miniMap.style.width = 200;
            miniMap.style.height = 140;
            miniMap.style.borderTopWidth = 1;
            miniMap.style.borderTopColor = Color.gray;
            
            _graphView.Add(miniMap);
            
            // Search Window
            var searchWindow = ScriptableObject.CreateInstance<UIFlowSearchWindow>();
            searchWindow.Init(_graphView);
            _graphView.nodeCreationRequest = ctx => SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), searchWindow);
        
            // Inspector de Cables
            _edgeInspector = new VisualElement {
                style = {
                    position = Position.Absolute,
                    right = 20, bottom = 20,
                    width = 220, backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.95f), 
                    borderLeftColor = Color.gray, borderLeftWidth = 1, display = DisplayStyle.None,
                    paddingBottom = 10, borderBottomLeftRadius = 5,borderBottomRightRadius = 5, 
                    paddingLeft = 10, borderTopRightRadius = 5,borderTopLeftRadius = 5, paddingRight = 10, paddingTop = 10,
                }
            };
            rootVisualElement.Add(_edgeInspector);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            toolbar.Add(new Button(CreateNew) { text = " New Flow" });

            _assetField = new ObjectField("") { 
                objectType = typeof(UIFlowAsset), 
                style = { width = 250, marginLeft = 10 } 
            };
            _assetField.RegisterValueChangedCallback(evt => Load((UIFlowAsset)evt.newValue));
            toolbar.Add(_assetField);

            toolbar.Add(new ToolbarSpacer { style = { flexGrow = 1 } });

            var btnSave = new Button(Save) { text = " Save Graph" };
            toolbar.Add(btnSave);

            rootVisualElement.Insert(0, toolbar);
        }

        private void Save()
        {
            if (!_asset) return;
            Undo.RecordObject(_asset, "Save UI Flow");
            _asset.nodes.Clear(); 
            _asset.edges.Clear();

            var nodeViews = _graphView.nodes.Cast<BaseNodeView>().ToList();
            foreach (var view in nodeViews) {
                var data = view.GetData();
                _asset.nodes.Add(data);
                if (view is PanelNodeView p && p.isInitial) _asset.initialNodeGuid = p.GUID;
            }
        
            foreach (var edgeElement in _graphView.edges) {
                if (edgeElement is UIFlowEdge edge) {
                    var outputNode = edge.output.node as BaseNodeView;
                    var inputNode = edge.input.node as BaseNodeView;

                    _asset.edges.Add(new UIFlowEdgeData {
                        baseNodeGuid = outputNode.GUID,
                        portName = edge.output.portName,
                        targetNodeGuid = inputNode.GUID,
                        transition = edge.transition
                    });
                }
            }

            EditorUtility.SetDirty(_asset);
            AssetDatabase.SaveAssets();
            Debug.Log("UI Flow Saved!");
        }

        private void CreateNew()
        {
            string path = EditorUtility.SaveFilePanelInProject("New Flow", "UIFlow", "asset", "Save UI Flow");
            if (string.IsNullOrEmpty(path)) return;
            var asset = ScriptableObject.CreateInstance<UIFlowAsset>();
            AssetDatabase.CreateAsset(asset, path);
            _assetField.value = asset;
        }

        private void ShowEdgeInspector(UIFlowEdge edge)
        {
            _edgeInspector.Clear();
            _edgeInspector.style.display = DisplayStyle.Flex;
            _edgeInspector.Add(new Label("EDGE TRANSITION") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 5 } });

            var choices = new List<string> { "Fade", "Slide" };
            var dropdown = new PopupField<string>("Type", choices, edge.transition is FadeTransition ? "Fade" : "Slide");
    
            dropdown.RegisterValueChangedCallback(evt => {
                if (evt.newValue == "Fade") edge.transition = new FadeTransition();
                else edge.transition = new SlideTransition();
                ShowEdgeInspector(edge); 
            });
    
            _edgeInspector.Add(dropdown);

            var durationField = new FloatField("Duration") { value = edge.transition.duration };
            durationField.RegisterValueChangedCallback(evt => edge.transition.duration = evt.newValue);
            _edgeInspector.Add(durationField);
        }

        private void LoadAssetFromInspector(UIFlowAsset asset)
        {
            if (_assetField != null) 
            {
                _assetField.value = asset; 
            }
            else 
            {
                Load(asset);
                _asset = asset;
            }
        }

        private void Load(UIFlowAsset asset)
        {
            _asset = asset;
            _graphView.graphElements.ToList().ForEach(_graphView.RemoveElement);
            if(!asset) return;

            var nodeMap = new Dictionary<string, BaseNodeView>();

            foreach (var data in asset.nodes)
            {
                string viewName = data.GetType().Name.Replace("Data", "View");
                Type viewType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == viewName);
                if (viewType != null)
                {
                    BaseNodeView view = Activator.CreateInstance(viewType) as BaseNodeView;
                    view.Initialize(data);
                    _graphView.AddElement(view);
                    nodeMap.Add(data.guid, view);
                    if (data.guid == asset.initialNodeGuid && view is PanelNodeView p)
                    {
                        p.SetInitial(true);
                    }
                }
            }

            foreach (var e in asset.edges)
            {
                if (!nodeMap.ContainsKey(e.baseNodeGuid) || !nodeMap.ContainsKey(e.targetNodeGuid)) continue;

                var outNode = nodeMap[e.baseNodeGuid];
                var inNode = nodeMap[e.targetNodeGuid];
                
                var outPort = outNode.outputContainer.Children().OfType<Port>().FirstOrDefault(p => p.portName == e.portName);
                var inPort = inNode.inputContainer.Children().OfType<Port>().FirstOrDefault();

                if (outPort != null && inPort != null) 
                {
                    var edgeView = new UIFlowEdge 
                    {
                        output = outPort,
                        input = inPort,
                        transition = e.transition 
                    };

                    edgeView.input.Connect(edgeView);
                    edgeView.output.Connect(edgeView);
                    _graphView.AddElement(edgeView);
                }}
        }


        public void SetInitial(PanelNodeView target) 
        {
            foreach (var n in _graphView.nodes.OfType<PanelNodeView>())
            {
                n.SetInitial(n == target);
            }
        }
    }
}