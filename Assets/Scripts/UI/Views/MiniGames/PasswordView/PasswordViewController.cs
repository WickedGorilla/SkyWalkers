using System;
using Infrastructure.Data.Game.MiniGames;
using UI.Core;
using UI.Views.MiniGames;

namespace UI.Views
{
    public class PasswordViewController : ViewController<PasswordView>, IMiniGameViewController
    {
        private readonly PasswordMiniGameData _miniGameData;

        private int _currentRound;
        private int _currentPassMistakes;

        public event Action OnCompleteMiniGame;
        public event Action OnFailMiniGame;
        
        public PasswordViewController(PasswordView view, PasswordMiniGameData miniGameData) : base(view)
        {
            _miniGameData = miniGameData;
        }

        protected override void OnShow()
        {
            _currentRound = 1;
            _currentPassMistakes = 0;
            
            View.Initialize(_miniGameData.GetRandomPassword().NodesIndexes, _currentRound, _miniGameData.CountRounds);
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