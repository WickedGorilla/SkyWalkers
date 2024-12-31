using System;

namespace UI.Views.MiniGames
{
    public interface IMiniGameViewController
    {
        event Action OnCompleteMiniGame;
        event Action OnFailMiniGame;
    }
}