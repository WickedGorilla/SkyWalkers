using System;
using Infrastructure.Actions;
using UI.Views.Timer;

namespace UI.Views.MiniGames
{
    public interface IMiniGameViewController
    {
        event Action<IEventAwaiter> OnCompleteMiniGame;
        event Action<IEventAwaiter> OnFailMiniGame;
        bool CheckIsComplete();
        void DoFailMiniGame();
        IUpdateTimer CreateTimer(Action onTimeLeft);
    }
}