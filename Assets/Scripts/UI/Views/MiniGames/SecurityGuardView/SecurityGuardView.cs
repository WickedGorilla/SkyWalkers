using DG.Tweening;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.MiniGames.SecurityGuardView
{
    public class SecurityGuardView : View
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private ViewTimer _timer;

        [SerializeField] private float _minAlpha = 0.2f;
        [SerializeField] private float _maxAlpha = 0.4f;
        [SerializeField] private float _duration = 1f;
        
        public ViewTimer Timer => _timer;

        public override void OnShow()
        {
            AnimateAlpha(-1);
        }

        public override void OnHide()
        {
            _backgroundImage.DOKill();
        }

        private void AnimateAlpha(int loops)
        {
            _backgroundImage.enabled = true;
            Color initialColor = _backgroundImage.color;
            initialColor.a = _maxAlpha;
            _backgroundImage.color = initialColor;
            
            _backgroundImage.DOFade(_minAlpha, _duration)
                .SetEase(Ease.InOutSine)
                .SetLoops(loops, LoopType.Yoyo)
                .OnComplete(() => _backgroundImage.enabled = false);
        }
    }
}