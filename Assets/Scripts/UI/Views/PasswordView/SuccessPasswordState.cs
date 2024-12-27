using UnityEngine;

namespace UI.Views
{
    public class SuccessPasswordState : PasswordState
    {
        public SuccessPasswordState(Color selectColor, IPasswordStateMachine stateMachine, UILineRenderer lineRenderer, NodeContainer[] nodeContainers) : base(selectColor, stateMachine, lineRenderer, nodeContainers)
        {
        }

        public override void CheckNode(NodeContainer node, int index, Vector2 touchPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}