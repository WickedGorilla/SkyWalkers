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
        [SerializeField] private Image _backgroundImage;

        private Image _selectedButton;

        public ViewTimer Timer => _timer;
        public UIButton LeftButton => _leftButton;
        public UIButton RightButton => _rightButton;
        
        public void HighlightButton(ConstructionViewController.ButtonDirectionType buttonDirectionType)
        {
            HighlightButton(buttonDirectionType == ConstructionViewController.ButtonDirectionType.Right
                ? _rightImage
                : _leftImage);
        }

        private void HighlightButton(Image imageButton)
        {
            if (_selectedButton != null)
                _selectedButton.color = _disableColor;
            
            _selectedButton = imageButton;
            _selectedButton.color = Color.white;
        }
        
        public void VisualizeFail(Action onComplete)
        {
            _backgroundImage.DOColor(Color.green, 1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
        
        public void VisualizeSuccess(Action onComplete)
        {
            _timer.SetParamText("1/1");
            _backgroundImage.DOColor(Color.green, 1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
    }
}