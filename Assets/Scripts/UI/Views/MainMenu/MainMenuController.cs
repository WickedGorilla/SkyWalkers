using Game.Environment;
using Game.Infrastructure;
using Player;
using UI.Core;
using UnityEngine;

namespace UI.Views
{
    public class MainMenuController : ViewController<MainMenuView>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly WalletService _walletService;
        private readonly IEnvironmentHolder _environmentHolder;

        public MainMenuController(MainMenuView view,
            IGameStateMachine gameStateMachine,
            WalletService walletService, IEnvironmentHolder environmentHolder) : base(view)
        {
            _gameStateMachine = gameStateMachine;
            _walletService = walletService;
            _environmentHolder = environmentHolder;
        }

        protected override void OnShow()
        {
            View.PlayButton.onClick.AddListener(OnClickPlay);
            View.Initialize(_walletService.Coins, $"User{Random.Range(126, 999)}");
        }

        protected override void OnHide()
        {
            View.PlayButton.onClick.RemoveListener(OnClickPlay);
        }

        private void OnClickPlay()
        {
            if (_environmentHolder.Environment.Animated)
                return;
            
            _gameStateMachine.Enter<GameTapLoopState>();
        }
    }
}