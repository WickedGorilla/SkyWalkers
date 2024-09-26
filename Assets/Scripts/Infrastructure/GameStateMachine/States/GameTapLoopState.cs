using Game.Environment;
using Game.Player;
using UI.Hud;
using UI.ViewService;

namespace Game.Infrastructure
{
    public class GameTapLoopState : IState
    {
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly ViewService _viewService;
        private readonly PlayerMovementByTap _playerMovementByTap;

        public GameTapLoopState(IEnvironmentHolder environmentHolder,
            ViewService viewService,
            PlayerMovementByTap playerMovementByTap)
        {
            _environmentHolder = environmentHolder;
            _viewService = viewService;
            _playerMovementByTap = playerMovementByTap;
        }
        
        public void Enter()
        {
            _environmentHolder.Environment.ShowBuildingGroup();
            _playerMovementByTap.Subscribe();
            _viewService.Show<HudView, HudController>();
        }

        public void Exit()
        {
            _playerMovementByTap.Unsubscribe();
            _environmentHolder.Environment.ShowSitGroup();
        }
    }
}