using System;
using Infrastructure.Data.Game.MiniGames;
using UI.Core;
using UI.Views.MiniGames;

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
        
        public PasswordViewController(ViewService viewService, PasswordView view, PasswordMiniGameData miniGameData) : base(view)
        {
            _viewService = viewService;
            _miniGameData = miniGameData;
        }

        protected override void OnShow()
        {
            _currentRound = 1;
            _currentPassMistakes = 0;

            int[] idsPass = _miniGameData.GetRandomPassword().NodesIndexes;
            View.Initialize(_viewService.RootTransform, idsPass, _currentRound, _miniGameData.CountRounds);
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
            if (_currentRound == _miniGameData.CountRounds)
            {
                OnCompleteMiniGame?.Invoke();   
                return;
            }
            
            _currentRound++;
        }
        
        private void OnErrorPass()
        {
            _currentPassMistakes++;
            
            if (_currentPassMistakes == _miniGameData.CountMistakes)
            {
                OnFailMiniGame?.Invoke();
            }
        }
    }
}