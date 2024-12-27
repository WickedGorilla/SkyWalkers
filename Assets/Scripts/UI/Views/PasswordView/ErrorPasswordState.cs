using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class ErrorPasswordState : PasswordState
    {
        public ErrorPasswordState(Color selectColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers) 
            : base(selectColor, stateMachine, lineRenderer, nodeContainers)
        {
        }

        public override void Enter(LinkedList<int> selectedNodes)
        {
            base.Enter(selectedNodes);
            LineRenderer.UpdateColor(SelectColor);
        }

        public override bool CheckNode(NodeContainer node, int index, Vector2 touchPosition)
        {
            if (BitmaskHelper.CheckContainsInBitmask(SelectedNodesMask, index))
                return false;
            
            if (!RectTransformUtility.RectangleContainsScreenPoint(node.Circle, touchPosition)) 
                return false;
            
            SelectedNodes.AddLast(index);
            node.SetColor(SelectColor);
            return true;
        }
    }
}