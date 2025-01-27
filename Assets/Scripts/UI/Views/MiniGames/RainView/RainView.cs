using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.MiniGames.RainView
{
    public class RainView : View
    {
        private readonly string[] _startTimerTexts = { "3", "2", "1", "TAP!" };

        [SerializeField] private Button _tapButton;
        [SerializeField] private ViewTimer _timer;
        [SerializeField] private Color _lineColor;
        [SerializeField] private Color _failLineColor;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private float _durationResultAnimation;
        [SerializeField] private TMP_Text _startTimeText;
        [SerializeField] private float _startWaitTime = 3f;

        public ViewTimer Timer => _timer;
        public Color LineColor => _lineColor;
        public Color FailLineColor => _failLineColor;
        public Button TapButton => _tapButton;

        public float DurationResultAnimation => _durationResultAnimation;

        public override void OnShow() 
            => _timer.SetParamText("0/1");

        public void WaitForStart(Action onComplete)
        {
            StartCoroutine(StartGame());

            IEnumerator StartGame()
            {
                var timeSegment = _startWaitTime / _startTimerTexts.Length; 

                _startTimeText.gameObject.SetActive(true);

                foreach (var text in _startTimerTexts)
                {
                    _startTimeText.text = text;
                    yield return new WaitForSeconds(timeSegment);
                }

                _startTimeText.gameObject.SetActive(false);
                onComplete();
            }
        }

        public void VisualizeFail(Action onComplete)
        {
            var defaultColor = _backgroundImage.color; 
            _backgroundImage.DOColor(_failLineColor, DurationResultAnimation)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _backgroundImage.color = defaultColor;
                    onComplete();
                });
        }

        public void VisualizeSuccess(Action onComplete)
        {
            _timer.SetParamText("1/1");
            var defaultColor = _backgroundImage.color; 
            _backgroundImage.DOColor(Color.green, DurationResultAnimation)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                {
                    _backgroundImage.color = defaultColor;
                    onComplete();
                });
        }
    }
}