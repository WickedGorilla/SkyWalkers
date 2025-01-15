using System;
using Game.Environment;
using Infrastructure.Actions;
using Infrastructure.Data.Game.MiniGames;
using UI.Core;
using UI.Views.MiniGames;
using UI.Views.Timer;

namespace UI.Views
{
    public class PasswordViewController : ViewController<PasswordView>, IMiniGameViewController
    {
        private readonly ViewService _viewService;
        private readonly PasswordMiniGameData _miniGameData;
        private readonly IEnvironmentHolder _environmentHolder;

        private int _currentRound;
        private int _currentPassMistakes;

        public event Action<IEventAwaiter> OnCompleteMiniGame;
        public event Action<IEventAwaiter> OnFailMiniGame;

        public Quadrocopter Quadrocopter => _environmentHolder.Environment.Quadrocopter;

        public PasswordViewController(ViewService viewService,
            PasswordView view,
            PasswordMiniGameData miniGameData,
            IEnvironmentHolder environmentHolder) : base(view)
        {
            _viewService = viewService;
            _miniGameData = miniGameData;
            _environmentHolder = environmentHolder;
        }

        public IUpdateTimer CreateTimer(int time, Action onComplete)
            => View.Timer.CreateTimer(time, onComplete);

        public bool CheckIsComplete()
            => _currentRound == _miniGameData.CountRounds;

        public void DoFailMiniGame()
        {
            var animationAwaiter = new EventAwaiter();
            OnFailMiniGame?.Invoke(animationAwaiter);
            View.FailPass(animationAwaiter.Complete);
        }

        public IUpdateTimer CreateTimer(Action onTimeLeft) 
            => View.Timer.CreateTimer(_miniGameData.TimeForMiniGame, onTimeLeft);

        protected override void OnShow()
        {
            _currentRound = 1;
            _currentPassMistakes = 0;

            UpdateRoundOnTheView(_currentRound);

            Quadrocopter.DoShow();
            
            View.OnCompletePass += OnCompletePass;
            View.OnErrorPass += OnErrorPass;
        }

        protected override void OnHide()
        {
            View.ResetPattern();

            Quadrocopter.DoHide();
            
            View.OnCompletePass -= OnCompletePass;
            View.OnErrorPass -= OnErrorPass;
        }

        private void OnCompletePass()
        {
            if (CheckIsComplete())
            {
                var animationAwaiter = new EventAwaiter();
                OnCompleteMiniGame?.Invoke(animationAwaiter);
                View.CompletePass(animationAwaiter.Complete);
                return;
            }

            _currentRound++;
            View.CompletePass(() => UpdateRoundOnTheView(_currentRound));
        }

        private void OnErrorPass()
        {
            _currentPassMistakes++;

            if (_currentPassMistakes != _miniGameData.CountMistakes)
            {
                View.ErrorPass();
                return;
            }

            DoFailMiniGame();
        }

        private void UpdateRoundOnTheView(int currentRound)
        {
            var idsPass = _miniGameData.GetRandomPassword().NodesIndexes;
            View.Initialize(_viewService.RootTransform, idsPass, currentRound, _miniGameData.CountRounds);
        }
    }
}