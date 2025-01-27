using System;
using DG.Tweening;
using UI.Core;
using UI.CustomButtons;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.MiniGames.ConstructionView
{
    public class ConstructionView : View
    {
        [SerializeField] private ViewTimer _timer;
        [SerializeField] private UIButton _leftButton;
        [SerializeField] private UIButton _rightButton;
        [SerializeField] private Image _leftImage;
        [SerializeField] private Image _rightImage;
        [SerializeField] private Color _disableColor = Color.gray;
        [SerializeField] private Color _successColor = Color.green;
        [SerializeField] private Color _failColor = Color.red;
        [SerializeField] private Image _backgroundImage;

        private Image _selectedButton;

        public ViewTimer Timer => _timer;
        public UIButton LeftButton => _leftButton;
        public UIButton RightButton => _rightButton;

        public void HighlightButton(ConstructionViewController.ButtonDirectionType buttonDirectionType)
        {
            HighlightButton(buttonDirectionType == ConstructionViewController.ButtonDirectionType.Right
                ? (_rightImage, _rightButton)
                : (_leftImage, _leftButton));
        }

        private void HighlightButton((Image, UIButton) uiButton)
        {
            if (_selectedButton != null)
                _selectedButton.color = _disableColor;

            _selectedButton.DOKill();
            _selectedButton = uiButton.Item1;
            _selectedButton.color = Color.white;
            
            Color originalColor = _selectedButton.color;
            var tweener = _selectedButton.DOFade(0.5f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .OnKill(() => _selectedButton.color = originalColor);

            tweener.OnUpdate(() =>
            {
                if (uiButton.Item2.IsPressed)
                    tweener.Kill();
            });
        }

        public void VisualizeFail(Action onComplete)
        {
            _backgroundImage.DOColor(_failColor, 1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }

        public void VisualizeSuccess(Action onComplete)
        {
            _timer.SetParamText("1/1");
            _backgroundImage.DOColor(_successColor, 1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
    }
}