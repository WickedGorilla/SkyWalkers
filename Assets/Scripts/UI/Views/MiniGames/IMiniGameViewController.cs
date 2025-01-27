using System;
using Infrastructure.Actions;
using UI.Views.Timer;

namespace UI.Views.MiniGames
{
    public interface IMiniGameViewController
    {
        event Action<IEventAwaiter> OnCompleteMiniGame;
        event Action<IEventAwaiter> OnFailMiniGame;

        IUpdateTimer CreateTimer(Action onTimeLeft);

        virtual void StartWhenReady(Action onStart) 
            => onStart();

        bool CheckIsComplete();
        void DoFailMiniGame();
    }
}