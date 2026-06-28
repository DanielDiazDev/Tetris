using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UINodeGraph.Editor.Views
{
    public class PanelNodeView : BaseNodeView
    {
        public GameObject prefab;
        public bool isInitial;
        private Toggle _backToggle;
        private Toggle _autoToggle;
        private FloatField _delayField;
        private VisualElement _btnContainer;
        private Button _initialBtn;
        private Toggle _closeIfLastToggle;
        private IntegerField _playerIDField;

        public override void Initialize(UIFlowNodeData data)
        {
            base.Initialize(data);
            var panelData = data as PanelNodeData;
            prefab = panelData?.panelPrefab;
            title = prefab != null ? prefab.name.ToUpper() : "PANEL NODE";
            AddToClassList("ui-node");
            AddToClassList("panel-node");
            _autoToggle = new Toggle("Auto Transition") { value = panelData.useAutoTransition };
            _autoToggle.RegisterValueChangedCallback(evt =>
            {
                UpdateFieldVisibility();
                RefreshButtons();
            });
            extensionContainer.Add(_autoToggle);
            _backToggle = new Toggle("Override Back Input") { value = panelData.useOverrideBack };
            _backToggle.RegisterValueChangedCallback(evt => RefreshButtons());
            extensionContainer.Insert(0, _backToggle);

            _closeIfLastToggle = new Toggle("Close If Last (Physical Back)")
                { value = panelData.closeIfLastOnBackInput };
            extensionContainer.Add(_closeIfLastToggle);

            _delayField = new FloatField("Delay (s)") { value = panelData.autoDelay };
            extensionContainer.Add(_delayField);

            _initialBtn = new UnityEngine.UIElements.Button(OnSetInitialClicked) { text = "Set Initial" };
            titleContainer.Add(_initialBtn);
            var objField = new ObjectField("Panel Prefab") { objectType = typeof(GameObject), value = prefab };
            objField.RegisterValueChangedCallback(evt =>
            {
                prefab = (GameObject)evt.newValue;
                title = prefab != null ? prefab.name.ToUpper() : "PANEL NODE";
                RefreshButtons();
            });
            extensionContainer.Add(objField);

            _btnContainer = new VisualElement();
            _btnContainer.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
            _btnContainer.style.marginTop = 5;
            _btnContainer.style.paddingBottom = 5;
            _btnContainer.style.paddingTop = 5;
            _btnContainer.style.paddingRight = 5;
            _btnContainer.style.paddingLeft = 5;
            extensionContainer.Add(_btnContainer);
            UpdateFieldVisibility();
            RefreshButtons();
        }

        private void OnSetInitialClicked()
        {
            var window = UnityEditor.EditorWindow.GetWindow<UIFlowEditorWindow>();
            window.SetInitial(this);
        }

        private void RefreshButtons()
        {
            int currentInsertIndex = 0;
            var backPort = outputContainer.Children().OfType<Port>().FirstOrDefault(p => p.portName == "Back");
            if (_backToggle.value)
            {
                if (backPort == null)
                {
                    backPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(bool));
                    backPort.portName = "Back";
                    backPort.portColor = new Color(1f, 0.3f, 0.3f);
                    backPort.contentContainer.Add(new Label("BACK EXIT")
                    {
                        style = { color = new Color(1f, 0.6f, 0.6f), unityFontStyleAndWeight = FontStyle.Bold }
                    });
                    outputContainer.Insert(currentInsertIndex, backPort);
                }
                else if (outputContainer.IndexOf(backPort) != currentInsertIndex)
                {
                    outputContainer.Insert(currentInsertIndex, backPort);
                }

                currentInsertIndex++;
            }
            else if (backPort != null)
            {
                RemovePortWithEdges(backPort);
            }

            _btnContainer.Clear();

            if (prefab == null)
            {
                _btnContainer.Add(new Label("Assign a prefab to see buttons"));
                var toRemove = outputContainer.Children().OfType<Port>()
                    .Where(p => p.portName != "AutoNext" && p.portName != "Back" && p.portName != "In").ToList();
                foreach (var p in toRemove)
                {
                    outputContainer.Remove(p);
                }
            }
            else
            {
                var buttons = prefab.GetComponentsInChildren<UnityEngine.UI.Button>(true);
                var uniqueBtnNames = new HashSet<string>(buttons.Select(b => b.gameObject.name));
                var existingPorts = outputContainer.Children().OfType<Port>()
                    .Where(p => p.portName != "AutoNext" && p.portName != "Back" && p.portName != "In").ToList();

                foreach (var p in existingPorts)
                {
                    if (!uniqueBtnNames.Contains(p.portName)) outputContainer.Remove(p);
                }

                foreach (var btnName in uniqueBtnNames)
                {
                    bool alreadyExists = outputContainer.Children().OfType<Port>().Any(p => p.portName == btnName);
                    if (!alreadyExists)
                    {
                        var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single,
                            typeof(bool));
                        port.portName = btnName;
                        var label = port.Query<Label>().First();
                        if (label != null)
                        {
                            label.style.fontSize = 11;
                            label.style.color = new Color(0.8f, 0.8f, 0.8f);
                            label.style.marginLeft = 4;
                        }

                        outputContainer.Add(port);
                    }
                }
            }

            RefreshExpandedState();
            RefreshPorts();
        }

        private void RemovePortWithEdges(Port port)
        {
            var edges = port.connections.ToList();
            foreach (var edge in edges)
            {
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                edge.RemoveFromHierarchy();
            }

            outputContainer.Remove(port);
        }

        private void UpdateFieldVisibility()
        {
            if (_delayField == null || _autoToggle == null) return;
            _delayField.style.display = _autoToggle.value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public override UIFlowNodeData GetData()
        {
            return new PanelNodeData()
            {
                guid = GUID,
                position = GetPosition().position,
                panelPrefab = prefab,
                useAutoTransition = _autoToggle?.value ?? false,
                autoDelay = _delayField?.value ?? 2.0f,
                useOverrideBack = _backToggle?.value ?? false,
                closeIfLastOnBackInput = _closeIfLastToggle?.value ?? false,
            };
        }


        public void SetInitial(bool initial)
        {
            isInitial = initial;


            if (initial)
            {
                AddToClassList("is-initial");
            }
            else
            {
                RemoveFromClassList("is-initial");
            }


            var titleLabel = titleContainer.Q<Label>("title-label");
            var existingBadge = titleContainer.Q("initial-badge");

            if (initial)
            {
                if (existingBadge == null)
                {
                    var badge = new Label("START NODE");
                    badge.name = "initial-badge";
                    badge.AddToClassList("initial-badge-label");
                    titleContainer.Insert(0, badge);
                }

                if (_initialBtn != null) _initialBtn.style.display = DisplayStyle.None;
            }
            else
            {
                if (existingBadge != null) existingBadge.RemoveFromHierarchy();
                if (_initialBtn != null) _initialBtn.style.display = DisplayStyle.Flex;
            }
        }
    }
}