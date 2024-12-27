using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class DefaultPasswordState : PasswordState
    {
        private readonly int _passwordNodesMask;

        public DefaultPasswordState(Color selectColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers,
            IEnumerable<int> passIndexes)
            : base(selectColor, stateMachine,  lineRenderer, nodeContainers)
        {
            _passwordNodesMask = BitmaskHelper.GenerateBitmask(passIndexes);
        }

        public override void CheckNode(NodeContainer node, int index, Vector2 touchPosition)
        {
            if (BitmaskHelper.CheckContainsInBitmask(SelectedNodesMask, index))
                return;

            if (!RectTransformUtility.RectangleContainsScreenPoint(node.Circle, touchPosition)) 
                return;
            
            SelectedNodes.AddLast(index);
                
            if (!BitmaskHelper.CheckContainsInBitmask(_passwordNodesMask, index))
            {
                StateMachine.EnterState<ErrorPasswordState>(SelectedNodes);
                return;
            }
                
            node.SetColor(SelectColor);
        }
    }
}