using System;
using Infrastructure.Actions;
using Infrastructure.Data.Game.MiniGames.SecurityGuardMiniGame;
using UI.Core;
using UI.Views.Timer;

namespace UI.Views.MiniGames.SecurityGuardView
{
    public class SecurityGuardViewController : ViewController<SecurityGuardView>, IMiniGameViewController
    {
        private readonly SecurityGuardMiniGameData _miniGameData;

        private int _earnCoin;
        private int _mistakes;
        
        public SecurityGuardViewController(SecurityGuardView view, 
            SecurityGuardMiniGameData miniGameData) : base(view)
        {
            _miniGameData = miniGameData;
        }

        public event Action<IEventAwaiter> OnCompleteMiniGame;
        public event Action<IEventAwaiter> OnFailMiniGame;

        protected override void OnShow()
        {
            View.OnEarnSuccess += OnEarnSuccess;
            View.OnEarnMistake += OnEarnMistake;
        }

        protected override void OnHide()
        {
            View.OnEarnSuccess -= OnEarnSuccess;
            View.OnEarnMistake -= OnEarnMistake;
        }
        
        public bool CheckIsComplete() 
            => _earnCoin == _miniGameData.EarnForComplete;

        public void DoFailMiniGame()
        {
            var animationAwaiter = new EventAwaiter();
            OnFailMiniGame?.Invoke(animationAwaiter);
            View.VisualizeFail(animationAwaiter.Complete);
        }

        public IUpdateTimer CreateTimer(Action onTimeLeft) 
            => View.Timer.CreateTimer(_miniGameData.TimeForMiniGame, onTimeLeft);
        
        private void OnEarnMistake()
        {
            _mistakes++;

            if (_mistakes == _miniGameData.CountMistakeForFail)
                DoFailMiniGame();
        }

        private void OnEarnSuccess()
        {
            _earnCoin++;
            View.Timer.SetParamText($"{_earnCoin}/{_miniGameData.EarnForComplete}");

            if (_earnCoin != _miniGameData.EarnForComplete)
                return;
            
            var animationAwaiter = new EventAwaiter();
            View.VisualizeSuccess(animationAwaiter.Complete);
            OnCompleteMiniGame?.Invoke(animationAwaiter);
        }
    }
}