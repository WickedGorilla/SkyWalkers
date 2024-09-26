using Game.BuildingSystem;
using Game.Environment;
using Game.Player;
using Game.Wallet;
using Infrastructure.Network;
using Player;
using UI.Hud;
using Zenject;

namespace Game.Infrastructure
{
    public class BindGameState : IState
    {
        private readonly DiContainer _container;
        private readonly IGameStateMachine _gameStateMachine;

        public BindGameState(DiContainer container,
            IGameStateMachine gameStateMachine)
        {
            _container = container;
            _gameStateMachine = gameStateMachine;
        } 
        
        public void Enter()
        {
            BindInfrastructureServices();
            BindEnvironment();
            BindPlayer();
            
            _gameStateMachine.Enter<LoadGameState>();
        }

        public void Exit()
        {
            
        }

        private void BindInfrastructureServices()
        {
            _container.Bind<ServerRequestSender>().AsSingle();
        }

        private void BindPlayer()
        {
            _container.Bind<WalletService>().AsSingle();
            _container.Bind<BuildingMovementSystem>().AsSingle();
            _container.Bind<PlayerMovementByTap>().AsSingle();
            _container.BindInterfacesAndSelfTo<PlayerHolder>().AsSingle();
            _container.BindInterfacesAndSelfTo<ClickCoinSpawner>().AsSingle();
            _container.BindInterfacesAndSelfTo<CoinsCalculatorService>().AsSingle();
        }
        
        private void BindEnvironment()
        {
            _container.BindInterfacesAndSelfTo<EnvironmentHolder>().AsSingle();
        }
    }
}