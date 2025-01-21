using System;
using DG.Tweening;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.MiniGames.RainView
{
    public class RainView : View
    {
        [SerializeField] private Button _tapButton;
        [SerializeField] private ViewTimer _timer;
        [SerializeField] private Color _lineColor;
        [SerializeField] private Color _failLineColor;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private float _durationResultAnimation;
        
        public ViewTimer Timer => _timer;
        public Color LineColor => _lineColor;
        public Color FailLineColor => _failLineColor;
        public Button TapButton => _tapButton;

        public float DurationResultAnimation => _durationResultAnimation;

        public override void OnShow()
        {
           
        }

        public override void OnHide() 
            => _backgroundImage.color = Color.white;

        public void VisualizeFail(Action onComplete)
        {
            _backgroundImage.DOColor(_failLineColor, DurationResultAnimation)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
        
        public void VisualizeSuccess(Action onComplete)
        {
            _backgroundImage.DOColor(Color.green, DurationResultAnimation)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() => onComplete());
        }
    }
}