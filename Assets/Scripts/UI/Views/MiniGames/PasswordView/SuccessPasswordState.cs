using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class SuccessPasswordState : PasswordState
    {
        private readonly Action _onSuccess;

        public SuccessPasswordState(Color selectedColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers,
            Action onSuccess)
            : base(selectedColor, stateMachine, lineRenderer, nodeContainers)
        {
            _onSuccess = onSuccess;
        }
        
        public override void Enter(LinkedList<int> selectedNodes)
        {
            base.Enter(selectedNodes);
            _onSuccess();
        }

        public override bool CheckNode(NodeContainer node, int selectedIndex, Vector2 touchPosition) 
            => true;
    }
}