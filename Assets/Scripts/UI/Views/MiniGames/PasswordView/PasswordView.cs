using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class PasswordView : View, IPasswordStateMachine
    {
        [SerializeField] private TMP_Text _numberGameText;
        [SerializeField] private string _numberGameTextUndo;
        [SerializeField] private string _numberGameTextUntil;

        [Header("Line render")]
        [SerializeField] private UILineRenderer _inputLineRenderer;
        [SerializeField] private UILineRenderer _previewLineRenderer;

        [Header("Background settings")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private float minAlpha = 0.2f;
        [SerializeField] private float maxAlpha = 0.4f;
        [SerializeField] private float duration = 1f;

        [Header("Colors")] 
        [SerializeField] private Color _whiteColor;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _successColor;
        [SerializeField] private Color _errorColor;

        [Space] 
        [SerializeField] private NodeContainer[] _nodeContainers;
        [SerializeField] private ViewTimer _timer;

        private RectTransform _canvasTransform;
        private PasswordState _currentState;
        private Dictionary<Type, PasswordState> _states;
        private int[] _passIndexes;

        public event Action OnCompletePass;
        public event Action OnErrorPass;
        public ViewTimer Timer => _timer;

        
        private void Awake()
        {
            _passIndexes = Array.Empty<int>();

            _states = new Dictionary<Type, PasswordState>
            {
                [typeof(DefaultPasswordState)] = new DefaultPasswordState(_defaultColor, this, _inputLineRenderer,
                    _nodeContainers, () => _passIndexes),

                [typeof(SuccessPasswordState)] = new SuccessPasswordState(_successColor, this, _inputLineRenderer,
                    _nodeContainers, () => OnCompletePass?.Invoke()),

                [typeof(ErrorPasswordState)] = new ErrorPasswordState(_errorColor, this, _inputLineRenderer,
                    _nodeContainers, () => OnErrorPass?.Invoke())
            };
        }

        public void Initialize(RectTransform canvasTransform, int[] passIndexes, int currentRound, int totalRounds)
        {
            _passIndexes = passIndexes;
            _canvasTransform = canvasTransform;
            UpdateRoundText(currentRound, totalRounds);

            _previewLineRenderer.SetPoints(GetPointsByIndex(_passIndexes));
            EnterState<DefaultPasswordState>(new LinkedList<int>());
        }

        private void UpdateRoundText(int currentRound, int totalRounds)
        {
            _numberGameText.text = $"{_numberGameTextUndo} {currentRound}/{totalRounds}\n{_numberGameTextUntil}";

            string text = $"{currentRound}/{totalRounds}";
            Timer.SetParamText(text);
        }

        public void EnterState<TState>(LinkedList<int> selectedNodes) where TState : PasswordState
        {
            if (!_states.TryGetValue(typeof(TState), out var state))
                throw new KeyNotFoundException($"No state by {typeof(TState)}");

            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter(selectedNodes);
        }

        private void Update()
        {
            if (GetTouchUp())
                _currentState?.OnEndInput();
            
            if (GetTouchDown())
                ResetPattern();

            if (!GetTouch(out Vector2 touchPosition))
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, touchPosition, null,
                out _);

            for (int i = 0; i < _nodeContainers.Length; i++)
            {
                var node = _nodeContainers[i];

                if (_currentState.CheckNode(node, i, touchPosition))
                    break;
            }

            _currentState.UpdateRender();
        }

        private IEnumerable<Vector2> GetPointsByIndex(IEnumerable<int> passIndexes)
        {
            foreach (var index in passIndexes)
                yield return _nodeContainers[index].Position;
        }

        public void ResetPattern()
        {
            foreach (var index in _currentState.SelectedNodes)
                _nodeContainers[index].SetColor(_whiteColor);

            _currentState.SelectedNodes.Clear();
            _inputLineRenderer.ClearPoints();
        }

        private bool GetTouchDown()
        {
            if (Input.touchCount == 0)
                return Input.GetMouseButtonDown(0);

            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }

        private bool GetTouchUp()
        {
            if (Input.touchCount == 0)
                return Input.GetMouseButtonUp(0);

            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }
        
        private bool GetTouch(out Vector2 position)
        {
            if (Input.touchCount == 0)
            {
                var isMouseDown = Input.GetMouseButton(0);
                position = isMouseDown ? Input.mousePosition : Vector2.zero;
                return isMouseDown;
            }

            Touch touch = Input.GetTouch(0);

            var isTouch = touch.phase is TouchPhase.Moved or TouchPhase.Stationary;
            position = isTouch ? touch.position : Vector2.zero;
            return isTouch;
        }

        public void CompletePass(Action onAnimationEnd)
        {
            _backgroundImage.color = _successColor;

            AnimateAlpha(1, OnComplete);
            
            void OnComplete()
            {
                ResetPattern();
                onAnimationEnd();
            }
        }

        public void ErrorPass()
        {
            _backgroundImage.color = _errorColor;
            AnimateAlpha(1, OnComplete);
            
            void OnComplete()
            {
                ResetPattern();
                EnterState<DefaultPasswordState>(_currentState.SelectedNodes);
            }
        }

        public void FailPass(Action animationEnd)
        {
            _backgroundImage.color = _errorColor;
            AnimateAlpha(1, OnComplete);
            
            void OnComplete()
            {
                ResetPattern();
                animationEnd?.Invoke();
            }
        }
        
        private void AnimateAlpha(int loops, Action onComplete)
        {
            _backgroundImage.enabled = true;
            Color initialColor = _backgroundImage.color;
            initialColor.a = maxAlpha;
            _backgroundImage.color = initialColor;
            
            _backgroundImage.DOFade(minAlpha, duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(loops, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _backgroundImage.enabled = false;
                    onComplete();
                });
        }
    }
}