namespace _Project.Scripts.UINodeGraph.Runtime.Nodes
{
    [System.Serializable]
    public class BackNodeData : UIFlowNodeData
    {
        public bool closeIfLast = false;
        
        public override void Execute(UINavigationManager manager) 
        {
            manager.DoBack(closeIfLast);
        }
    }
}