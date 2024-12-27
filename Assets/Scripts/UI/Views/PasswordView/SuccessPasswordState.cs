using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class SuccessPasswordState : PasswordState
    {
        public SuccessPasswordState(Color selectColor,
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
            => true;
    }
}