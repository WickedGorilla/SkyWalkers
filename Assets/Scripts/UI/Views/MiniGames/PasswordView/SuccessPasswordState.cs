using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class SuccessPasswordState : PasswordState
    {
        private readonly Action _onSuccess;

        public SuccessPasswordState(Color selectColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers,
            Action onSuccess)
            : base(selectColor, stateMachine, lineRenderer, nodeContainers)
        {
            _onSuccess = onSuccess;
        }
        
        public override void Enter(LinkedList<int> selectedNodes)
        {
            base.Enter(selectedNodes);
            LineRenderer.UpdateColor(SelectColor);
            _onSuccess();
        }

        public override bool CheckNode(NodeContainer node, int index, Vector2 touchPosition) 
            => true;
    }
}