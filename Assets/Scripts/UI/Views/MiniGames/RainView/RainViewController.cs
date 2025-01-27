using System;
using System.Threading;
using DG.Tweening;
using Game.Environment;
using Game.Player;
using Infrastructure.Actions;
using Infrastructure.Data.Game.MiniGames.RainMiniGame;
using SkyExtensions;
using UI.Core;
using UI.Views.Timer;
using UnityEngine;

namespace UI.Views.MiniGames.RainView
{
    public class RainViewController : ViewController<RainView>, IMiniGameViewController
    {
        private readonly RainMiniGameData _miniGameData;
        private readonly IEnvironmentHolder _environmentHolder;

        private PlayerAnimation _player;
        private float _forceUp;
        private CancellationTokenSource _cancellationTokenSource;
        private float _defaultPlayerPosition;
        
        public RainViewController(RainView view,
            RainMiniGameData miniGameData,
            IEnvironmentHolder environmentHolder) : base(view)
        {
            _miniGameData = miniGameData;
            _environmentHolder = environmentHolder;
        }

        public event Action<IEventAwaiter> OnCompleteMiniGame;
        public event Action<IEventAwaiter> OnFailMiniGame;

        private ParticleSystem RainParticle
            => _environmentHolder.Environment.RainParticle;

        private SpriteRenderer StopLine
            => _environmentHolder.Environment.StopLine;

        protected override void OnShow()
        {
            View.TapButton.AddClickAction(OnTapButton);

            _player = _environmentHolder.Environment.Player;
            _defaultPlayerPosition = _player.transform.position.y;
            
            RainParticle.Play();
            StopLine.color = View.LineColor;
            StopLine.gameObject.SetActive(true);
            DoFadeStopLine(1f);

            _cancellationTokenSource = new CancellationTokenSource();
        }

        public IUpdateTimer CreateTimer(Action onTimeLeft)
        {
            return View.Timer.CreateTimer(_miniGameData.TimeForMiniGame, OnTimeLeft);

            void OnTimeLeft()
            {
                SuccessMiniGame();
                onTimeLeft();
            }
        }

        protected override void OnHide()
        {
            StopLine.gameObject.SetActive(false);
        }

        private void OnTapButton()
        {
            _player.AnimateByClick();
            _forceUp += _miniGameData.ForceByClick;
        }

        public void StartWhenReady(Action onStart)
        {
            View.WaitForStart(() =>
            {
                onStart();
                OnUpdate(_cancellationTokenSource);
            });
        }

        private async void OnUpdate(CancellationTokenSource tokenSource)
        {
            while (!tokenSource.IsCancellationRequested)
            {
                UpdateMovement();
                CheckPositionForLine();

                await Awaitable.NextFrameAsync();
            }
        }

        private void UpdateMovement()
        {
            var currentPosition = _player.transform.position;
            float deltaY = _forceUp > 0 ? _forceUp * Time.deltaTime : 0;
            _forceUp = Mathf.Max(_forceUp - _miniGameData.ForceDecayRate * Time.deltaTime, 0);

            if (_forceUp == 0)
                deltaY -= _miniGameData.Gravity * Time.deltaTime;

            var newY = Mathf.Clamp(currentPosition.y + deltaY, _miniGameData.MinY, _miniGameData.MaxY);
            _player.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }

        private void CheckPositionForLine()
        {
            if (_player.transform.position.y > _miniGameData.FailPlayerPosition)
                return;

            DoFailMiniGame();
        }

        public bool CheckIsComplete()
            => true;

        public void DoFailMiniGame()
        {
            StopLine.color = View.FailLineColor;
            var animationAwaiter = new EventAwaiter();
            OnFailMiniGame?.Invoke(animationAwaiter);
            View.VisualizeFail(animationAwaiter.Complete);
            EndMiniGame();
        }

        private void SuccessMiniGame()
        {
            var animationAwaiter = new EventAwaiter();
            OnCompleteMiniGame?.Invoke(animationAwaiter);
            View.VisualizeSuccess(animationAwaiter.Complete);
            EndMiniGame();
        }
        
        private void EndMiniGame()
        {
            bool isMoved = false;
            
            _player.transform.DOMoveY(_defaultPlayerPosition, 
                View.DurationResultAnimation * 2)
                .OnComplete(() => isMoved = true);
            
            _player.AnimateWhile(() => isMoved);
            _cancellationTokenSource.Cancel();
            RainParticle.Stop();
            
            View.TapButton.RemoveClickAction(OnTapButton);
            _forceUp = default;
        }
        
        private void DoFadeStopLine(float endValue, Action onComplete = null)
            => StopLine.DOFade(endValue, 1f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => onComplete?.Invoke());
    }
}