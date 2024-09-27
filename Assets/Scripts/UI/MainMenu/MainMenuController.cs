using Game.Infrastructure;
using Player;
using UI.ViewService;

namespace UI.MainMenu
{
    public class MainMenuController : ViewController<MainMenuView>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly WalletService _walletService;

        public MainMenuController(MainMenuView view, 
            IGameStateMachine gameStateMachine,
            WalletService walletService) : base(view)
        {
            _gameStateMachine = gameStateMachine;
            _walletService = walletService;
        }

        protected override void OnShow()
        {
            View.PlayButton.onClick.AddListener(OnClickPlay);
            View.Initialize(_walletService.Coins, "User001");
        }

        protected override void OnHide()
        {
            View.PlayButton.onClick.RemoveListener(OnClickPlay);
        }

        private void OnClickPlay() 
        {
            _gameStateMachine.Enter<GameTapLoopState>();
        }
    }
}