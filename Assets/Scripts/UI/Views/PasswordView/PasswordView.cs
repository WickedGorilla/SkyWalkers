using System;
using System.Collections.Generic;
using UI.Core;
using UnityEngine;

namespace UI.Views
{
    public class PasswordView : View, IPasswordStateMachine
    {
        [SerializeField] private UILineRenderer _inputLineRenderer;
        [SerializeField] private UILineRenderer _previewLineRenderer;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _successColor;
        [SerializeField] private Color _errorColor;

        [SerializeField] private RectTransform _viewTransofrm;
        [SerializeField] private NodeContainer[] _nodeContainers;

        private PasswordState _currentState;
        private Dictionary<Type, PasswordState> _states;

        private void Awake()
        {
            _states = new Dictionary<Type, PasswordState>
            {
                [typeof(DefaultPasswordState)] = new DefaultPasswordState(_defaultColor, this, _inputLineRenderer,
                    _nodeContainers, Array.Empty<int>()),
                
                [typeof(SuccessPasswordState)] = new SuccessPasswordState(_defaultColor, this, _inputLineRenderer,
                    _nodeContainers),
                
                [typeof(ErrorPasswordState)] = new ErrorPasswordState(_errorColor, this, _inputLineRenderer,
                    _nodeContainers)
            };
        }

        public void Initialize(IEnumerable<int> passIndexes)
        {
            _previewLineRenderer.SetPoints(GetPointsByIndex(passIndexes));
            EnterState<DefaultPasswordState>(new LinkedList<int>());
        }

        public void EnterState<TState>(LinkedList<int> selectedNodes) where TState : PasswordState
        {
            if (!_states.TryGetValue(typeof(TState), out var state))
                throw new KeyNotFoundException($"No state by {typeof(TState)}");
            
            _currentState = state;
            _currentState.Enter(selectedNodes);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                ResetPattern();

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Input.mousePosition;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_viewTransofrm, mousePosition, null,
                    out _);

                for (int i = 0; i < _nodeContainers.Length; i++)
                {
                    var node = _nodeContainers[i];
                    
                    if (_currentState.CheckNode(node, i, mousePosition))
                        break;
                }

                _currentState.UpdateRender();
            }
        }

        private IEnumerable<Vector2> GetPointsByIndex(IEnumerable<int> passIndexes)
        {
            foreach (var index in passIndexes)
                yield return _nodeContainers[index].Position;
        }
        
        public void ResetPattern()
        {
            foreach (var index in _currentState.SelectedNodes)
                _nodeContainers[index].SetColor(_defaultColor);

            _currentState.SelectedNodes.Clear();
            _inputLineRenderer.ClearPoints();
            _previewLineRenderer.ClearPoints();
        }
    }
}