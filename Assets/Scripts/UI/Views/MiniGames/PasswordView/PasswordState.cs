using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public abstract class PasswordState
    {
        private readonly NodeContainer[] _nodeContainers;
        private int _prevCountSelectedNodes;
        
        protected PasswordState(Color selectColor, IPasswordStateMachine stateMachine, 
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers)
        {
            LineRenderer = lineRenderer;
            _nodeContainers = nodeContainers;
            SelectColor = selectColor;
            StateMachine = stateMachine;
        }

        public LinkedList<int> SelectedNodes { get; private set; }

        protected Color SelectColor { get; }
        protected IPasswordStateMachine StateMachine { get; }
        protected UILineRenderer LineRenderer { get; }
        protected int SelectedNodesMask { get; private set; }
        
        public virtual void Enter(LinkedList<int> selectedNodes)
        {
            SelectedNodes = selectedNodes;
            UpdateColor(SelectColor);
        }
        
        public abstract bool CheckNode(NodeContainer node, int index, Vector2 touchPosition);

        public void UpdateRender()
        {
            if (_prevCountSelectedNodes == SelectedNodes.Count)
                return;
            
            LineRenderer.SetPoints(GetPointsByIndex(SelectedNodes));
            _prevCountSelectedNodes = SelectedNodes.Count;
            SelectedNodesMask = BitmaskHelper.GenerateBitmask(SelectedNodes);
        }

        private IEnumerable<Vector2> GetPointsByIndex(IEnumerable<int> passIndexes)
        {
            foreach (var index in passIndexes)
                yield return _nodeContainers[index].Position;
        }
        
        private void UpdateColor(Color color)
        {
            foreach (var node in _nodeContainers)
                node.SetColor(color);
        }
    }
}