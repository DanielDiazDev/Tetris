using System;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Runtime.Nodes
{
    [Serializable]
    public class PanelNodeData : UIFlowNodeData
    {
        public GameObject panelPrefab;
        public bool useAutoTransition;
        public bool useOverrideBack;
        public float autoDelay = 2.0f;
        //public string firstFocusButtonName; 
        // public UIControlMode controlMode = UIControlMode.MasterOnly;
        public bool closeIfLastOnBackInput;
        // public int assignedPlayerID = -1; 
        //public bool isNavigationRoot = true;
        public override void Execute(UINavigationManager manager)
        {
            manager.NavigateTo(guid);
        }
    }
}