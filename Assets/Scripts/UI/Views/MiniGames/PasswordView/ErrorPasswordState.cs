using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class ErrorPasswordState : PasswordState
    {
        private readonly Action _onErrorPass;

        private bool _isInputEnd;
        
        public ErrorPasswordState(Color selectedColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers, 
            Action onErrorPass)
            : base(selectedColor, stateMachine, lineRenderer, nodeContainers)
        {
            _onErrorPass = onErrorPass;
        }

        public override void Enter(LinkedList<int> selectedNodes)
        {
            base.Enter(selectedNodes);
            _isInputEnd = false;
            _onErrorPass();
        }
        
        public override bool CheckNode(NodeContainer node, int index, Vector2 touchPosition)
        {
            if (BitmaskHelper.CheckContainsInBitmask(SelectedNodesMask, index))
                return false;

            if (!RectTransformUtility.RectangleContainsScreenPoint(node.Circle, touchPosition) || _isInputEnd)
                return false;

            AddSelected(index);
            node.SetColor(SelectedColor);
            return true;
        }

        public override void OnEndInput() 
            => _isInputEnd = true;
    }
}