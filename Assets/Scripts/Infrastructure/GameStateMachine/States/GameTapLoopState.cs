using Game.Environment;
using Game.MiniGames;
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
        private readonly FarmCoinsSystem _farmCoinsSystem;
        private readonly CoinValidationService _coinValidationService;
        private readonly MiniGamesSystem _miniGamesSystem;

        public GameTapLoopState(IEnvironmentHolder environmentHolder,
            ViewService viewService,
            FarmCoinsSystem farmCoinsSystem, 
            CoinValidationService coinValidationService,
            MiniGamesSystem miniGamesSystem)
        {
            _environmentHolder = environmentHolder;
            _viewService = viewService;
            _farmCoinsSystem = farmCoinsSystem;
            _coinValidationService = coinValidationService;
            _miniGamesSystem = miniGamesSystem;
        }
        
        public void Enter()
        {
            _environmentHolder.Environment.ShowBuildingGroup();
            _farmCoinsSystem.Subscribe();
            _viewService.Show<HudView, HudController>();
            
            _coinValidationService.Start();
            _miniGamesSystem.OnStart();
        }

        public void Exit()
        {
            _farmCoinsSystem.Unsubscribe();
            _environmentHolder.Environment.ShowSitGroup();
            _coinValidationService.Stop();
            _miniGamesSystem.OnStop();
        }
    }
}