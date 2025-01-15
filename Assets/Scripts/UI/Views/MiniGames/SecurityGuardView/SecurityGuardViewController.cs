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
        
        public SecurityGuardViewController(SecurityGuardView view, 
            SecurityGuardMiniGameData miniGameData) : base(view)
        {
            _miniGameData = miniGameData;
        }

        public event Action<IEventAwaiter> OnCompleteMiniGame;
        public event Action<IEventAwaiter> OnFailMiniGame;
        
        public bool CheckIsComplete() 
            => _earnCoin == _miniGameData.EarnForComplete;

        public void DoFailMiniGame()
        {
            
        }

        public IUpdateTimer CreateTimer(Action onTimeLeft) 
            => View.Timer.CreateTimer(_miniGameData.TimeForMiniGame, onTimeLeft);
    }
}