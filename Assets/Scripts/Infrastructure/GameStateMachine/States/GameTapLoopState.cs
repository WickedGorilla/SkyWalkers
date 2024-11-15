using Game.Environment;
using Game.Player;
using Game.Validation;
using UI.Hud;
using UI.Core;

namespace Game.Infrastructure
{
    public class GameTapLoopState : IState
    {
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly ViewService _viewService;
        private readonly PlayerMovementByTap _playerMovementByTap;
        private readonly CoinValidationService _coinValidationService;

        public GameTapLoopState(IEnvironmentHolder environmentHolder,
            ViewService viewService,
            PlayerMovementByTap playerMovementByTap, CoinValidationService coinValidationService)
        {
            _environmentHolder = environmentHolder;
            _viewService = viewService;
            _playerMovementByTap = playerMovementByTap;
            _coinValidationService = coinValidationService;
        }
        
        public void Enter()
        {
            _environmentHolder.Environment.ShowBuildingGroup();
            _playerMovementByTap.Subscribe();
            _viewService.Show<HudView, HudController>();
            _coinValidationService.Start();
        }

        public void Exit()
        {
            _playerMovementByTap.Unsubscribe();
            _environmentHolder.Environment.ShowSitGroup();
            _coinValidationService.Stop();
        }
    }
}