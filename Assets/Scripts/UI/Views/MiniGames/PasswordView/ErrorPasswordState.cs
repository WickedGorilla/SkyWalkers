using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class ErrorPasswordState : PasswordState
    {
        private readonly Action _onOnErrorPass;

        public ErrorPasswordState(Color selectColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers, Action onOnErrorPass) 
            : base(selectColor, stateMachine, lineRenderer, nodeContainers)
        {
            _onOnErrorPass = onOnErrorPass;
        }

        public override void Enter(LinkedList<int> selectedNodes)
        {
            base.Enter(selectedNodes);
            LineRenderer.UpdateColor(SelectColor);
            _onOnErrorPass();
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