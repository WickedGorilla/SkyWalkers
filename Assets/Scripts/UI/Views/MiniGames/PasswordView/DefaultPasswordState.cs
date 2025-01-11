using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Views
{
    public class DefaultPasswordState : PasswordState
    {
        private readonly Func<int[]> _getPassIndexes;
        private int[] _password;

        public DefaultPasswordState(Color selectedColor,
            IPasswordStateMachine stateMachine,
            UILineRenderer lineRenderer,
            NodeContainer[] nodeContainers,
            Func<int[]> getPassIndexes)
            : base(selectedColor, stateMachine, lineRenderer, nodeContainers)
        {
            _getPassIndexes = getPassIndexes;
        }

        public override void Enter(LinkedList<int> selectedNodes)
        {
            _password = _getPassIndexes();
            base.Enter(selectedNodes);
        }

        public override bool CheckNode(NodeContainer node, int selectedIndex, Vector2 touchPosition)
        {
            if (BitmaskHelper.CheckContainsInBitmask(SelectedNodesMask, selectedIndex))
                return false;

            if (!RectTransformUtility.RectangleContainsScreenPoint(node.Circle, touchPosition))
                return false;

            AddSelected(selectedIndex);

            var currentInputIndex = SelectedNodes.Count - 1;

            if (currentInputIndex < 1)
                return true;
            
            if (!ValidatePass(currentInputIndex, selectedIndex))
            {
                StateMachine.EnterState<ErrorPasswordState>(SelectedNodes);
                return true;
            }

            if (SelectedNodes.Count == _password.Length)
            {
                StateMachine.EnterState<SuccessPasswordState>(SelectedNodes);
                return true;
            }

            if (currentInputIndex == 1)
                UpdateColor(SelectedNodes, SelectedColor);
            else
                node.SetColor(SelectedColor);

            return true;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _password = Array.Empty<int>();
        }

        private bool ValidatePass(int offset, int index)
        {
            if (offset >= _password.Length)
                return false;

            if (_password[offset] == index)
                return true;

            int reverseOffset = _password.Length - 1 - offset;
            if (reverseOffset >= 0 && _password[reverseOffset] == index)
                return true;

            return false;
        }
    }
}