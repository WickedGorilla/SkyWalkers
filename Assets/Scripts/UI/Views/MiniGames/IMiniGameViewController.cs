using System;
using UI.Views.Timer;

namespace UI.Views.MiniGames
{
    public interface IMiniGameViewController
    {
        event Action OnCompleteMiniGame;
        event Action OnFailMiniGame;
        IUpdateTimer CreateTimer(int time, Action onComplete);
        bool CheckIsComplete();
        void DoFailMiniGame();
    }
}