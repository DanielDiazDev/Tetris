using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime.Nodes;
using _Project.Scripts.UINodeGraph.Runtime.Transitions;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Runtime
{
    public class UINavigationManager : MonoBehaviour
    {
        public static UINavigationManager Instance;
        [Header("Asset & Config")]
        public UIFlowAsset flowAsset;
        [SerializeField] VoidEvent OnUIClosed;
        
        private Stack<UIPanel> history = new Stack<UIPanel>();
        private Dictionary<string, UIPanel> instances = new Dictionary<string, UIPanel>();
        public bool IsTransitioning => _isTransitioning;
        private bool _isTransitioning;
        private Coroutine _autoTransitionCoroutine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (flowAsset != null && !string.IsNullOrEmpty(flowAsset.initialNodeGuid))
            {
                StartCoroutine(DelayedStart());
            }
        }

        private IEnumerator DelayedStart() {
            yield return null;
            NavigateTo(flowAsset.initialNodeGuid);
            yield return new WaitForSeconds(0.1f);
        }

        public void NavigateTo(string targetGuid)
        {
            if (_isTransitioning || string.IsNullOrEmpty(targetGuid)) return;
            CancelAutoTransition();
            var node = flowAsset.nodes.FirstOrDefault(n => n.guid == targetGuid);
            if (node is PanelNodeData panelData)
            {
                StartCoroutine(TransitionRoutine(panelData));
            }
            else
            {
                node?.Execute(this);
            }
        }
       
        public void ClosePanel(UIPanel panel, float fadeDuration = 0.25f)
        {
            if (panel == null) return;

            if (history.Count > 0 && history.Peek() == panel)
            {
                history.Pop();
            }
            else if (history.Contains(panel))
            {
                var list = history.ToList();
                list.Remove(panel);
                list.Reverse();
                history = new Stack<UIPanel>(list);
            }

            panel.Hide(fadeDuration);
        }

        private IEnumerator TransitionRoutine(PanelNodeData data)
        {
            _isTransitioning = true;
            UIPanel next = GetOrSpawn(data);
            UIPanel from = history.Count > 0 ? history.Peek() : null;
    
            if (from != next) 
                history.Push(next);
            yield return new FadeTransition().DoTransition(from?.GetComponent<CanvasGroup>(), next.GetComponent<CanvasGroup>(), 0.25f);
            _isTransitioning = false;
            if (data.useAutoTransition) {
                var autoEdge = flowAsset.edges.FirstOrDefault(e => e.baseNodeGuid == data.guid && e.portName == "AutoNext");
                if (autoEdge != null) {
                    CancelAutoTransition();
                    _autoTransitionCoroutine = StartCoroutine(AutoNavigateRoutine(autoEdge.targetNodeGuid, data.autoDelay));
                }
            }
        }
        public void ExecuteSequence(string sequenceNodeGuid) 
        {
            var edges = flowAsset.edges
                .Where(e => e.baseNodeGuid == sequenceNodeGuid)
                .OrderBy(e => e.portName)
                .ToList();

            foreach (var edge in edges) 
            {
                ExecuteNodeByGuid(edge.targetNodeGuid);
            }
        }
        
        private void ExecuteNodeByGuid(string guid) 
        {
            var node = flowAsset.nodes.FirstOrDefault(n => n.guid == guid);
            if (node is PanelNodeData) NavigateTo(guid);
            else node?.Execute(this);
        }

        public void DoBack(bool forceCloseLast = false)
        {
            if (_isTransitioning || history.Count == 0) return;
            UIPanel currentPanel = history.Peek();
            PanelNodeData currentData = flowAsset.nodes.FirstOrDefault(n => n.guid == currentPanel.nodeGuid) as PanelNodeData;
            CancelAutoTransition();
            
            if (history.Count > 1) 
            {
                UIPanel current = history.Pop();
                UIPanel prev = history.Peek();
                StartCoroutine(BackTransitionRoutine(prev, current));
            }
            else
            {
                if (!forceCloseLast)
                {
                    var backEdge = flowAsset.edges.FirstOrDefault(e =>
                        e.baseNodeGuid == currentPanel.nodeGuid && e.portName == "Back");
                    if (backEdge != null)
                    {
                        ExecuteNodeByGuid(backEdge.targetNodeGuid);
                        return;
                    }
                }
                bool shouldClose = forceCloseLast || (currentData != null);
                if (shouldClose) StartCoroutine(ExitUIRoutine(history.Pop()));
            }
        }

        private IEnumerator ExitUIRoutine(UIPanel panel) 
        {
            _isTransitioning = true;

            yield return new FadeTransition().DoTransition(panel.GetComponent<CanvasGroup>(), null, 0.25f);
            OnUIClosed?.Raise();
            _isTransitioning = false;
        }
        public GameObject GetPanelByGuid(string guid)
        {
            if (instances.TryGetValue(guid, out var panel))
            {
                return panel.gameObject;
            }
            return null;
        }
        private IEnumerator BackTransitionRoutine(UIPanel target, UIPanel from) 
        {
            _isTransitioning = true;
            yield return new FadeTransition().DoTransition(from.GetComponent<CanvasGroup>(), target.GetComponent<CanvasGroup>(), 0.25f);
            _isTransitioning = false;
        }
        public int GetCurrentHistoryCount() => history.Count;
        private IEnumerator AutoNavigateRoutine(string targetGuid, float delay) 
        {
            yield return new WaitForSecondsRealtime(delay);
            _autoTransitionCoroutine = null;
            NavigateTo(targetGuid);
        }

        private UIPanel GetOrSpawn(PanelNodeData data)
        {
            if (instances.TryGetValue(data.guid, out var p)) 
            {
                p.Show(0);
                return p;
            }
            GameObject obj = Instantiate(data.panelPrefab, transform);
            UIPanel panel = obj.GetComponent<UIPanel>() ?? obj.AddComponent<UIPanel>();
            panel.nodeGuid = data.guid;

            var edges = flowAsset.edges.Where(e => e.baseNodeGuid == data.guid);
            foreach (var edge in edges) {
                var btn = obj.GetComponentsInChildren<UnityEngine.UI.Button>(true).FirstOrDefault(b => b.name == edge.portName);
                if (btn) {
                    btn.onClick.AddListener(() => 
                    {
                        NavigateTo(edge.targetNodeGuid);
                    });
                }
            }
            instances.Add(data.guid, panel);
            return panel;
        }

        private void CancelAutoTransition() 
        {
            if (_autoTransitionCoroutine != null) 
            {
                StopCoroutine(_autoTransitionCoroutine);
                _autoTransitionCoroutine = null;
            }
        }
    }
}