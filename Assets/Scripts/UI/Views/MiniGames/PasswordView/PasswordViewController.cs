using System;
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

        private int _currentRound;
        private int _currentPassMistakes;

        public event Action OnCompleteMiniGame;
        public event Action OnFailMiniGame;
        
        public IUpdateTimer CreateTimer(int time, Action onComplete) 
            => View.Timer.CreateTimer(time, onComplete);

        public bool CheckIsComplete() 
            => _currentRound == _miniGameData.CountRounds;

        public void DoFailMiniGame() 
            => OnErrorPass();

        public PasswordViewController(ViewService viewService, 
            PasswordView view,
            PasswordMiniGameData miniGameData) : base(view)
        {
            _viewService = viewService;
            _miniGameData = miniGameData;
        }

        protected override void OnShow()
        {
            _currentRound = 1;
            _currentPassMistakes = 0;

            UpdateRoundOnTheView(_currentRound);
            
            View.OnCompletePass += OnCompletePass;
            View.OnErrorPass += OnErrorPass;
        }

        protected override void OnHide()
        {
            View.ResetPattern();
            
            View.OnCompletePass -= OnCompletePass;
            View.OnErrorPass -= OnErrorPass;
        }

        private void OnCompletePass()
        {
            if (CheckIsComplete())
            {
                OnCompleteMiniGame?.Invoke();   
                return;
            }
            
            UpdateRoundOnTheView(_currentRound++);
        }

        private void UpdateRoundOnTheView(int currentRound)
        {
            var idsPass = _miniGameData.GetRandomPassword().NodesIndexes;
            View.Initialize(_viewService.RootTransform, idsPass, currentRound, _miniGameData.CountRounds);
        }
        
        private void OnErrorPass()
        {
            _currentPassMistakes++;
            
            if (_currentPassMistakes == _miniGameData.CountMistakes)
                OnFailMiniGame?.Invoke();
        }

    }
}