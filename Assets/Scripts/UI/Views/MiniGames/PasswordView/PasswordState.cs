using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public abstract class PasswordState
    {
        private readonly NodeContainer[] _nodeContainers;
        
        private int _prevCountSelectedNodes;
        
        protected PasswordState(Color selectedColor, IPasswordStateMachine stateMachine, 
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers)
        {
            LineRenderer = lineRenderer;
            _nodeContainers = nodeContainers;
            SelectedColor = selectedColor;
            StateMachine = stateMachine;
        }

        public LinkedList<int> SelectedNodes { get; private set; }

        protected Color SelectedColor { get; }
        protected IPasswordStateMachine StateMachine { get; }
        protected UILineRenderer LineRenderer { get; }
        protected int SelectedNodesMask { get; private set; }
        
        public virtual void Enter(LinkedList<int> selectedNodes)
        {
            SelectedNodes = selectedNodes;
            UpdateColor(selectedNodes, SelectedColor);
        }
        
        public abstract bool CheckNode(NodeContainer node, int selectedIndex, Vector2 touchPosition);
        
        
        protected virtual void Reset()
        {
            _prevCountSelectedNodes = default;
        }
        
        public void UpdateRender()
        {
            if (_prevCountSelectedNodes == SelectedNodes.Count)
                return;
            
            LineRenderer.SetPoints(GetPointsByIndex(SelectedNodes));
            _prevCountSelectedNodes = SelectedNodes.Count;
        }

        protected void AddSelected(int index)
        {
            SelectedNodes.AddLast(index);
            SelectedNodesMask = BitmaskHelper.GenerateBitmask(SelectedNodes);
        }
        
        private IEnumerable<Vector2> GetPointsByIndex(IEnumerable<int> passIndexes)
        {
            foreach (var index in passIndexes)
                yield return _nodeContainers[index].Position;
        }
        
        protected void UpdateColor(LinkedList<int> selectedNodes, Color color)
        {
            foreach (var index in selectedNodes)
              _nodeContainers[index].SetColor(color);
        }
    }
}